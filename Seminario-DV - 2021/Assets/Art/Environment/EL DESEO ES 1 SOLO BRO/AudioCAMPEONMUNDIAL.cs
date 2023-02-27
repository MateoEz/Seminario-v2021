using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCAMPEONMUNDIAL : MonoBehaviour
{

    [SerializeField] AudioSource audioSong;
    [SerializeField] AudioSource audioWorldChampion;
    [SerializeField] GameObject particle1;

    private void Start()
    {
        Debug.Log("arranque");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) || other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            Debug.Log("entre wacho");
            audioSong.gameObject.SetActive(false);
            audioWorldChampion.gameObject.SetActive(true);
            particle1.SetActive(true);
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
