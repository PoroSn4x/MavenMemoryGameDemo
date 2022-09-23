using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Segment { TopLeft, TopRight, Bottom, Nothing };

public class SegmentHitboxScript : MonoBehaviour
{
    public Segment segment;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Exile")
        {
            gameObject.GetComponentInParent<MemoryGameManager>().SegmentTriggered(segment);
        }
    }
}
