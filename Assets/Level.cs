using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum ResetPhase { notReseting, reseting, finished}

public interface IReset
{
    void LaunchResetProcess();
    void RegisterToCollection();
}

public class Level : MonoBehaviour, IReset {

    public float lowestPoint;
    private GameObject playerObject;
    public UnityEvent OnFallOff = new UnityEvent();
    public UnityEvent OnUpdate = new UnityEvent();
    ResetPhase resetPhase = ResetPhase.notReseting;
    public List<IReset> resetObjects = new List<IReset>();

    public void LaunchResetProcess()
    {
        Debug.Log("Level reset.");
    }

    public void RegisterToCollection()
    {
        resetObjects.Add(this);
    }

    private void LaunchAllResets()
    {
        foreach (IReset resetObject in resetObjects)
        {
            resetObject.LaunchResetProcess();
        }
    }

    public void CheckFallOff()
    {
        if (resetPhase != ResetPhase.notReseting)
            return; 

        Transform playerTransform = playerObject.transform;
        Vector3 playerPosition = playerTransform.position;
        float playerY = playerPosition.y;

        if (playerY < lowestPoint)
        {
            resetPhase = ResetPhase.reseting;

            LaunchAllResets();

            PlayerControl playerControl = GameObject.FindObjectOfType<PlayerControl>();
            playerControl.LoseLife();
        }
    }

    // Notes.
    // Remove Reset class and its members; centralize the Reset process to the ResetScene script. 
    public void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        //OnFallOff.AddListener(reset.StartProcess);
        //OnUpdate.AddListener(reset.Update);

        RegisterToCollection();
    }

    public void Update()
    {
        OnUpdate.Invoke();
    }
}
