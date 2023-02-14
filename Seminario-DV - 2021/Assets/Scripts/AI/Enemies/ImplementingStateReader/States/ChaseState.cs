using AI.Core.StateMachine;
using Player;
using UnityEngine;
using MyUtilities;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class ChaseState : MyState
    {
        private Animator _animator;
        private Transform _target;
        private float _speed;
        private Transform _ownerTransform;
        private Rigidbody _rigidbody;

        public ChaseState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }
        public ChaseState(EntityState preConditions, MisionType misionType, int priority = 0) : base(preConditions, misionType, priority)
        {
        }

        public void Init(Animator animator, float speed, Transform ownerTransform, Rigidbody rigidbody)
        {
            _animator = animator;
            _speed = speed;
            _ownerTransform = ownerTransform;
            _target = PlayerState.Instance.Transform;
            _rigidbody = rigidbody;
        }

        public override void Awake()
        {
            _animator.SetBool("IsMoving", true);
            _animator.SetFloat("VelocityZ", 1f);
            _animator.SetFloat("VelocityX", 0f);
        }

        public override void Execute()
        {
            var position = _ownerTransform.position;
            _ownerTransform.forward = Utils.GetDirIgnoringHeight(position, _target.position);
            _rigidbody.MovePosition(position + _ownerTransform.forward * _speed * Time.deltaTime);
        }

        public override bool CanChangeState()
        {
            return true;
        }
    }
}