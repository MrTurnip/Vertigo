using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour, IReset {

    public bool isUnlocked;
    private System.Action UnlockingTechnique;
    public GameObject lockObject;
    public Collider collider;

    private void RevealLock()
    {
        lockObject.SetActive(true);
    }

    private void ActivateCollider()
    {
        collider.enabled = true;
    }

    public void ResetToStarting()
    {
        RevealLock();
        ActivateCollider();
        SubscribeTechniques();

        isUnlocked = false;
    }

    private void HideLock()
    {
        lockObject.SetActive(false);
    }

    private void DeactivateCollider()
    {
        collider.enabled = false;
    }

    private void ClearTechniques()
    {
        UnlockingTechnique = delegate { };
    }

    private void SubscribeTechniques()
    {
        UnlockingTechnique += HideLock;
        UnlockingTechnique += DeactivateCollider;
        UnlockingTechnique += ClearTechniques;
    }

    public void Start()
    {
        SubscribeTechniques();
    }

    public void Unlock()
    {
        UnlockingTechnique();

        isUnlocked = true;
    }
}
