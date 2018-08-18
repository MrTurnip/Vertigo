using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Verified to run correctly.

// This class spins the camera around every few seconds to create the dizzying effect of the camera spinning. 
// RotationsPerSecond controls how many times a second the camera will rotate.
// Can be activated or deactivatedd with isActive.
// When the camera is reset it does not affect the isActive boolean.
public class CameraRotation : MonoBehaviour
{
    // Enables camera spinning.
    public bool isActive = true;

    // How many times per second the camera makes a complete rotation.
    public float rotationsPerSecond;

    // Scaled from 0 to 1 to regulate the total rate of rotation.
    public float power = 1.0f;

    // The amount of degrees in a circle.
    const int radius = 360;

    // How many ticks-per-second the camera rotates. 
    float rate { get { return radius * rotationsPerSecond; } }

    // Will eventually contain the reset parameters.
    System.Action AlternateMode = delegate { };

    // Is used during reset to return the camera to its starting position.
    Vector3 startingPosition;

    // Likewise returns the camera to its starting rotation.
    Quaternion startingRotation;

    // A reference to level.cs, which contains the resetTimer.
    Level level;

    public void SlowToHalt()
    {
        AlternateMode = delegate
        {
            if (power > 0)
                power -= 1f * Time.deltaTime;
            else
                power = 0;
        };
    }

    // Called once per frame, it rotates the camera's Z-axis to make the scene spin.
    void RotateZ()
    {
        // How many degrees in a frame the camera rotates.
        float deltaRate = rate * Time.deltaTime * power;

        // The vector containing deltaRate in its Z-axis.
        Vector3 rotation = new Vector3(0, 0, deltaRate);

        // Rotates the camera, thus spinning the scene. 
        this.transform.Rotate(rotation);
    }

    // Resets the Camera's starting rotation and locatio (in case it moved).
    public void ResetToOrigin()
    {
        // Resets position.
        this.transform.position = startingPosition;

        // Resets rotation.
        this.transform.rotation = startingRotation;
    }

    // Returns the ResetStart method. Level.cs will execute this method when you get game-over.
    public UnityAction GetResetStart()
    {
        return ResetStart;
    }

    // Even though this method only exists to be move on from, it follows the pattern the rest of the game follows during reset.
    void ResetStart()
    {
        // Reassigns the second part of the method,
        AlternateMode = ResetProcess;
    }

    // The second part of the reset, Process waits until Level.cs's resetTimer hits 0 to move onto the next part.
    void ResetProcess()
    {
        // Once level's resetTimer reaches 0 this will switch to the final part.
        if(level.resetTimer <= 0)
        {
            // Reassigns the Action delegate to ResetFinalize, the final part.
            AlternateMode = ResetFinalize;
        }
    }

    // The final part of the reset process. Resets everything in the camera. 
    void ResetFinalize()
    {
        // Resets the camera's position and rotation.
        ResetToOrigin();

        // Resets the Action method.
        AlternateMode = delegate { };
    }
    
    // Use this for initialization
    void Start()
    {
        // Finds and assigns the level script to our level reference.
        // We need this reference for access to its timer. 
        level = GameObject.FindObjectOfType<Level>();

        // Stores starting location for later use.
        startingPosition = this.transform.position;

        // Likewise stores starting rotation.
        startingRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Used during reset.
        if (isActive)
        {
            // Constantly rotates the camera.
            RotateZ();
            
            AlternateMode();
        }
    }
}
