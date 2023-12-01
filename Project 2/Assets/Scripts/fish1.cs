using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish1 : agent
{
    //tracks current state
    //updates and behavior changes accordingly
    private string state = "wander";

    //weights
    float wanderWeight = 10f;
    float boundsWeight = 8f;
    float cohesionWeight = 5f;
    float separateWeight = 1f;
    float obstacleWeight = 5f;
    

    Vector3 totalForce = Vector3.zero;


    Vector3 flockDirection = Vector3.zero;

    //"timer" for checking for state change
    float timer = 1f;

    void Start()
    {
        //finds the manager
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
            totalForce += Cohesion() * cohesionWeight;
            totalForce += AvoidObstacles(avoidTime) * obstacleWeight;
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

        transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
        // Zero out acceleration
        acceleration = Vector3.zero;

        totalForce = Vector3.zero;

    }

    //separates from other fish1
    protected Vector3 Separate()
    {
        Vector3 steeringForce = Vector3.zero;

        for (int i = 0; i < manager.fish1GOList.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, manager.fish1GOList[i].transform.position);

            if (Mathf.Epsilon < dist)
            {
                steeringForce += Flee(manager.fish1GOList[i].transform.position) * (separateRange / (dist));
            }
        }
        return steeringForce;
    }

    //method for eating food
    //loop through food list, check if distance to one is zero
    //if so, change fish state, reset hunger timer, destroy object from list. check abt specifics for making it null. should
    //be as easy as just checking the spot on the list.
}
