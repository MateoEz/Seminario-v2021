using System;
using System.Linq;
using AI.Core.StateMachine;
using MyUtilities;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class HideBehindTankState : MyState
    {
        private SquadManager _squad;
        private BaseEnemyWithStateReader _owner;
        private BaseEnemyWithStateReader _nearest;
        private Rigidbody _ownerRigidbody;
        private float _speed;
        private float _distanceBehindTank;
        private Animator _animator;
        private float _offsetToAchivePosition = 0.4f;
        private bool _hided;
        private float _offsetToMoveToPosition = 0.7f;

        public HideBehindTankState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public HideBehindTankState(EntityState preConditions, MisionType missionType, int priority = 0) : base(preConditions, missionType, priority)
        {
        }

        public void Init(SquadManager squad, BaseEnemyWithStateReader owner, Rigidbody ownerRigidbody, float speed, float distanceBehindTank, Animator animator)
        {
            _hided = false;
            _squad = squad;
            _owner = owner;
            _ownerRigidbody = ownerRigidbody;
            _speed = speed;
            _distanceBehindTank = distanceBehindTank;
            _animator = animator;
        }

        public override void Awake()
        {
            base.Awake();
            
            SetNearestTankFromSquad();
            if (_nearest == null)
                _owner.SetWorldState("squad", false);
            else
                _animator.SetBool("IsMoving", true);
            
        }

        public override void Execute()
        {
            if (_nearest == null) return;
            var wantedPosition = GetBehindTankPosition();
            LookAtAndMoveTo(wantedPosition);
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

        private void SetNearestTankFromSquad()
        {
            _nearest = null;
            var squadMembers = _squad.GetActiveSquadMembers();
            if (squadMembers.Count > 0)
            {
                var tanksByDistance = squadMembers.Where(member => member.GetType().Equals(EnemyType.Tank))
                    .OrderBy(member =>
                        Utils.GetVectorIgnoringHeight(_owner.transform.position, member.transform.position).magnitude);
                if (tanksByDistance.Any())
                    _nearest = tanksByDistance.First();
                else
                    _nearest = null;
            }
            else
                _nearest = null;
        }

        private Vector3 GetBehindTankPosition()
        {
            if(_nearest == null) throw new Exception("No deberia llegar hasta aca sin un nearest");
            var nearestTransform = _nearest.transform;
            return (-1) * _distanceBehindTank * nearestTransform.forward + nearestTransform.position;
        }

        private void LookAtAndMoveTo(Vector3 wantedPosition)
        {
            var transform = _owner.transform;
            var currentPosition = transform.position;
            if (Vector3.Distance(wantedPosition, currentPosition) < _offsetToAchivePosition && !_hided)
            {
                _hided = true;
                LookAtPlayer(transform, currentPosition);
                StopMovementAnimationAndStartTilt();
                return;
            }

            if (Vector3.Distance(wantedPosition, currentPosition) < _offsetToMoveToPosition && _hided)
            {
                LookAtPlayer(transform, currentPosition);
                return;
            }
            
            if (Vector3.Distance(wantedPosition, currentPosition) > _offsetToMoveToPosition && _hided)
            {
                _hided = false;
                _animator.SetBool("IsMoving", true);
                _animator.ResetTrigger("Tilt");
            }
            transform.forward = Utils.GetDirIgnoringHeight(currentPosition, wantedPosition);
            _ownerRigidbody.MovePosition(currentPosition + _speed * Time.deltaTime * transform.forward);
        }

        private void StopMovementAnimationAndStartTilt()
        {
            _animator.SetBool("IsMoving", false);
            _animator.ResetTrigger("Tilt");
            _animator.SetTrigger("Tilt");
        }

        private static void LookAtPlayer(Transform transform, Vector3 currentPosition)
        {
            transform.forward = Utils.GetDirIgnoringHeight(currentPosition, PlayerState.Instance.Transform.position);
        }
    }
}