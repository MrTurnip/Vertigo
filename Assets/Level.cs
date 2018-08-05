using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Level : MonoBehaviour {

    public float lowestPoint;
    private GameObject playerObject;
    public UnityEvent OnFallOff = new UnityEvent();
    
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
        playerObject = GameObject.FindGameObjectWithTag("Player");
        OnFallOff.AddListener(delegate () { Debug.Log("Dead"); });
    }
}
