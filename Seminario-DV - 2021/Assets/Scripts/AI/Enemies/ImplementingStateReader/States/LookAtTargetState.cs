using AI.Core.StateMachine;
using MyUtilities;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class LookAtTargetState : MyState
    {
        private Animator _animator;
        private Transform _ownerTransform;
        
        public LookAtTargetState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public void Init(Animator animator)
        {
            _animator = animator;
            _ownerTransform = _animator.transform;
        }

        public override void Awake()
        {
            _animator.SetTrigger("Idle");
            _animator.SetBool("IsMoving", false);
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