using System;
using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private AudioSource _baseAudioSource;

    private AudioSource _audioSource;

    private void Awake()
    {
        AudioMaster.Instance.SetAudioService(this);
    }

    public AudioSource CreateSource()
    {
        return Instantiate(_baseAudioSource);
    }

    public void DestroySource(AudioSource source)
    {
        if (source)
        {
            Destroy(source.gameObject);
        }
    }
}