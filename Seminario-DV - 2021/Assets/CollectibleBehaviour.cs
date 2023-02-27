using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectToDeactivate;
    [SerializeField] private GameObject particle;

    [SerializeField] private AudioSource myAudioSource;
    [SerializeField] private AudioClip myAudioClip;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) ||
            other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnTouched();
            }

            if (particle)
            {
                if (particle.activeInHierarchy) return;
                particle.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) ||
            other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            particle.SetActive(false);
        }
    }

    private void OnTouched()
    {
        GetComponent<BoxCollider>().enabled = false;
        gameObjectToDeactivate.SetActive(false);
        Destroy(particle);
        
        if (AchievementsManager.Instance != null)
        {
            AchievementsManager.Instance.TrackAchievement("collectible");
        }
        myAudioSource.PlayOneShot(myAudioClip, .3f);
        FindObjectOfType<CollectibleHandler>().OnGrabbed();

    }
}
