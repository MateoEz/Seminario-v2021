using System.Runtime.InteropServices;
using Player;
using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "NewBlinkState", menuName = "AnimatorStates/BlinkState")]
    public class AnimatorBlinkState : AnimatorStateData
    {
        private PlayerView _view;
        
        [Header("BlinkValues")]
        //[SerializeField] private float _maxMovingDistance;
        [SerializeField] private float _minFinalDistanceToTarget;

        private ITarget _initialTarget;
        private float _lastFrameTime = 0;
        private Transform _transform;

        private float _movingDistance;
        private float _sumOfMovingFractions;

        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<AnimatorBlinkState>();
            instance._minFinalDistanceToTarget = _minFinalDistanceToTarget;
            return instance;
        }

        public override void StartState(Animator animator, AnimatorStateInfo stateInfo)
        {
            PlayerState.Instance.IsBlinking = true;
            PlayerState.Instance.IsAttacking = true;
            
            _view = GetPlayerView(animator);
            _sumOfMovingFractions = 0;
            animator.SetBool("HasToAttack", false);
            _initialTarget = _view.CurrentTarget;
            if (_initialTarget == null) return;

            _movingDistance = 0f;
            _transform = _view.transform;
            var targetPos = _initialTarget.GetTransform().position;
            var targetPositionIgnoringY = new Vector3(targetPos.x, 0, targetPos.z);
            var playerPos = _view.transform.position;
            var playerPositionIgnoringY = new Vector3(playerPos.x, 0, playerPos.z);
            var dir = (targetPositionIgnoringY - playerPositionIgnoringY).normalized;
            _transform.forward = dir;
            _movingDistance = Vector3.Distance(targetPositionIgnoringY, playerPositionIgnoringY) - _minFinalDistanceToTarget;
        }

        public override void UpdateState(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_initialTarget == null) return;
            var movingFranction = stateInfo.normalizedTime - _lastFrameTime;
            MoveForward(movingFranction);
            _lastFrameTime = stateInfo.normalizedTime;
        }

        public override void FinishState(Animator animator, AnimatorStateInfo stateInfo)
        {
            PlayerState.Instance.IsBlinking = false;
            if (_initialTarget != null)
            {
                var movingFranction = 1 - _lastFrameTime;
                MoveForward(movingFranction);
            }

            Debug.LogWarning("Distance moved in blink: " + _sumOfMovingFractions);
            _initialTarget = null;
            _lastFrameTime = 0;
        }

        private void MoveForward(float movingFranction)
        {
            _sumOfMovingFractions += movingFranction;
            _view.Rigidbody.MovePosition(_transform.position + _transform.forward * movingFranction * _movingDistance);
        }

        private PlayerView GetPlayerView(Animator anim)
        {
            return anim.GetComponent<PlayerView>();
        }
    }
}