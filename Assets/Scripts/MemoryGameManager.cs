using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MemoryGameManager : MonoBehaviour
{
    public Object blinkingAnimation, expandingDeathZone;
    public Material baseMat, correctMat, wrongMat;
    public float delay, blinkDuration, speedIncrease;

    private enum MemoryState { Showing, Playing, Waiting };
    private MemoryState curState = MemoryState.Waiting;

    private float timer = 2f;

    private Segment[] segmentOrder;
    private int segmentTracker;

    private IList<Segment> triggers;

    // Start is called before the first frame update
    void Start()
    {
        triggers = new List<Segment>();
        StartMemoryGame();
        //curState = MemoryState.Playing;
    }

    public void StartMemoryGame()
    {
        segmentOrder = GenerateOrder(5);
        curState = MemoryState.Showing;
        segmentTracker = 0;
    }

    private Segment[] GenerateOrder(int length)
    {
        Segment[] order = new Segment[length];
        int prev = -1;
        for(int i = 0; i < length; i++)
        {
            int next = Random.Range(0, 3);
            // No two consecutive segments allowed
            if(next == prev)
            {
                int coinflip = Random.Range(1, 3);
                next = (next + coinflip) % 3;
            }
            order[i] = (Segment)next;
            prev = next;
        }
        return order;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case MemoryState.Showing:
                UpdateShowing();
                break;
            case MemoryState.Playing:
                UpdatePlaying();
                break;
            case MemoryState.Waiting:
            default:
                break;
        };
       

    }

    private void UpdateShowing()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if(segmentTracker != segmentOrder.Length)
            {
                // Show next blink
                Blink((Segment)segmentOrder[segmentTracker++], blinkDuration, baseMat);
                timer = delay + blinkDuration;
            }
            else
            {
                // Show and tell is over, move on to the next state
                curState = MemoryState.Playing;
                segmentTracker = 0;
                // Clear all triggers except the last one
                Segment currentSegment = triggers[triggers.Count - 1];
                triggers.Clear();
                triggers.Add(currentSegment);

                Instantiate(expandingDeathZone, GetComponent<Transform>());
            }
        }
    }

    private void UpdatePlaying()
    {
        if(gameObject.GetComponentInChildren<ExpandingDeathZoneScript>().Collides)
        {
            Debug.Log("Game Over!");
            //Destroy(GameObject.Find("Exile"));
            Reset();
        }

        //Debug.Log("Updating Playing");
        foreach(Segment segment in triggers)
        {
            if(segment == segmentOrder[segmentTracker])
            {
                if (++segmentTracker >= segmentOrder.Length)
                {
                    Blink(Segment.TopLeft, blinkDuration * 2, correctMat);
                    Blink(Segment.TopRight, blinkDuration * 2, correctMat);
                    Blink(Segment.Bottom, blinkDuration * 2, correctMat);
                    Reset();
                    break;
                }
                else
                {
                    Blink(segment, blinkDuration, correctMat);
                }
            }
            else
            {
                Blink(segment, blinkDuration, wrongMat);
                // Apply penalty for wrong memory
                gameObject.GetComponentInChildren<ExpandingDeathZoneScript>().expandingSpeed += speedIncrease;
            }
        }
        
        triggers.Clear();
    }

    private void Reset()
    {
        Destroy(GameObject.FindGameObjectWithTag("ExpandingDeathZone"));
        curState = MemoryState.Waiting;
        triggers.Clear();
        segmentOrder = null;
        segmentTracker = 0;

    }

    public void SegmentTriggered(Segment segment)
    {
        triggers.Add(segment);
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
