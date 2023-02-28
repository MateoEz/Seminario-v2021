using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AudioMaster
{
    private static AudioMaster _instance = new AudioMaster();
    private static readonly string path = "Audio/";
    private AudioService _audioService;

    public static AudioMaster Instance
    {
        get
        {
            return _instance;
        }
    }

    public void SetAudioService(AudioService audioService)
    {
        _audioService = audioService;
    }

    private AudioSource _audioSource;

    public void PlayClip(string clipName)
    {
        var clip = Resources.Load<AudioClip>(path + clipName);
        _audioSource = _audioService.CreateSource();
        _audioSource.clip = clip;
        _audioSource.Play();
        DestroySourceAfterClipFinish(_audioSource).Subscribe();
    }
    
    public void PlayClip(string clipName, float volume)
    {
        var clip = Resources.Load<AudioClip>(path + clipName);
        _audioSource = _audioService.CreateSource();
        _audioSource.volume = volume;
        _audioSource.clip = clip;
        _audioSource.Play();
        DestroySourceAfterClipFinish(_audioSource).Subscribe();
    }
    
    public void PlayClip(AudioClip audioClip)
    {
        var source = _audioService.CreateSource();
        source.clip = audioClip;
        source.Play();
        DestroySourceAfterClipFinish(source).Subscribe();
    }

    public bool IsPlaying()
    {
        if (_audioSource)
        {
            return _audioSource.isPlaying;
        }

        return false;
    }

    private IObservable<Unit> DestroySourceAfterClipFinish(AudioSource source)
    {
        return Observable.Timer(TimeSpan.FromSeconds(source.clip.length + 0.5f))
            .DoOnCompleted(() => _audioService.DestroySource(source))
            .AsUnitObservable();
    }
}