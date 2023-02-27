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


    private bool _canBeGrabbed = false;
    private void Update()
    {
        if (!_canBeGrabbed) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnTouched();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) ||
            other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            _canBeGrabbed = true;
            
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
            _canBeGrabbed = false;
        }
    }

    private void OnTouched()
    {
        _canBeGrabbed = false;
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
