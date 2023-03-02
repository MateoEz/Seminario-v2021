using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class MainSongFade : MonoBehaviour
{
    [SerializeField] private float fadeInSeconds;
    [SerializeField] private float fadeOutSeconds;
    [SerializeField] private float fightPitch;
    private AudioSource _mainSongAudioSource;
    private float _initialVolume;

    private float _squadsCounter = 0;
    public bool InCreepyZone { get; set; }
    public bool BossFighting { get; set; }
    private void Start()
    {
        _mainSongAudioSource = GetComponent<AudioSource>();
        _initialVolume = _mainSongAudioSource.volume;
        
        FadeAudio(false);
    }

    public void FadeAudio(bool fadeIn, float seconds = 0,Action onCompleted = null)
    {
        if (fadeIn)
        {
            Tween(_mainSongAudioSource.volume, 0, seconds != 0 ? seconds : fadeInSeconds).DoOnCompleted(()=> onCompleted?.Invoke()).Subscribe(x =>
            {
                _mainSongAudioSource.volume = x;
            });
        }
        else
        {
            Tween(0, _initialVolume, seconds != 0 ? seconds : fadeOutSeconds).DoOnCompleted(()=> onCompleted?.Invoke()).Subscribe(x =>
            {
                _mainSongAudioSource.volume = x;
            });
        }
    }

    private bool _flag = false;
    public void SetFightStatus(bool status)
    {
        if (InCreepyZone) return;
        if (status)
        {
            _squadsCounter++;
            if (_flag) return;
            
            _flag = true;
            FadeAudio(true, 1, () =>
            {
                Tween(_mainSongAudioSource.pitch, fightPitch, 1).DoOnCompleted(()=>
                {
                    FadeAudio(false,1);
                }).Subscribe(x =>
                {
                    _mainSongAudioSource.pitch = x;
                });
            });
        }
        else
        {

            if (BossFighting) return;
            if (_squadsCounter > 1)
            {
                _squadsCounter--;
                return;
            }
            
            if (!_flag) return;
            
            _flag = false;
            _squadsCounter--;
            
            FadeAudio(true, 1, () =>
            {
                Tween(_mainSongAudioSource.pitch, 1, 2).DoOnCompleted(()=>
                {
                    FadeAudio(false,2);
                }).Subscribe(x =>
                {
                    _mainSongAudioSource.pitch = x;
                });
            });
        }
    }
    
    private static IObservable<float> Tween( float from, float to, float seconds ) {
        float delta = to - from;
        Func<float, float> lerpFunc = ( progress ) => { return from + (delta * progress); };
        return UniRx.Observable.FromMicroCoroutine<float>(( observer, cancellationToken ) => TweenEveryCycleCore(observer, from, to, seconds, lerpFunc, cancellationToken), FrameCountType.Update);
    }
    private static IEnumerator TweenEveryCycleCore<T>( IObserver<T> observer, T from, T to, float seconds, Func<float,T> lerpFunc, CancellationToken cancellationToken ) {
        if( cancellationToken.IsCancellationRequested ) yield break;
        if( seconds <= 0 ) {
            observer.OnNext(to);
            observer.OnCompleted();
            yield break;
        }
        float totalTime = 0f;
        observer.OnNext(from);
        while( true ) {
            yield return null;
            if( cancellationToken.IsCancellationRequested ) yield break;

            totalTime += UnityEngine.Time.deltaTime;
            if( totalTime >= seconds ) {
                observer.OnNext(to);
                observer.OnCompleted();
                yield break;
            }
            else {
                float normalizedTime = totalTime / seconds;
                observer.OnNext(lerpFunc(normalizedTime));
            }
        }
    }
}
