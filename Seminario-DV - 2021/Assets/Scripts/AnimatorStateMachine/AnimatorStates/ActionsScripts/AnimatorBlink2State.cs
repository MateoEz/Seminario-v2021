using System.Linq;
using Domain.Services;
using Player;
using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "NewBlink2State", menuName = "AnimatorStates/Blink2State")]
    public class AnimatorBlink2State : AnimatorStateData
    {
        private PlayerView _view;

        [Header("BlinkValues")]
        [SerializeField] private LayerMask _targetsLayer;
        [SerializeField] private float _blinkSpeed;

        private ITarget _initialTarget;
        private float _lastFrameTime = 0;
        private Transform _transform;

        private float _movingDistance;
        private float _sumOfMovingFractions;
        private IEnergyView _energy;

        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<AnimatorBlink2State>();
            instance._targetsLayer = _targetsLayer;
            instance._blinkSpeed = _blinkSpeed;
            return instance;
        }

        public override void StartState(Animator animator, AnimatorStateInfo stateInfo)
        {
            _view = GetPlayerView(animator);
            _energy = _view.GetEnergyService();
            
            animator.SetBool("HasToAttack", false);
            PlayerState.Instance.IsAttacking = true;
            _initialTarget = _view.CurrentTarget;
            if (_initialTarget == null) return;

            if (!_energy.IsAffordable(_view.BlinkEnergy))
            {
                animator.SetTrigger("SkipBlink");
                return;
            }
            PlayerState.Instance.IsBlinking = true;
            _view.ShowBlinkingFeedback();
            _energy.UseEnergy(_view.BlinkEnergy);

            _view.Rigidbody.mass = 10000;
            _sumOfMovingFractions = 0;

            _movingDistance = 0f;
            _transform = _view.transform;
            LookAtTarget();
        }

        private void LookAtTarget()
        {
            var targetPos = _initialTarget.GetTransform().position;
            var targetPositionIgnoringY = new Vector3(targetPos.x, 0, targetPos.z);
            var playerPos = _view.transform.position;
            var playerPositionIgnoringY = new Vector3(playerPos.x, 0, playerPos.z);
            var dir = (targetPositionIgnoringY - playerPositionIgnoringY).normalized;
            _transform.forward = dir;
        }

        public override void UpdateState(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_initialTarget == null || !_initialTarget.GetTransform().gameObject.activeSelf)
            {
                animator.SetTrigger("SkipBlink");
                return;
            }
            
            LookAtTarget();

            var movingMagnitude = _blinkSpeed * Time.fixedDeltaTime;
            var distantoToTarget = GetDistanceToTarget();
            var maxPossibleMovingVectorMagnitude = distantoToTarget - GetCollidersOffsets();
            if (movingMagnitude > maxPossibleMovingVectorMagnitude)
            {
                movingMagnitude = maxPossibleMovingVectorMagnitude;
                animator.SetTrigger("SkipBlink");//Este es sin return porque en este caso queremos que se mueva un ultimo frame.
            }

            if (_view.IsStucked())
            {
                Debug.LogError("Is Stucked");
                animator.SetTrigger("SkipBlink");
                return;
            }

            MoveForward(movingMagnitude);
        }

        private float GetCollidersOffsets()
        {
            var playerHalfColliderExtenent = (_view.Collider.size * 0.5f).magnitude;
            var targetHalfColliderExtenent = _initialTarget.GetColliderHalfExtent();
            return playerHalfColliderExtenent + targetHalfColliderExtenent;
        }

        private float GetDistanceToTarget()
        {
            var targetPos = _initialTarget.GetTransform().position;
            var targetPositionIgnoringY = new Vector3(targetPos.x, 0, targetPos.z);
            var playerPos = _view.transform.position;
            var playerPositionIgnoringY = new Vector3(playerPos.x, 0, playerPos.z);
            var distantoToTarget = Vector3.Distance(targetPositionIgnoringY, playerPositionIgnoringY);
            return distantoToTarget;
        }

        public override void FinishState(Animator animator, AnimatorStateInfo stateInfo)
        {
            PlayerState.Instance.IsBlinking = false;
            _initialTarget = null;
            _view.Rigidbody.mass = 5;
            _view.HideBlinkingFeedback();
        }

        private void MoveForward(float movingMagnitude)
        {
            _view.Rigidbody.MovePosition(_transform.position + _transform.forward * movingMagnitude);
        }

        private PlayerView GetPlayerView(Animator anim)
        {
            return anim.GetComponent<PlayerView>();
        }
    }
}