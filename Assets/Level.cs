using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public enum ResetFunction { Launch, Process, Finish }
    public float lowestPoint;
    private GameObject playerObject;
    public UnityEvent OnOutOfBounds = new UnityEvent();

    public void CheckFallOff()
    {
        Vector3 playerPosition = playerObject.transform.position;
        float playerY = playerPosition.y;
        if (playerY <= lowestPoint)
        {
            OnOutOfBounds.Invoke();
        }
    }

    public void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerControl playerControl = playerObject.GetComponent<PlayerControl>();
        ScreenFade screenFade = GameObject.FindObjectOfType<ScreenFade>();

        OnOutOfBounds.AddListener(screenFade.FadeToBlack);
        OnOutOfBounds.AddListener(playerControl.SwitchToResetStart);
    }

    public void Update()
    {
    }
}
