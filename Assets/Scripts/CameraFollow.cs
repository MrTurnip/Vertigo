using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float overheadDistance = 10;
    public float buffer = 0.3f;
    public Transform followTarget;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 followTargetPosition = followTarget.position;

        Vector3 currentPosition = this.transform.position;
        Vector3 nextPosition = Vector3.SmoothDamp(currentPosition, followTargetPosition, ref velocity, buffer);
        nextPosition.y = overheadDistance = 10;
        this.transform.position = nextPosition;
	}
}
