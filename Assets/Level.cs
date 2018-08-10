using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public bool inState = false;
    public float lowestPoint;
    private GameObject playerObject;
    public UnityEvent OnOutOfBounds = new UnityEvent();
    public const float maxResetTime = 1.0f;
    public float resetTimer = maxResetTime;
    public System.Action Action = delegate { };
    public bool isResetting = false;

    private void SwitchResetStateToLaunch()
    {
        if (inState)
            return;
        inState = true;

        Action = delegate { };
        Action += SwitchResetStateToProcess;
        Action += delegate { inState = false; };
    }
    private void SwitchResetStateToProcess()
    {
        if (inState)
            return;
        inState = true;

        Action = delegate { };
        Action += delegate
        {
            resetTimer -= Time.deltaTime;
            
            if (resetTimer <= 0)
            {
                inState = false;
                SwitchResetStateToFinalize();
            }
        };

    }

    private void SwitchResetStateToFinalize()
    {
        if (inState)
            return;
        inState = true;

        Action = delegate { };
        Action += delegate { resetTimer = maxResetTime; Action = delegate { }; inState = false; isResetting = false; };
    }

    public void CheckFallOff()
    {
        if (isResetting)
            return; 

        Vector3 playerPosition = playerObject.transform.position;
        float playerY = playerPosition.y;
        if (playerY <= lowestPoint)
        {
            OnOutOfBounds.Invoke();

            // Ensures that this method is only ran once. 
            isResetting = true;
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
        CameraRotation cameraRotation = GameObject.FindObjectOfType<CameraRotation>();

        OnOutOfBounds.AddListener(screenFade.StartFadingProcess);
        OnOutOfBounds.AddListener(playerControl.SwitchToResetStart);
        OnOutOfBounds.AddListener(this.SwitchResetStateToLaunch);
        OnOutOfBounds.AddListener(cameraRotation.GetResetStart());
    }

    public void Update()
    {
        Action();
    }
}
