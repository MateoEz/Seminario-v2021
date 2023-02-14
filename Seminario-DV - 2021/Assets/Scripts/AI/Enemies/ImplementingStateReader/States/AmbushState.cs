using AI.Core.StateMachine;
using Domain;
using MyUtilities;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class AmbushState : MyState
    {
        private const float VELOCITY_MULTIPLIER_WHEN_GOING_BACKWARDS = 0.5f;
        private Animator _animator;
        private float _speed;
        private Transform _ownerTransform;
        private Vector3 _targetPosition;
        private Rigidbody _rigidbody;
        private IAmbusher _ambusher;

        public AmbushState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public AmbushState(EntityState preConditions, MisionType missionType, int priority = 0) : base(preConditions, missionType, priority)
        {
        }
        
        public void Init(Animator animator, float speed, Transform ownerTransform, Rigidbody rigidbody, IAmbusher ambusher)
        {
            _animator = animator;
            _speed = speed;
            _ownerTransform = ownerTransform;
            _rigidbody = rigidbody;
            _ambusher = ambusher;
        }

        public override void Awake()
        {
            base.Awake();
            _animator.SetBool("IsMoving", true);
            _targetPosition = _ambusher.GetAmbushPosition();
            // if (!AudioMaster.Instance.IsPlaying())
            // {
            //     AudioMaster.Instance.PlayClip("TEST");
            // }
        }

        public override void Execute()
        {
            
            _animator.SetBool("IsMoving", true);
            _targetPosition = _ambusher.GetAmbushPosition();
            _ownerTransform.forward = Utils.GetDirIgnoringHeight(_ownerTransform.position, PlayerState.Instance.Transform.position);
            if (_targetPosition.IsAtRange(_ownerTransform.position, 0.5f))
            {
                _animator.SetBool("IsMoving", false);
                return;
            }
            _ambusher.SetAmbusherExpectedPosition(_targetPosition);
            var position = _ownerTransform.position;
            var moveVector = Utils.GetVectorIgnoringHeight(position, _targetPosition);
            var moveDir = Utils.GetDirIgnoringHeight(position, _targetPosition);

            var playerAvoidance = Utils.GetVectorIgnoringHeight(PlayerState.Instance.Transform.position, position);
            if (playerAvoidance.magnitude <= 8)//si esta muy cerca del player
            {
                moveDir = (moveDir + playerAvoidance.normalized * 0.6f).normalized; //0.6f es el Avoidance Multiplier
            }
            

            // --- ESTO VA A SERVIR CUANDO QUERRAMOS BLENDEAR DIRECCIONES EN EL ANIMATOR ---
            
            var velocitiyZ = Vector3.Dot(_ownerTransform.forward, moveDir);
            var velocitiyX = Vector3.Dot(_ownerTransform.right, moveDir);
            _animator.SetFloat("VelocityZ", velocitiyZ);
            _animator.SetFloat("VelocityX", velocitiyX);
            
            // --- *** ----
            
            var angleOfMoving = Vector3.Angle(_ownerTransform.forward, moveDir);
            if (angleOfMoving > 90)
            {
                var percentageOfSpeedToSlow = angleOfMoving / 180 * VELOCITY_MULTIPLIER_WHEN_GOING_BACKWARDS;
                moveDir *= (1 - percentageOfSpeedToSlow);
            }

            if (PlayerState.Instance.IsBlinking) return;
            _rigidbody.MovePosition(position + moveDir * _speed * Time.deltaTime);
        }

        public override bool CanChangeState()
        {
            return true;
        }
        
        
    }
}