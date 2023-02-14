using System;
using System.Collections.Generic;
using AI.Core.StateMachine;
using MyUtilities;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class DoAnimationWhileLoockingAtTargetState : MyState
    {
        private Animator _animator;
        private Action<Animator> _animations;
        private Transform _ownerTransform;

        public DoAnimationWhileLoockingAtTargetState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public void Init(Animator animator, Action<Animator> animations, Transform ownerTransform)
        {
            _animator = animator;
            _animations = animations;
            _ownerTransform = ownerTransform;
        }

        public override void Awake()
        {
            _animator.ResetTrigger("Idle");
            _animations(_animator);
        }

        public override void Execute()
        {
            _ownerTransform.forward =
                Utils.GetDirIgnoringHeight(_ownerTransform.position, PlayerState.Instance.Transform.position);
        }

        public override bool CanChangeState()
        {
            return true;
        }
    }
}