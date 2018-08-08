using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour, IReset
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
    private bool lifeDeducted = false;

    public const int MAX_LIVES = 3;
    public const string LIVESREMAINING = "LivesRemaining";
    public int livesRemaining = MAX_LIVES;
    public enum SpeedSetting { full, half, stopped }
    public SpeedSetting speedSetting;
    public bool canMove = true;
    public const float fullSpeed = 2.5f;
    public const float halfSpeed = 1.0f;
    public const float stopped = 0.0f;

    public void LaunchResetProcess()
    {
        Debug.Log("Player reset.");
    }

    public void RegisterToCollection()
    {
        Level level = GameObject.FindObjectOfType<Level>();
        level.resetObjects.Add(this);
    }

    public void LoseLife()
    {
        if (lifeDeducted)
            return;

        livesRemaining--;
        lifeDeducted = true;

        if (livesRemaining == 0)
        {
            Debug.Log("Out of lives. Restoring.");
            livesRemaining = MAX_LIVES;
        }

       // PlayerPrefs.SetInt(LIVESREMAINING, livesRemaining);
    }

    private void SubscribeToUnityEvent(UnityEvent targetEvent, UnityAction listener)
    {
        targetEvent.AddListener(listener);
    }

    private void SubscribeToActivePhase(UnityAction listener)
    {
        SubscribeToUnityEvent(activePhase, listener);
    }

    private void SubscribeToOnFall(UnityAction listener)
    {
        SubscribeToUnityEvent(onFall, listener);
    }

    public void SubscribeFallOffCheck()
    {
        Level level = GameObject.FindObjectOfType<Level>();
        SubscribeToOnFall(level.CheckFallOff);
       
    }

    public void SubscribePhysicsObservation()
    {
        onUpdate.AddListener(ObserveVelocity);
    }

    private void ObserveVelocity()
    {
        Vector3 velocity = rigidbody.velocity;
        float yVelocity = velocity.y;
        if (yVelocity < -1.0f)
        {
            onFall.Invoke();
        }
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

    public void Awake()
    {
        if (PlayerPrefs.GetInt(LIVESREMAINING, -1) == -1)
        {
            PlayerPrefs.SetInt(LIVESREMAINING, MAX_LIVES);
        }

        livesRemaining = PlayerPrefs.GetInt(LIVESREMAINING);
    }

    public void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();

        EnableMovement();

        SubscribeFallOffCheck();

        SubscribePhysicsObservation();

        RegisterToCollection();
    }

    public void Update()
    {
        activePhase.Invoke();

        onUpdate.Invoke();
    }

   
}
