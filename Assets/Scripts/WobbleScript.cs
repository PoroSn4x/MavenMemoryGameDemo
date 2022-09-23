using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleScript : MonoBehaviour
{
    public float wobbleX, wobbleZ, offset, multiplier;

    private Vector3 anchor;
    private float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        anchor = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        float offsetX = Mathf.Sin(t + offset) * wobbleX;
        float offsetZ = Mathf.Sin(t*multiplier + offset) * wobbleZ;
        GetComponent<Transform>().position = anchor + new Vector3(offsetX, 0, offsetZ);
    }
}
