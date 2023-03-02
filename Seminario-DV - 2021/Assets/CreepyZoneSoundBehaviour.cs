using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CreepyZoneSoundBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private float creepyPitch;
    [SerializeField] private float seconds;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView)|| other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            FindObjectOfType<MainSongFade>().InCreepyZone = true;
            if (mainAudioSource.pitch < 1f) return;
            Tween(mainAudioSource.pitch, creepyPitch, seconds).Subscribe(x =>
            {
                mainAudioSource.pitch = x;
            });
        }
    }
    private void OnTriggerExit(Collider other)
    { ;
        if (other.TryGetComponent(out PlayerView playerView)|| other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            FindObjectOfType<MainSongFade>().InCreepyZone = false;
            if (mainAudioSource.pitch > creepyPitch) return;
            Tween(mainAudioSource.pitch, 1, seconds).Subscribe(x =>
            {
                mainAudioSource.pitch = x;
            });
        }
    }
    
    public static IObservable<float> Tween( float from, float to, float seconds ) {
        float delta = to - from;
        Func<float, float> lerpFunc = ( progress ) => { return from + (delta * progress); };
        return UniRx.Observable.FromMicroCoroutine<float>(( observer, cancellationToken ) => TweenEveryCycleCore(observer, from, to, seconds, lerpFunc, cancellationToken), FrameCountType.Update);
    }
    static IEnumerator TweenEveryCycleCore<T>( IObserver<T> observer, T from, T to, float seconds, Func<float,T> lerpFunc, CancellationToken cancellationToken ) {
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
