using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour, IReset
{
    ResetPhase resetPhase = ResetPhase.notReseting;
    private System.Action resetProcess = delegate { };
    SpriteRenderer spriteRenderer;

    public float transitionTime;
    private float rate;
    private float progress;

    public void LaunchResetProcess()
    {
        Debug.Log("ScreenFade reset.");
    }

    public void RegisterToCollection()
    {
        Level level = GameObject.FindObjectOfType<Level>();
        level.resetObjects.Add(this);
    }

    private void LerpOpacityToFull()
    {
        Color color = spriteRenderer.color;
        color = Color.Lerp(Color.clear, Color.white, progress);
        progress += rate;
        spriteRenderer.color = color;

        if (progress >= 1.0f)
        {
            resetPhase = ResetPhase.finished;
        }
    }

    private void StartResetProcess()
    {
        resetPhase = ResetPhase.reseting;

    }

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        //ResetScene resetScene = GameObject.FindObjectOfType<ResetScene>();
        //resetScene.levelReset += StartResetProcess;
        //spriteRenderer = this.GetComponent<SpriteRenderer>();

        RegisterToCollection();

        resetProcess += LerpOpacityToFull;

        rate = 1.0f / ((float)Application.targetFrameRate * transitionTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (resetPhase == ResetPhase.reseting)
        {
            resetProcess();
        }
    }


}
