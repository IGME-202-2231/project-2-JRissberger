using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish2 : agent
{
    //tracks current state
    //updates and behavior changes accordingly
    private string state = "wander";

    //weights
    float wanderWeight = 10f;
    float boundsWeight = 8f;
    float separateWeight = 5f;
    float chaseWeight = 10f;

    Vector3 totalForce = Vector3.zero;

    public List<obstacle> obstacles;

    //Target for chasing
    GameObject target = null;

    //timer for chase duration
    float chaseTimer = 3f;

    //timer must be expired before chase can happen again
    float cooldownTimer = 0f;

    void Start()
    {
        //finds the manager
        managerGO = GameObject.Find("Manager");

        manager = managerGO.GetComponent<AgentManager>();
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

            //decreases cooldown timer if applicable
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= 1f * Time.deltaTime;
            }

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
            chaseTimer -= 1f * Time.deltaTime;

            //Checks if timer has expired
            if (chaseTimer <= 0f)
            {
                //sets maxspeed back to original
                maxSpeed = 2f;
                state = "wander";

                //resets timer for next use
                chaseTimer = 3f;

                //Sets cooldown timer
                cooldownTimer = 15f;
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
            if (dist <= 0.5 && cooldownTimer <= 0)
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

    //Separates from other fish2
    protected Vector3 Separate()
    {
        Vector3 steeringForce = Vector3.zero;

        for (int i = 0; i < manager.fish2GOList.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, manager.fish2GOList[i].transform.position);

            if (Mathf.Epsilon < dist)
            {
                steeringForce += Flee(manager.fish2GOList[i].transform.position) * (separateRange / (dist));
            }
        }

        return steeringForce;
    }
}
