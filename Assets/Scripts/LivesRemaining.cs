using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// This class is subscribed to playerControl's OnDeath event. 
// Every time the player dies a heart disappears.
// When your hearts all vanish the game ends for real and you are taken back to the main menu.

// NOTE: because the main menu won't exist until the game is completed, it will simply reload the earliest scene (0). 
public class LivesRemaining : MonoBehaviour
{
    public bool isGameOver = false;

    private System.Action onGameOver = delegate { };

    bool isBannerDisplayed = false;

    // The scene index that is loaded when you die.
    public const int targetScene = 0;

    // How many lives you have remaining.
    public int amount = 3;

    // An array of hearts. 
    // These objects are sequently turned off as your lives vanish.
    public GameObject[] lifeObjects;

    public GameObject banner;

    // When this object is called a heart vanishes. 
    // When all lives are lost the game resets to final screen. 
    public void LoseLife()
    {
        if (amount == 0)
            return;

        // Deducts one of your lives.
        amount--;

        // This procedurally gets and assigns the Image from the last life in the lifeObjects array.
        GameObject lifeObject = lifeObjects[amount];
        Image lifeImage = lifeObject.GetComponent<Image>();
        // The lifeImage is then disabled so that it appears lost.
        lifeImage.enabled = false;

        if (amount == 0)
        {
            isGameOver = true;
            onGameOver += DisplayBanner;
        }
    }

    private void DisplayBanner()
    {
        Image bannerImage = banner.GetComponent<Image>();
        bannerImage.enabled = true;

        onGameOver = delegate { };
    }

    void LoadFirstScene()
    {
        SceneManager.LoadScene(targetScene);
    }

    public void Update()
    {
        onGameOver();
    }
}
