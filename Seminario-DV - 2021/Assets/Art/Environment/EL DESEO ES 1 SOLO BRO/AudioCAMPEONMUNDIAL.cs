using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCAMPEONMUNDIAL : MonoBehaviour
{

    [SerializeField] AudioSource audioSong;
    [SerializeField] AudioSource audioWorldChampion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) || other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            audioSong.gameObject.SetActive(false);
            audioWorldChampion.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) || other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            audioSong.gameObject.SetActive(true);
        audioWorldChampion.gameObject.SetActive(false);

        }
    }
}
