using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SegmentBlinkingScript : MonoBehaviour
{
    public enum BlinkState { Base, Correct, Wrong };
    public Material baseMaterial, correctMaterial, wrongMaterial;

    private bool blinking;
    private float remaining;

    // Start is called before the first frame update
    void Start()
    {
        blinking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (blinking)
        {
            remaining -= Time.deltaTime;
            if(remaining <= 0)
            {
                Disable();
                blinking = false;
            }
        }
    }

    // Duration in seconds
    public void Blink(int duration, BlinkState color)
    {
        Debug.Log("Blinking");
        if(!blinking)
        {
            blinking = true;
            remaining = duration;
            GetComponent<MeshRenderer>().material = ColorToMat(color);
            Enable();
        }
    }

    private Material ColorToMat(BlinkState color)
    {
        switch(color)
        {
            case BlinkState.Correct:
                return correctMaterial;
            case BlinkState.Wrong:
                return wrongMaterial;
            default:
                return baseMaterial;
        }
    }

    private void Disable()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void Enable()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called");
        if (other.gameObject.name == "Exile")
        {
            Blink(1, BlinkState.Base);
        }
    }
  
}
