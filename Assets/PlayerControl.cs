using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    private UnityEvent activePhase = new UnityEvent();
    private UnityEvent onUpdate = new UnityEvent();
    private UnityEvent onFall = new UnityEvent();
    private Vector3 direction = new Vector2();
    private Rigidbody rigidbody;
    private Vector3 momentum { get { return direction * Speed; } }
    private float Speed
    {
        get
        {
            switch (speedSetting)
            {
                case SpeedSetting.full: return fullSpeed;
                case SpeedSetting.half: return halfSpeed;
                default: return stopped;
            }
        }
    }

    public enum SpeedSetting { full, half, stopped }
    public SpeedSetting speedSetting;
    public bool canMove = true;
    public const float fullSpeed = 2.5f;
    public const float halfSpeed = 1.0f;
    public const float stopped = 0.0f;

    private void SubscribeToUnityEvent(UnityEvent targetEvent, UnityAction listener)
    {
        targetEvent.AddListener(listener);
    }

    private void SubscribeToActivePhase(UnityAction listener)
    {
        SubscribeToUnityEvent(activePhase, listener);
    }

    private void SubscribeToOnUpdate(UnityAction listener)
    {
        SubscribeToUnityEvent(onUpdate, listener);
    }

    public void SubscribeFallOffCheck()
    {
        Level level = GameObject.FindObjectOfType<Level>();
        SubscribeToOnUpdate(level.CheckFallOff);
    }

    private void GetInput()
    {
        // If the player can't move then there's no point in getting any input.
        if (!canMove)
            return; 

        // Gets vertical and horizontal input as 0-1 values then assigns them to direction.
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        this.direction = new Vector3(xAxis, 0, yAxis);

        // If shift is down then speedSetting is set to half, slowing the player; otherwise speed is at full.
        bool shiftIsdown = Input.GetKey(KeyCode.LeftShift);
        speedSetting = shiftIsdown == true ? SpeedSetting.half : SpeedSetting.full;
    }

    private void RollRigidbody()
    {
        this.rigidbody.angularVelocity = momentum;
    }

    public void EnableMovement()
    {
        SubscribeToActivePhase(GetInput);
        SubscribeToActivePhase(RollRigidbody);
    }

    public void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();

        EnableMovement();

        SubscribeFallOffCheck();
    }

    public void Update()
    {
        activePhase.Invoke();

        onUpdate.Invoke();
    }
}
