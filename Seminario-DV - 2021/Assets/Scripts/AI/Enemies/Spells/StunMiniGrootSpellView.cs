using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AI.Enemies.Spells
{
    public class StunMiniGrootSpellView : MonoBehaviour, ISpellView
    {
        private static readonly int reverseAnimationStateHash = Animator.StringToHash("Base Layer.Reverse");
        [SerializeField] private Animator _animator;
        private float _timeStunned;

        [SerializeField] AudioSource aS;
        [SerializeField] AudioClip soundClip;
        public void Init(float timeStunned)
        {
            _timeStunned = timeStunned;
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Active()
        {
            gameObject.SetActive(true);
            _animator.SetTrigger("Active");
            aS.PlayOneShot(soundClip, .2f);
            DisactiveAfterStunnedTime()
                .Subscribe();
        }

        public void Disactive()
        {
            _animator.SetTrigger("Reverse");
            StartCoroutine(WaitUntilAnimationEndsCorroutine(reverseAnimationStateHash));
        }

        private IObservable<Unit> DisactiveAfterStunnedTime()
        {
            return Observable.Timer(TimeSpan.FromSeconds(_timeStunned))
                .DoOnCompleted(Disactive)
                .AsUnitObservable();
        }

        private IEnumerator WaitUntilAnimationEndsCorroutine(int hashState, int layerIndex = 0)
        {
            while (_animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash != hashState)
            {
                yield return null;
            }

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1)
            {
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}