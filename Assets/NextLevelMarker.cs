using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelMarker : MonoBehaviour
{
    // This is invoked whenever a rigidbody touches the flagpole.
    public event System.Action onWin = delegate { };

    // This is set to true once the marker has been activated.
    public bool isActivated = false;

    // These are references to various components around the level.
    // There are special methods in each that are subscribed to the onWin delegate. 
    public Level level;
    public PlayerControl playerControl;
    public CameraRotation cameraRotation;
    public ParticleSystem particleSystem;

    // Stops the particle system.
    // Is automatically invoked from the start.
    private void StopParticleSystem()
    {
        particleSystem.Stop();
    }

    // Launches the particle system.
    private void PlayParticleSystem()
    {
        particleSystem.Play();
    }

    // Clears the onWin delegate.
    private void ClearOnWin()
    {
        onWin = delegate { };
    }

    // Sets the isActivated bool to true.
    private void Activate()
    {
        isActivated = true;
    }

    // Runs on the first frame of runtime.
    private void Start()
    {
        StopParticleSystem();

        onWin += playerControl.EnterWinPhase;
        onWin += level.EnterWinPhase;
        onWin += cameraRotation.SlowToHalt;
        onWin += Activate;
        onWin += ClearOnWin;
        onWin += PlayParticleSystem;
    }

    // Is ran whenever a rigidbody colliders with it.
    public void OnTriggerEnter(Collider collision)
    {
        // Checks if the Player touched it and not some rouge rigidbody.
        if (collision.tag == "Player")
            onWin(); // launches the exit-level process.
    }
}
