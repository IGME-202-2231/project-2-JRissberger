using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish1 : MonoBehaviour
{
    //tracks current state
    //updates and behavior changes accordingly
    private string state = "wander";

    //Reference to manager
    [SerializeField]
    GameObject managerGO;

    AgentManager manager;

    [SerializeField]
    float time = 1f;

    [SerializeField]
    float radius = 1f;

    [SerializeField]
    private Vector3 position;

    [SerializeField]
    private Vector3 direction;

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
    float separateRange = 1f;

    [SerializeField]
    float avoidTime = 1f;

    Vector3 totalForce = Vector3.zero;

    void Start()
    {
        //maybe finds the manager?
        managerGO = GameObject.Find("Manager");

        manager = managerGO.GetComponent<AgentManager>();
    }

    void Update()
    {
        totalForce += Separate() * 0.1f;

        //wandering movement
        if (state == "wander")
        {
            totalForce += (Wander(time, radius));
            totalForce += StayInBounds() * 0.3f;
            ApplyForce(totalForce);
        }

        // Calculate the velocity for this frame
        velocity += acceleration * Time.deltaTime;

        //Cap max velocity
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        CheckBounds();

        position += velocity * Time.deltaTime;

        // Grab current direction from velocity
        direction = velocity.normalized;

        transform.position = position;

        // Zero out acceleration
        acceleration = Vector3.zero;
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void CheckBounds()
    {
        //Left/Right
        if (position.x <= -9)
        {
            velocity.x *= -1f;

            position.x = -9;
        }
        else if (position.x >= 9)
        {
            velocity.x *= -1f;

            position.x = 9;
        }

        //Up/Down
        if (position.y <= -4)
        {
            velocity.y *= -1f;

            position.y = -4;
        }
        else if (position.y >= 4)
        {
            velocity.y *= -1f;

            position.y = 4;
        }
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

    //may need updates
    public Vector3 Seek(GameObject target)
    {
        //Calls the other version of Seek
        //which returns the seeking steering force
        //and then return that returned vector.
        return Seek(target.transform.position);
    }

    protected Vector3 Wander(float time, float radius)
    {
        Vector3 targetPos = CalcFuturePosition(time);

        float randAngle = Random.Range(0, Mathf.PI * 2f);

        targetPos.x += Mathf.Cos(randAngle) * radius;
        targetPos.y += Mathf.Sin(randAngle) * radius;

        return Seek(targetPos);
    }

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


    protected Vector3 Separate()
    {
        Vector3 steeringForce = Vector3.zero;

        for(int i = 0; i < manager.fish1GOList.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, manager.fish1GOList[i].transform.position);

            if (Mathf.Epsilon < dist)
            {
                steeringForce += Flee(manager.fish1GOList[i].transform.position) * (separateRange / dist);
            }
        }

        return steeringForce;
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 futurePos = CalcFuturePosition(time);

        float dist = Vector3.Distance(transform.position, futurePos);
        Vector3 boxSize = new Vector3(radius * 2f, dist, radius * 2f);
        Vector3 boxCenter = transform.position; //Vector3.zero;
        boxCenter.y += dist / 2f;

        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        
    }

}
