using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Color = UnityEngine.Color;

public class SegmentBlinkingAnimation : MonoBehaviour
{
    public float duration = 1.0f;
    public float fadeDuration = 0.2f;
    public float maxAlpha = .5f;
    private float remaining;
    private float fadeSpeed;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        remaining = duration;
        fadeSpeed = 1f / fadeDuration * maxAlpha;
        mat = GetComponent<MeshRenderer>().material;
        Color c = mat.color;
        c.a = 0;
        mat.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        remaining -= Time.deltaTime;
        if (remaining <= 0)
        {
            Object.Destroy(this.gameObject);
        }
        if(remaining >= duration - fadeDuration)
        {
            // We're fading in
            Color c = mat.color;
            c.a += fadeSpeed * Time.deltaTime;
            mat.color = c;
        }
        else if (remaining <= fadeDuration)
        {
            // We're fading out
            Color c = mat.color;
            c.a -= fadeSpeed * Time.deltaTime;
            mat.color = c;
        }
    }
}
