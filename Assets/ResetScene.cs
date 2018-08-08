using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour {

    public ResetPhase phase = ResetPhase.notReseting;
    public System.Action levelReset;
    public System.Action resetProcess = delegate { };
    public float timeUntilLevelReset;

    private void ReloadLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    private void ResetCountdown()
    {
        timeUntilLevelReset -= Time.deltaTime;

        if (timeUntilLevelReset <= 0)
        {
            ReloadLevel();
        }
    }

    private void StartResetProcess()
    {
        phase = ResetPhase.reseting;
    }
    
	// Use this for initialization
	void Start () {
        levelReset += StartResetProcess;
        resetProcess += ResetCountdown;
	}
	
	// Update is called once per frame
	void Update () {
        if (phase == ResetPhase.reseting)
            resetProcess();
	}
}
