using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    public enum Phase { None, ActiveControl, ResetStart, ResetProcessing, ResetExecute }
    public Phase activePhase = Phase.None;

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
    public System.Action Action = delegate { };
    public UnityEvent OnFall = new UnityEvent();
    private Level level;
    private LivesRemaining livesRemaining;

    public bool hasWonLevel = false;
    public UnityEvent OnDeath = new UnityEvent();
    public const int MAX_LIVES = 3;
    public const string LIVESREMAINING = "LivesRemaining";
    public enum SpeedSetting { full, half, stopped }
    public SpeedSetting speedSetting;
    public bool canMove = true;
    public const float fullSpeed = 2.5f;
    public const float halfSpeed = 1.0f;
    public const float stopped = 0.0f;
    public Vector3 startingPosition;
    public float resetTimer { get { return level.resetTimer; }}

    public void EnterWinPhase()
    {
        canMove = false;
        rigidbody.useGravity = false;

        hasWonLevel = true;

        Action = delegate 
        {
            rigidbody.AddForce(Vector3.up);
        };
    }

    
    public void SwitchToActiveAtStart()
    {
        if (this.activePhase == Phase.ActiveControl || this.hasWonLevel == true)
            return;

        this.activePhase = Phase.ActiveControl;

        this.transform.position = startingPosition;
        this.rigidbody.velocity = Vector3.zero;

        if (livesRemaining.isGameOver)
            canMove = false;

        Action = delegate { };
        Action += GetInput;
        Action += RollRigidbody;
        Action += CheckIfFall;
    }

    public void SwitchToResetStart()
    {
        if (this.activePhase == Phase.ResetStart)
            return;

        this.activePhase = Phase.ResetStart;

        Action = delegate { OnDeath.Invoke(); };
        Action += SwitchToResetProcess;
        
    }

    public void SwitchToResetProcess()
    {
        if (this.activePhase == Phase.ResetProcessing)
            return;

        this.activePhase = Phase.ResetProcessing;

        Action = delegate { };
        Action += delegate {if (resetTimer <= 0) SwitchToResetExecute();  };
        
    }

    public void SwitchToResetExecute()
    {
        if (this.activePhase == Phase.ResetExecute)
            return;

        this.activePhase = Phase.ResetExecute;

        Action = delegate { };
        Action += SwitchToActiveAtStart;
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

    private void CheckIfFall()
    {
        Vector3 velocity = rigidbody.velocity;
        float yVelocity = velocity.y;
        if (yVelocity <= -1.0f)
        {
            OnFall.Invoke();
        }
    }


    public void Awake()
    {
        startingPosition = this.transform.position;
    }

    public void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        livesRemaining = GameObject.FindObjectOfType<LivesRemaining>();
        OnDeath.AddListener(livesRemaining.LoseLife);

        SwitchToActiveAtStart();

        level = GameObject.FindObjectOfType<Level>();
        OnFall.AddListener(level.CheckFallOff);
    }

    public void Update()
    {
        Action();
    }


}
