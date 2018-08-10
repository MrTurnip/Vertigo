using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenFade : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public float transitionTime;
    private float rate;
    private float progress = 0;
    private System.Action Action = delegate { };
    public UnityEvent OnFinishReset = new UnityEvent();
    public Level level;
    private float resetTimer { get { return level.resetTimer; } }
    
    public void StartFadingProcess()
    {
        Action = LerpOpacityToFull;
        Action += delegate
         {
             if (resetTimer <= 0)
             {
                 ResetVeilToBlack();
                 Action = delegate { };
             }
         };
    }

    private void LerpOpacityToFull()
    {
        Color color = spriteRenderer.color;
        color = Color.Lerp(Color.clear, Color.white, progress);
        progress += rate;
        spriteRenderer.color = color;
    }

    private void ResetVeilToBlack()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        
        progress = 0;
    }
    
    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rate = 1.0f / ((float)Application.targetFrameRate * transitionTime);

        level = GameObject.FindObjectOfType<Level>();
    }

    // Update is called once per frame
    void Update()
    {
        Action();
    }


}
