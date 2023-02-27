using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioCAMPEONMUNDIAL : MonoBehaviour
{

    [SerializeField] AudioSource audioSong;
    [SerializeField] AudioSource audioWorldChampion;
    [SerializeField] GameObject particle1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) || other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            audioSong.gameObject.SetActive(false);
            audioWorldChampion.gameObject.SetActive(true);
            particle1.SetActive(true);
            if (AchievementsManager.Instance != null)
            {
                AchievementsManager.Instance.TrackAchievement("world_cup");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) || other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            audioSong.gameObject.SetActive(true);
            audioWorldChampion.gameObject.SetActive(false);
            particle1.SetActive(false);
        }
    }
}
