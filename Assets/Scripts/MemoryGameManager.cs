using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MemoryGameManager : MonoBehaviour
{
    public Object blinkingAnimation;
    public Material baseMat, correctMat, wrongMat;
    public float delay, blinkDuration;

    private enum MemoryState { Showing, Playing, Waiting };
    private enum Segment { TopLeft, TopRight, Bottom };
    private MemoryState curState = MemoryState.Waiting;

    private float timer = 0;
    private int temp = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Blink(Segment.TopRight, 2, baseMat);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            // Show next blink
            Blink((Segment)temp++, blinkDuration, baseMat);
            timer = delay + blinkDuration;
        }
    }

    private void Blink(Segment segment, float duration, Material mat)
    {
        Object anim = Instantiate(blinkingAnimation, GetComponent<Transform>());

        Vector3 offset;
        Vector3 eulers = Vector3.zero;
        switch (segment)
        {
            case Segment.TopLeft:
                offset = new Vector3(-0.1f, 0, 0);
                break;
            case Segment.TopRight:
                offset = new Vector3(0.1f, 0, 0);
                eulers = new Vector3(0, 0, 240);
                break;
            case Segment.Bottom:
            default:
                offset = new Vector3(0, -0.2f, 0);
                eulers = new Vector3(0, 0, 120);
                break;
        }
        anim.GetComponent<Transform>().Translate(offset);
        anim.GetComponent<Transform>().Rotate(eulers);
        anim.GetComponent<MeshRenderer>().material = mat;
        anim.GetComponent<SegmentBlinkingAnimation>().duration = duration;
    }
}
