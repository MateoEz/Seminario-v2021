using AI.Core.StateMachine;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class DeathState : MyState
    {
        private Animator _animator;

        public DeathState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public DeathState(EntityState preConditions, MisionType missionType, int priority = 0) : base(preConditions, missionType, priority)
        {
        }

        public void Init(Animator animator)
        {
            _animator = animator;
        }
        
        public override void Awake()
        {
            _animator.SetTrigger("Die");
        }

        public override void Execute()
        {
            
        }

        public override bool CanChangeState()
        {
            return false;
        }
    }
}