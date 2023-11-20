using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    //fish1 gameobject list
    public List<GameObject> fish1GOList;
    //Reference to fish1 agents
    public List<fish1> fish1List;

    [SerializeField]
    private GameObject fishType1;

    //Number of each type of fish to spawn
    [SerializeField]
    float numberOfFish = 10f;

    //getter for list for fish1, fish2

    // Start is called before the first frame update
    void Start()
    {
        //instantiate agents
        //add to agent list
        for (int i = 0; i < numberOfFish; i++) 
        {
            fish1GOList.Add(Instantiate(fishType1, new Vector2(Random.Range(-8, 8), Random.Range(-4, 4)), Quaternion.identity));
        }

        //add script reference to script list using getcomponent
        for (int i = 0; i < fish1GOList.Count; i++)
        {
            fish1List.Add(fish1GOList[i].GetComponent<fish1>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
