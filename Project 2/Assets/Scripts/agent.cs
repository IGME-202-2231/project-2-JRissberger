using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agent : MonoBehaviour
{
    //reference to manager
    [SerializeField]
    public GameObject managerGO;
    public AgentManager manager;

    [SerializeField]
    public float time = 2f;

    [SerializeField]
    public float radius = 0.5f;

    [SerializeField]
    public Vector3 position;

    [SerializeField]
    public Vector3 velocity;

    [SerializeField]
    public Vector3 acceleration = Vector3.zero;

    [SerializeField]
    public float mass = 1;

    [SerializeField]
    public float maxSpeed = 10;

    [SerializeField]
    public float maxForce = 10;

    [SerializeField]
    public float separateRange = 1f;

    [SerializeField]
    public float avoidTime = 1f;
    public float avoidDist = 0f;

    public Vector3 direction = Vector3.zero;

    public List<Vector3> foundObstacles = new List<Vector3>();

    //TODO: add spriteRenderer

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

    //Keeping objects in bounds
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

    //Cohesion of agents for fish1
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

    //Checks if any obstacles need to be avoided
    protected Vector3 AvoidObstacles(float avoidRange)
    {
        //total force to steer away from obstacles
        Vector3 totalAvoidForce = Vector3.zero;

        Vector3 futurePos = CalcFuturePosition(avoidTime);

        avoidDist = Vector3.Distance(transform.position, futurePos) + radius;

        //Makes sure obstacle list is clear
        foundObstacles.Clear();

        //loops through obstacle list
        for (int i = 0; i < manager.obstacles.Count; i++)
        {
            Vector3 aToO = manager.obstacles[i].position - transform.position;
            float rightDot = 0;
            float forwardDot = 0;

            //calculates dots
            forwardDot = Vector3.Dot(direction, aToO);

            //check if forward
            if (forwardDot >= 0)
            {
                //checking if within range
                if (forwardDot <= avoidDist)
                {
                    //agent needs to be looking in its movement direction.
                    rightDot = Vector3.Dot(transform.right, aToO);

                    if (Mathf.Abs(rightDot) <= radius + manager.obstacles[i].radius)
                    {
                        //Add found obstacles to list
                        foundObstacles.Add(manager.obstacles[i].position);

                        //calc steering around this obstacle
                        if (rightDot >= 0)
                        {
                            //on right side
                            totalAvoidForce -= transform.right * maxSpeed;
                        }
                        else
                        {
                            //on left side
                            totalAvoidForce += transform.right * maxSpeed;
                        }
                    }
                }
            }
        }
        return totalAvoidForce;
    }
}
