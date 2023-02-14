using AI.Core.StateMachine;
using MyUtilities;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class RunAwayFromTargetState : MyState
    {
        private Rigidbody _ownerRigidBody;
        private float _speed;
        private Animator _animator;

        public RunAwayFromTargetState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public RunAwayFromTargetState(EntityState preConditions, MisionType missionType, int priority = 0) : base(
            preConditions, missionType, priority)
        {
        }

        public void Init(Rigidbody ownerRigidBody, float speed, Animator animator)
        {
            _ownerRigidBody = ownerRigidBody;
            _speed = speed;
            _animator = animator;
        }

        public override void Awake()
        {
            base.Awake();
            _animator.SetBool("IsMoving", true);
        }

        public override void Execute()
        {
            var ownerPosition = _ownerRigidBody.transform.position;
            var moveDir = Utils.GetDirIgnoringHeight(PlayerState.Instance.Transform.position, ownerPosition);
            _ownerRigidBody.transform.forward = moveDir;
            _ownerRigidBody.MovePosition(ownerPosition + _speed * Time.deltaTime * moveDir);
        }

        public override void Sleep()
        {
            base.Sleep();
            _animator.SetBool("IsMoving", false);
        }

        public override bool CanChangeState()
        {
            return true;
        }
    }
}