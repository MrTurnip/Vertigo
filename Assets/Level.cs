using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum ResetPhase { notReseting, reseting, finished}

public class Level : MonoBehaviour {

    public class Reset
    {
        public Level level;
        ResetPhase phase;

        public void StartProcess()
        {
            phase = ResetPhase.reseting;
        }

        public void Update()
        {

        }

        public Reset(Level level)
        {

        }
    }

    public float lowestPoint;
    private GameObject playerObject;
    public UnityEvent OnFallOff = new UnityEvent();
    public UnityEvent OnUpdate = new UnityEvent();
    private Reset reset;

    public void CheckFallOff()
    {
        Transform playerTransform = playerObject.transform;
        Vector3 playerPosition = playerTransform.position;
        float playerY = playerPosition.y;

        if (playerY < lowestPoint)
        {
            OnFallOff.Invoke();
        }
    }

    public void Start()
    {
        reset = new Reset(this);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        OnFallOff.AddListener(reset.StartProcess);
        OnUpdate.AddListener(reset.Update);
    }

    public void Update()
    {
        OnUpdate.Invoke();
    }
}
