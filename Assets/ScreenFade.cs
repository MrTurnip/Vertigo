using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenFade : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public float transitionTime;
    private float rate;
    private float progress;
    private System.Action Action = delegate { };
    public UnityEvent OnFinishReset = new UnityEvent();

    public void FadeToBlack() { }
    
    private void ResetFadeProcess()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        progress = 0;
    }

    private void LerpOpacityToFull()
    {
        Color color = spriteRenderer.color;
        color = Color.Lerp(Color.clear, Color.white, progress);
        progress += rate;
        spriteRenderer.color = color;

        if (progress >= 1.0f)
        {
            OnFinishReset.Invoke();
            ResetFadeProcess();
            Action = delegate { };
        }
    }

 
    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rate = 1.0f / ((float)Application.targetFrameRate * transitionTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        Action();
        
    }

   
}
