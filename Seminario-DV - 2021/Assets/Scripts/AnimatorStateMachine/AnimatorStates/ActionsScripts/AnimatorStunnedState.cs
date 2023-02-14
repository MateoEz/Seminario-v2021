using System;
using System.Collections.Generic;
using AI.Core.StateMachine;
using DefaultNamespace;
using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "New Stunned State", menuName = "AnimatorStates/StunnedState")]
    public class AnimatorStunnedState : AnimatorStateData, IObservable
    {
        private List<IObserver> _observers = new List<IObserver>();
        
        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<AnimatorStunnedState>();
            instance._observers = new List<IObserver>();
            return instance;
        }

        public override void StartState(Animator animator, AnimatorStateInfo stateInfo)
        {
            base.StartState(animator, stateInfo);
            animator.ResetTrigger("FinishMeleeAttack");
        }

        public override void FinishState(Animator animator, AnimatorStateInfo stateInfo)
        {
            if(stateInfo.normalizedTime >= 1f)
                Notify();
        }

        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer)) 
                _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.OnNotify();
            }
        }
    }
}