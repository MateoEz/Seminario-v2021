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
    private AudioSource _mainSongAudioSource;
    private float _initialVolume;

    private void Start()
    {
        _mainSongAudioSource = GetComponent<AudioSource>();
        _initialVolume = _mainSongAudioSource.volume;
        
        FadeAudio(false);
    }

    public void FadeAudio(bool fadeIn)
    {
        if (fadeIn)
        {
            Tween(_mainSongAudioSource.volume, 0, fadeInSeconds).Subscribe(x =>
            {
                _mainSongAudioSource.volume = x;
            });
        }
        else
        {
            Tween(0, _initialVolume, fadeOutSeconds).Subscribe(x =>
            {
                _mainSongAudioSource.volume = x;
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
