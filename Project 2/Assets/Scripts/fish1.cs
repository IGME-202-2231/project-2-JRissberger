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
    float time = 2f;

    [SerializeField]
    float radius = 0.5f;

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
    float separateRange = 1f;

    [SerializeField]
    float avoidTime = 1f;

    //weights
    float wanderWeight = 10f;
    float boundsWeight = 8f;
    float cohesionWeight = 5f;
    float separateWeight = 1f;

    Vector3 direction = Vector3.zero;

    Vector3 totalForce = Vector3.zero;

    public List<obstacle> obstacles;

    Vector3 flockDirection = Vector3.zero;

    //"timer" for checking for state change
    float timer = 1f;

    void Start()
    {
        //maybe finds the manager?
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
            totalForce += Separate() * separateWeight;
            totalForce = totalForce + (Cohesion() * cohesionWeight);

            //TODO: checking flee status with jellyfish

            ApplyForce(totalForce);

            //reduces timer each second
            /*timer -= 1 * Time.deltaTime;
            

            //checks if timer is zero, changes state if so
            if (timer <= 0)
            {
                state = "starving";
            } */
        }

        //Seeking food
        /*if (state == "starving")
        {
            //lower wander rate
            //no cohesion
            //slower speed--cut applied force in half
            //search for food
            //picks one off list
            //seeks it
            
        } */

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
        //Calls the other version of Seek
        //which returns the seeking steering force
        //and then return that returned vector.
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

        for(int i = 0; i < manager.fish1GOList.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, manager.fish1GOList[i].transform.position);

            if (Mathf.Epsilon < dist)
            {
                steeringForce += Flee(manager.fish1GOList[i].transform.position) * (separateRange / (dist));
            }
        }

        return steeringForce;
    }

    //Flocking
    protected Vector3 Cohesion()
    {
        if (manager.fish1List.Count > 1)
        {
            Vector3 centerPoint = Vector3.zero;

            for (int i = 0; i < manager.fish1List.Count; i++)
            {
                //Avoids checking self
                if (manager.fish1List[i].transform.position == transform.position)
                {
                    centerPoint += Vector3.zero;
                }
                else
                {
                    centerPoint += manager.fish1List[i].transform.position;
                }
            }

            centerPoint /= manager.fish1List.Count;

            return Seek(centerPoint);
        }
        else
        {
            return Vector3.zero;
        }
    }

   /* protected Vector3 Alignment()
    {
        if (manager.fish1GOList.Count > 1)
        {
            Vector3 flockDirection = Vector3.zero;

            foreach (Agent agent in manager.agents)
            {
                //Commented out, need direction getter in myPhysicsObject
                //flockDirection += agent.myPhysicsObject.Direction;

                //to skip checkng self, check if distance is 0
            }

            flockDirection /= manager.agents.Count;

            return flockDirection - myPhysicsObject.Velocity;
        }
        else
        {
            return Vector3.zero;
        }
    } */
    /*private void OnDrawGizmosSelected()
    {
        Vector3 futurePos = CalcFuturePosition(time);

        float dist = Vector3.Distance(transform.position, futurePos);
        Vector3 boxSize = new Vector3(radius * 2f, dist, radius * 2f);
        Vector3 boxCenter = transform.position; //Vector3.zero;
        boxCenter.y += dist / 2f;

        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        
    } */

    //method for eating food
    //loop through food list, check if distance to one is zero
    //if so, change fish state, reset hunger timer, destroy object from list. check abt specifics for making it null. should
    //be as easy as just checking the spot on the list.
}
