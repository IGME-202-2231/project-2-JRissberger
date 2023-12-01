using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    //radius of obstacle, used for avoidance purposes
    [SerializeField]
    public float radius = 1f;

    public Vector3 position = Vector3.zero;

    void Start()
    {
        position = transform.position;
    }
}
