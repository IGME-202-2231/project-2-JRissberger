using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    //fish1 gameobject list
    public List<GameObject> fish1GOList;
    //Reference to fish1 agents
    public List<fish1> fish1List;

    //fish2 gameobject list
    public List<GameObject> fish2GOList;
    //fish2 agent scripts
    public List<fish2> fish2List;

    [SerializeField]
    private GameObject fishType1;

    [SerializeField]
    private GameObject fishType2;

    //Number of each type of fish to spawn
    [SerializeField]
    float numberOfFish1 = 7f;
    [SerializeField]
    float numberOfFish2 = 3f;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate agents
        //add to agent list
        for (int i = 0; i < numberOfFish1; i++) 
        {
            fish1GOList.Add(Instantiate(fishType1, new Vector2(Random.Range(-8, 8), Random.Range(-4, 4)), Quaternion.identity));
        }
        for (int i = 0; i < numberOfFish2; i++)
        {
            fish2GOList.Add(Instantiate(fishType2, new Vector2(Random.Range(-8, 8), Random.Range(-4, 4)), Quaternion.identity));
        }

        //add script reference to script list using getcomponent
        for (int i = 0; i < fish1GOList.Count; i++)
        {
            fish1List.Add(fish1GOList[i].GetComponent<fish1>());
        }
        for (int i = 0; i < fish2GOList.Count;i++)
        {
            fish2List.Add(fish2GOList[i].GetComponent<fish2>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        //TODO: detecting click method
    }

    //method here for user click
    //checks for click each frame
    //if click detected, get mouse position (check to ensure it's something that translates properly to coordinates)
    //if I can't get position right, could do random from the top
    //instantiate a piece of food
    //add food to gameobject list
}   

