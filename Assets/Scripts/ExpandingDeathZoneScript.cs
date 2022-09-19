using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingDeathZoneScript : MonoBehaviour
{
    public float expandingSpeed;
    public bool Collides
    {
        get { return collides; }
    }
    private bool collides = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float increase = expandingSpeed * Time.deltaTime;
        Vector3 scale = GetComponent<Transform>().localScale;
        scale.x += increase;
        scale.z += increase;
        GetComponent<Transform>().localScale = scale;
    }

    private void OnTriggerEnter(Collider other)
    {
        collides = true;
    }
}
