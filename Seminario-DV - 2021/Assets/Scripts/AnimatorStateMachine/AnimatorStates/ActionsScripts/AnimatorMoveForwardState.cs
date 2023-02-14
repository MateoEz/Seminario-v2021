using System;
using DefaultNamespace;
using Player;
using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "NewMoveForwardState", menuName = "AnimatorStates/MoveForwardState")]
    public class AnimatorMoveForwardState : AnimatorStateData
    {
        [Range(0, 1)] public float startMovingTime;
        [Range(0, 1)] public float stopMovingTime;
        public float movingDistance;

        private IEntityView _entity;
        private Vector3 _initialPosition;
        private float _movingFraction;
        private Transform _transform;
        private float _lastFrameTime;
        private float _timeNormalizedMoving;

        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<AnimatorMoveForwardState>();
            instance.startMovingTime = startMovingTime;
            instance.stopMovingTime = stopMovingTime;
            instance.movingDistance = movingDistance;
            return instance;
        }

        public override void StartState(Animator animator, AnimatorStateInfo stateInfo)
        {
            _lastFrameTime = startMovingTime;
            _timeNormalizedMoving = stopMovingTime - startMovingTime;
            _entity = GetEntity(animator);
            _transform = _entity.Transform;
            
            
            if (_entity.CurrentTarget != null && _entity.CurrentTarget.GetTransform())
            {
                var targetPos = _entity.CurrentTarget.GetTransform().position;
                var targetPositionIgnoringY = new Vector3(targetPos.x, 0, targetPos.z);
                var playerPos = _transform.position;
                var playerPositinIgnoringY = new Vector3(playerPos.x, 0, playerPos.z);
                var dir = (targetPositionIgnoringY - playerPositinIgnoringY).normalized;
                _transform.forward = dir;
            }
            
        }

        public override void UpdateState(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (startMovingTime <= stateInfo.normalizedTime && stateInfo.normalizedTime <= stopMovingTime)
            {
                var movingFranction = (stateInfo.normalizedTime - _lastFrameTime) / _timeNormalizedMoving;
                _entity.Rigidbody.MovePosition(_transform.position + _transform.forward * movingFranction * movingDistance);
                _lastFrameTime = stateInfo.normalizedTime;
            }

            if (stateInfo.normalizedTime >= stopMovingTime && _lastFrameTime <= stopMovingTime)
            {
                var movingFranction = (stopMovingTime - _lastFrameTime) / _timeNormalizedMoving;
                _entity.Rigidbody.MovePosition(_transform.position + _transform.forward * movingFranction * movingDistance);
                _lastFrameTime = stateInfo.normalizedTime;//Con esto más la condicion del if nos aseguramos que se reproduzca esto una sola vez.
            }
        }

        private IEntityView GetEntity(Animator anim)
        {
            return anim.GetComponent<IEntityView>();
        }
    }
}