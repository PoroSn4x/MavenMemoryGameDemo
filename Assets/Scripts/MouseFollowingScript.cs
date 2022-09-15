using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowingScript : MonoBehaviour
{
    public float followingSpeed = 1f;
    public float minDistance = .1f;
    public LayerMask layerMask;

    private Vector3 target;
    private float halfSize;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set initial movement
        target = GetComponent<Transform>().position;
        halfSize = GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            // We can keep the z-position of our mouse to 0 since we move in 2d
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitData;

            // Random maximum distance, we do not care about it
            if (Physics.Raycast(ray, out hitData, 100000f, layerMask))
            {
                target = hitData.point;
            }
        }

        Vector3 curPos = GetComponent<Transform>().position;
        Vector3 diff = target - curPos;
        diff.y = 0;
        if(diff.magnitude > minDistance)
            GetComponent<Transform>().Translate(diff.normalized * followingSpeed * Time.deltaTime);
    }
}
