using System;
using System.Collections.Generic;
using AI.Core.StateMachine;
using DefaultNamespace;
using MyUtilities;
using Player;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "New Attack State", menuName = "AnimatorStates/AttackState")]
    public class AnimatorAttackState : AnimatorStateData, IObservable
    {
        public bool isLastComboState;

        [Header("Combo Data")]
        [Range(0,1)] public float startComboTime;
        [Range(0,1)] public float endComboTime;

        private bool _continueCombo;

        [Header("Collision Detection")]
        [Range(0,1)] public float activeCollisionTime;
        [Range(0,1)] public float disableCollisionTime;

        [Header("Attack Data")] 
        [SerializeField] private int _damage;
        [Range(0,1)] public float attackFinishCharge;

        private bool _hasActivateCollider;
        private bool _hasDisableCollider;
        private bool _hasChargeAttack;
        private ITarget _initialTarget;

        private IEntityView _entity;
        private List<IObserver> _observers = new List<IObserver>();

        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<AnimatorAttackState>();
            instance.startComboTime = startComboTime;
            instance.endComboTime = endComboTime;
            instance.activeCollisionTime = activeCollisionTime;
            instance.disableCollisionTime = disableCollisionTime;
            instance.attackFinishCharge = attackFinishCharge;
            instance._damage = _damage;
            instance._observers = new List<IObserver>();
            return instance;
        }

        public override void StartState(Animator animator, AnimatorStateInfo stateInfo)
        {
            _entity = GetEntity(animator);
            animator.SetBool("ContinueCombo", false);
            _hasActivateCollider = false;
            _hasDisableCollider = false;
            _continueCombo = false;
            if(!PlayerState.Instance.IsDashing)
                _entity.CurrentWeapon.Damage = _damage;
            _initialTarget = _entity.CurrentTarget;
            if (_entity is PlayerView)
            {
                float random = Random.Range(0.1f, 0.2f);
                AudioMaster.Instance.PlayClip("SwordSwoosh",random);
                
                float randomHit = Random.Range(0.1f, 0.2f);
                AudioMaster.Instance.PlayClip("Golpe1",randomHit);
            }
        }

        public override void UpdateState(Animator animator, AnimatorStateInfo stateInfo)
        {
            CheckContinueCombo(animator, stateInfo);

            ActiveCollider(stateInfo);
            DisableCollider(stateInfo);

            if (stateInfo.normalizedTime >= attackFinishCharge)
            {
                _hasChargeAttack = true;
            }
            else
            {
                if (_entity.CurrentTarget != null)
                {
                    _entity.Transform.forward = Utils.GetDirIgnoringHeight(_entity.Transform.position,
                        _entity.CurrentTarget.GetTransform().position);
                }
            }
        }

        public override void FinishState(Animator animator, AnimatorStateInfo stateInfo)
        {
            Notify();
            _hasActivateCollider = false;
            _hasChargeAttack = false;
            
            //Esto se hae asi porque cuando se dashea en medio del ataque se pierde la referencia. Hay que separar logica entre ataque enemigo y ataque player.
            if(!(_entity is PlayerView))
                _entity.CurrentWeapon.DisableCollision();
            else
            {
                if(!PlayerState.Instance.IsDashing)
                    _entity.CurrentWeapon.DisableCollision();
            }
            
            if (isLastComboState || !_continueCombo)
            {
                PlayerState.Instance.IsAttacking = false;
                if ((_entity is PlayerView))
                {
                    _entity.Transform.GetComponent<PlayerView>().ResetSpeed();
                }
                return;
            }

            if (_initialTarget == null) return;
            if (_initialTarget != _entity.CurrentTarget && _entity.CurrentTarget != null)
            {
                animator.SetBool("HasToAttack", true);
            }
        }

        private IEntityView GetEntity(Animator animator)
        {
            return animator.GetComponent<IEntityView>();
        }

        private void CheckContinueCombo(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (startComboTime <= stateInfo.normalizedTime && stateInfo.normalizedTime <= endComboTime)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    _continueCombo = true;
                    animator.SetInteger("Action", 1);
                    animator.SetBool("ContinueCombo", _continueCombo);
                }
            }
        }

        private void ActiveCollider(AnimatorStateInfo stateInfo)
        {
            if (activeCollisionTime <= stateInfo.normalizedTime && !_hasActivateCollider)
            {
                _hasActivateCollider = true;
                //Esto se hae asi porque cuando se dashea en medio del ataque se pierde la referencia. Hay que separar logica entre ataque enemigo y ataque player.
                if(!(_entity is PlayerView))
                    _entity.CurrentWeapon.EnableCollision();
                else
                {
                    if(!PlayerState.Instance.IsDashing)
                        _entity.CurrentWeapon.EnableCollision();
                }
            }
        }

        private void DisableCollider(AnimatorStateInfo stateInfo)
        {
            if (disableCollisionTime <= stateInfo.normalizedTime && !_hasDisableCollider)
            {
                _hasDisableCollider = true;
                //Esto se hae asi porque cuando se dashea en medio del ataque se pierde la referencia. Hay que separar logica entre ataque enemigo y ataque player.
                if(!(_entity is PlayerView))
                    _entity.CurrentWeapon.DisableCollision();
                else
                {
                    if(!PlayerState.Instance.IsDashing)
                        _entity.CurrentWeapon.DisableCollision();
                }
            }
        }

        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer)) 
                _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.OnNotify();
            }
        }

        public bool IsAllreadyCharged()
        {
            return _hasChargeAttack;
        }
    }
}
