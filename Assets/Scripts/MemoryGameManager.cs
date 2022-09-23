using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryGameManager : MonoBehaviour
{
    public Object blinkingAnimation, expandingDeathZone;
    public Material baseMat, correctMat, wrongMat;
    public float delay, blinkDuration, speedIncrease, delayBetweenGames;
    public int length;

    private enum MemoryState { Showing, Playing, Waiting, GameOver };
    private MemoryState curState = MemoryState.Waiting;

    private float timer;

    private Segment[] segmentOrder;
    private int segmentTracker;
    private Segment prevSegment = Segment.Nothing;

    private IList<Segment> triggers;

    private int wins = 0;
    private int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        timer = delayBetweenGames;
        triggers = new List<Segment>();

        AdjustDifficulty();


        StartMemoryGame(length);
        //curState = MemoryState.Playing;
    }

    private void AdjustDifficulty()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        // Based on difficulty, change variables here
        switch (difficulty)
        {

        }
    }

    public void StartMemoryGame(int l)
    {
        segmentOrder = GenerateOrder(l);
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
            case MemoryState.GameOver:
                UpdateGameOver();
                break;
            case MemoryState.Waiting:
            default:
                break;
        };
    }

    private void UpdateGameOver()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SceneManager.LoadScene(0);
        }

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
            Blink(Segment.TopLeft, blinkDuration, wrongMat);
            Blink(Segment.TopRight, blinkDuration, wrongMat);
            Blink(Segment.Bottom, blinkDuration, wrongMat);
            Destroy(GameObject.Find("Exile"));
            Reset();
            curState = MemoryState.GameOver;
            timer = 1.5f;
        }

        //Debug.Log("Updating Playing");
        foreach(Segment segment in triggers)
        {
            if(segment == segmentOrder[segmentTracker])
            {
                if (++segmentTracker >= segmentOrder.Length)
                {
                    Blink(Segment.TopLeft, blinkDuration, correctMat);
                    Blink(Segment.TopRight, blinkDuration, correctMat);
                    Blink(Segment.Bottom, blinkDuration, correctMat);
                    Reset();
                    // Start a new memory game which is one longer
                    wins++;
                    StartMemoryGame(length + wins);
                    break;
                }
                else
                {
                    Blink(segment, blinkDuration, correctMat);
                    prevSegment = segment;
                }
            }
            else if (segment != prevSegment)
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
        timer = delayBetweenGames;
        prevSegment = Segment.Nothing;
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
