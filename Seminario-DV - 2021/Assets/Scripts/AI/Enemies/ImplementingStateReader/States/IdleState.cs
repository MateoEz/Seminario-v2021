using AI.Core.StateMachine;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class IdleState : MyState
    {
        private Animator _animator;
        
        public IdleState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public void Init(Animator animator)
        {
            _animator = animator;
        }

        public override void Awake()
        {
            _animator.ResetTrigger("Idle");
            _animator.SetTrigger("Idle");
            _animator.SetBool("IsMoving", false);
        }

        public override void Execute()
        {
        }

        public override bool CanChangeState()
        {
            return true;
        }
    }
}