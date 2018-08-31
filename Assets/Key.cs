using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Key : MonoBehaviour, IReset
{
    public Lock master;
    private System.Action CollectionTechnique;
    public Collider collider;
    public GameObject keyObject;

    public bool isCollected;

    private void RevealKey()
    {
        keyObject.SetActive(true);
    }

    private void HideKey()
    {
        keyObject.SetActive(false);
    }
    
    private void ActivateCollider()
    {
        collider.enabled = true;
    }

    private void DeactivateCollider()
    {
        collider.enabled = false;
    }

    private void ClearCollectionTechnique()
    {
        CollectionTechnique = delegate { };
    }

    private void UnlockMaster()
    {
        master.Unlock();
    }

    private void SubscribeCollectionTechniques()
    {
        CollectionTechnique += HideKey;
        CollectionTechnique += DeactivateCollider;
        CollectionTechnique += UnlockMaster;
        CollectionTechnique += ClearCollectionTechnique;
    }
    
    private void Collect()
    {
        CollectionTechnique();

        isCollected = true;
    }

    public void ResetToStarting()
    {
        ActivateCollider();
        RevealKey();
        SubscribeCollectionTechniques();
        
        isCollected = false;
    }
    
    private void Awake()
    {
        SubscribeCollectionTechniques();
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.tag == "Player")
        {
            Collect();
        }
    }
}
