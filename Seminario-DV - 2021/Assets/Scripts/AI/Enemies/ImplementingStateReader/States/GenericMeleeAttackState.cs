using System;
using System.Collections.Generic;
using System.Linq;
using AI.Core.StateMachine;
using AnimatorStateMachine.AnimatorStates.ActionsScripts;
using DefaultNamespace;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class GenericMeleeAttackState : MyState, IObserver
    {
        private Animator _animator;
        private bool _canChangeState;
        private BaseEnemyWithStateReader _owner;
        private Action<float> _refreshCooldown;
        private List<AnimatorAttackState> _attackAnimatorStates = new List<AnimatorAttackState>();

        public GenericMeleeAttackState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }
        public GenericMeleeAttackState(EntityState preConditions, MisionType missionType, int priority = 0) : base(preConditions, missionType, priority)
        {
        }

        public void Init(Animator animator, BaseEnemyWithStateReader owner)
        {
            _owner = owner;
            _animator = animator;
            var animStatesMachines = _animator.GetBehaviours<AnimatorStateMachine.AnimatorStateMachine>();
            foreach (var sm in animStatesMachines)
            {
                var actions = sm.GetListOfActions().Where(state => state.GetType() == typeof(AnimatorAttackState)).ToList();
                foreach (AnimatorAttackState action in actions)
                {
                    action.RegisterObserver(this);
                    _attackAnimatorStates.Add(action);
                }
            }
        }

        public override void Awake()
        {
            _owner.OnAttackStart();
            _canChangeState = false;         
            _animator.SetTrigger("MeleeAttack");
            _owner.transform.forward = (PlayerState.Instance.Transform.position - _owner.transform.position).normalized;
        }

        public override void Execute()
        {
            if (!IsHitStoppingTheAttack())
                _owner.OnChargedAttack();
        }

        public override void Sleep()
        {
            base.Sleep();
            _canChangeState = false;
        }

        public override bool CanChangeState()
        {           
            return _canChangeState;
        }

        public void OnNotify()
        {
            _owner.OnAttackEnd();
            _canChangeState = true;
            _owner.SetWorldState("inAttackRange", false);
            _owner.SetWorldState("attackCooldown", false);
        }

        public override void OnForceQuit()
        {
            base.OnForceQuit();
            _canChangeState = true;
            _animator.SetTrigger("FinishMeleeAttack");
            _owner.OnAttackEnd();
        }

        public bool IsHitStoppingTheAttack()
        {
            if (_owner.GetComponent<BossBehaviour>()) return false;
            foreach (var attack in _attackAnimatorStates)
            {
                if (attack.IsAllreadyCharged())
                    return false;
            }
            return true;
        }
    }
}