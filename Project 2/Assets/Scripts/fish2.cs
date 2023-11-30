using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish2 : MonoBehaviour
{
    //tracks current state
    //updates and behavior changes accordingly
    private string state = "wander";

    //Reference to manager
    [SerializeField]
    GameObject managerGO;

    AgentManager manager;

    [SerializeField]
    float time = 2f;

    [SerializeField]
    float radius = 1f;

    [SerializeField]
    private Vector3 position;

    [SerializeField]
    private Vector3 velocity;

    [SerializeField]
    private Vector3 acceleration = Vector3.zero;

    [SerializeField]
    float mass = 1;

    [SerializeField]
    float maxSpeed = 10;

    [SerializeField]
    float maxForce = 10;

    [SerializeField]
    float separateRange = 3f;

    [SerializeField]
    float avoidTime = 1f;

    //weights
    float wanderWeight = 10f;
    float boundsWeight = 8f;
    float separateWeight = 4f;
    float chaseWeight = 10f;

    Vector3 direction = Vector3.zero;

    Vector3 totalForce = Vector3.zero;

    public List<obstacle> obstacles;

    //Target for chasing
    GameObject target = null;

    //timer for chase duration
    float timer = 3f;

    void Start()
    {
        //gets the manager object
        managerGO = GameObject.Find("Manager");

        manager = managerGO.GetComponent<AgentManager>();

        //Sets a random time for timer
        timer = Random.Range(5, 20);
    }

    void Update()
    {
        //wandering movement
        if (state == "wander")
        {
            totalForce += (Wander(time, radius)) * wanderWeight;
            totalForce += StayInBounds() * boundsWeight;
            totalForce = totalForce + (Separate() * separateWeight);
            ApplyForce(totalForce);

            //Checks to see if chase state can be entered
            target = fishCheck();
        }

        //Targets fish
        if (state == "chase")
        {
            totalForce += Seek(target.transform.position) * chaseWeight;
            totalForce += Separate() * separateWeight;
            ApplyForce(totalForce);

            //reduces timer
            timer -= 1f * Time.deltaTime;

            //Checks if timer has expired
            if (timer <= 0f)
            {
                //sets maxspeed back to original
                maxSpeed = 2f;
                state = "wander";

                //resets timer for next use
                timer = 3f;

                //TODO: Add cooldown timer for this state so they aren't constantly in it
            }
        }

        // Calculate the velocity for this frame
        velocity += acceleration * Time.deltaTime;

        //Cap max velocity
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        position += velocity * Time.deltaTime;

        // Grab current direction from velocity
        direction = velocity.normalized;

        transform.position = position;

        // Zero out acceleration
        acceleration = Vector3.zero;

        totalForce = Vector3.zero;
    }

    //applies forces to object
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    public Vector3 Seek(Vector3 targetPos)
    {
        //Calculate desired velocity
        Vector3 desiredVelocity = targetPos - transform.position;

        //Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //Calculate seek steering force
        Vector3 seekingForce = desiredVelocity - velocity;

        //Return seek steering force
        return seekingForce;
    }

    //Seeking object
    public Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    //General random movement
    protected Vector3 Wander(float time, float radius)
    {
        Vector3 targetPos = CalcFuturePosition(time);

        float randAngle = Random.Range(0, Mathf.PI * 2f);

        targetPos.x += Mathf.Cos(randAngle) * radius;
        targetPos.y += Mathf.Sin(randAngle) * radius;

        return Seek(targetPos);
    }

    //Fleeing from object
    protected Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = transform.position - targetPos;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 fleeingforce = desiredVelocity - velocity;

        return fleeingforce;
    }

    protected Vector3 Flee(GameObject target)
    {
        return Flee(target.transform.position);
    }

    public Vector3 CalcFuturePosition(float time)
    {
        return velocity * time + transform.position;
    }

    //Keeping objects in bounds. working in tandem with checkbounds
    protected Vector3 StayInBounds()
    {
        //If out of bounds
        if (transform.position.x <= -8 ||
            transform.position.x >= 8 ||
            transform.position.y <= -3.5f ||
            transform.position.y >= 3.5f)
        {
            return Seek(Vector3.zero);
        }

        return Vector3.zero;
    }

    //Split from other objects
    protected Vector3 Separate()
    {
        Vector3 steeringForce = Vector3.zero;

        for (int i = 0; i < manager.fish2GOList.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, manager.fish2GOList[i].transform.position);

            if (Mathf.Epsilon < dist)
            {
                steeringForce += Flee(manager.fish2GOList[i].transform.position) * (separateRange / dist);
            }
        }

        return steeringForce;
    }

    //Checks to see if any fish are nearby. changes agent state if so
    protected GameObject fishCheck()
    {
        //loop through list of fish
        //Check for distance
        //distance under a certain amount? trigger chase status
        //chase increases max speed
        //returns vector3 of fish position
        for (int i = 0; i < manager.fish1GOList.Count; i++)
        {
            //calculates distance between agent and fish
            float dist = Vector3.Distance(transform.position, manager.fish1GOList[i].transform.position);

            //updates if chase can be activated
            if (dist <= 0.5)
            {
                //increases max speed
                maxSpeed = 4f;
                state = "chase";

                //returns target object
                return manager.fish1GOList[i];
            }
        }

        //returns null otherwise
        return null;
    }

}
