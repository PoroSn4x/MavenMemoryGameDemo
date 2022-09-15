using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObjectFollowingScript : MonoBehaviour
{
    public GameObject following;
    public float y;
    public Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float y = GetComponent<Transform>().position.y;
        Vector3 followPos = following.GetComponent<Transform>().position;
        followPos.y = y;
        GetComponent<Transform>().position = followPos + offset;
    }
}
