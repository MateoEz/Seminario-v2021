using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.Core.StateMachine;
using AI.Enemies.ImplementingStateReader.States;
using AI.Enemies.Spells;
using Domain;
using MyUtilities;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader
{
    public class MiniGrootEnemy : BaseEnemyWithStateReader, ITarget, ISquadMember
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private List<string> statesInfo;
        [SerializeField] private float _speed;
        [SerializeField] private float _distanceBehindTank;
        [SerializeField] private GameConfig _config;
        [SerializeField] private Transform _middleBodyPoint;

        [Header("Spell properties:")]
        [SerializeField] private float _distanceToDetectTarget;
        [SerializeField] private float _spellRange;
        
        [Header("Targeting feedback")]
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material targetingMaterial;
        
        [Header("MiniGrootStunSpell")] 
        [SerializeField] private StunMiniGrootSpellView _stunMiniGrootSpellView;
        [SerializeField] private AreaStunMiniGrootSpellView _areaStunMiniGrootSpellView;
        
        [Header("Die")]
        [SerializeField] private float _timeToDissapearAfterDie;
        [SerializeField] private List<Collider> _collidersToDisableWhenDie;
        [SerializeField] GameObject eye1;
        [SerializeField] GameObject eye2;

        private CastSpell _castSpell;
        private ISpell _stunSpell;
        private SpellCooldownManager _spellsCooldownManager;
        private SquadManager _squad;

        [SerializeField] AudioSource myAudioSource;

        public Transform GetTransform()
        {
            if(this)
                return transform;
            return null;
        }

        public override void Init(EntityState myEntityState, List<MyState> states, MyStateReader stateReader)
        {
            _myEntityState = myEntityState;
            _states = states.OrderByDescending(state => state.GetPrority()).ToList();
            _stateReader = stateReader;
        }
        
        public void Init(SquadManager squad)
        {
            _squad = squad;
        }

        public override void Start()
        {
            //Falta inicializar el stunSpell
            _stunSpell = new StunMiniGrootSpell(_config, Instantiate(_stunMiniGrootSpellView), Instantiate(_areaStunMiniGrootSpellView));
            var spellList = new List<ISpell>();
            spellList.Add(_stunSpell);
            _spellsCooldownManager = new SpellCooldownManager(spellList);
            _castSpell = new CastSpell(_spellsCooldownManager, null);
            
            var entityState = CreateEntityDefaultValues();
            
            var defState = CreateIdleState();
            var tiltTarget = CreateDoAnimationWhileLoockingAtTargetState(new Dictionary<string, object>()
            {
                {"target", PlayerState.Instance.Transform},
                {"squad", false}
            }, 1);
            var tiltTargetWhenDown = CreateDoAnimationWhileLoockingAtTargetState(new Dictionary<string, object>()
            {
                {"target", PlayerState.Instance.Transform},
                {"playerIsParalyzed", true}
            }, 10);
            var runAwayFromTarget = CreateRunAwayFromTargetState();
            var hideBehindTank = CreateHideBehindTankState();
            var castSpell = CreateCastingSpellState();
            var doSpell = CreateInvokeSpellState();
            var deathState = CreateDeathState();
            
            var listOfStates = new List<MyState>();
            listOfStates.Add(defState);
            listOfStates.Add(tiltTarget);
            listOfStates.Add(tiltTargetWhenDown);
            listOfStates.Add(runAwayFromTarget);
            listOfStates.Add(hideBehindTank);
            listOfStates.Add(castSpell);
            listOfStates.Add(doSpell);
            listOfStates.Add(deathState);
            
            Init(entityState, listOfStates, new MyStateReader(defState));
            
        }

       

        public override void Update()
        {
            if (IsDead) return;
            
            base.Update();

            SetWorldState("playerIsParalyzed", PlayerState.Instance.IsStunned || PlayerState.Instance.IsKnocked || PlayerState.Instance.IsRecoveringFromKnock);
            
            if (Physics.OverlapSphere(transform.position, _distanceToDetectTarget, _playerLayer).Any())
            {
                if (_squad)
                {
                    _squad.NotifySquadMembers("target", PlayerState.Instance.Transform);
                }
                else
                {
                    SetWorldState("target", PlayerState.Instance.Transform);
                }
                
                CheckTargetIsInSpellRange();
                CheckSpellIsInCooldown();
            }

            if (_squad)
            {
                if(_squad.GetActiveSquadMembersOfType(EnemyType.Tank).Count > 0)
                    SetWorldState("squad", true);
                else
                    SetWorldState("squad", false);
            }
            else
            {
                SetWorldState("squad", false);
            }
        }

        private void CheckTargetIsInSpellRange()
        {
            SetWorldState("isTargetInSpellRange", Vector3.Distance(PlayerState.Instance.Transform.position, transform.position) <= _spellRange);
        }

        private void CheckSpellIsInCooldown()
        {
            if(_spellsCooldownManager.IsSpellOnCooldown(_stunSpell))
                SetWorldState("isSpellInCooldown", true);
            else
                SetWorldState("isSpellInCooldown", false);
        }
        [SerializeField] AudioClip hitSound;
        public override void GetDamaged(int damaged)
        {
            base.GetDamaged(damaged);
            //AudioMaster.Instance.PlayClip("GrootHit"); 
            myAudioSource.PlayOneShot(hitSound, 1);
        }

        protected override void Die()
        {
            base.Die();
            IsDead = true;
            var stunSpell = _stunSpell as StunMiniGrootSpell;
            ChangeEyes();
            stunSpell.TurnOff();
            transform.forward = Utils.GetDirIgnoringHeight(transform.position, PlayerState.Instance.Transform.position);
            SetWorldState("dead", true);
            _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            foreach (var collider in _collidersToDisableWhenDie)
            {
                collider.enabled = false;
            }
            StartCoroutine(DestroyAfterSegs());
        }

        private IEnumerator DestroyAfterSegs()
        {
            yield return new WaitForSeconds(_timeToDissapearAfterDie);
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            yield return new WaitForSeconds(1);
            Destroy(this.gameObject);
        }



        void ChangeEyes()
        {
            var first = eye1.GetComponent<MeshRenderer>();
            var second  = eye2.GetComponent<MeshRenderer>();
            first.material.SetColor("_EmissionColor", Color.black * 30f);
            second.material.SetColor("_EmissionColor", Color.black * 30f);

        }
        public override void OnAttackStart()
        {
        }

        public override void OnChargedAttack()
        {
        }

        public override void OnAttackEnd()
        {
        }

        public override Vector3 MiddleBodyPoint
        {
            get { return _middleBodyPoint.position; }
        }

        public float GetColliderHalfExtent()
        {
            return 0;
        }

        public void ShowFeedback()
        {
            if (this == null) return;
            var currentMat = transform.GetComponentInChildren<SkinnedMeshRenderer>();
            currentMat.material = targetingMaterial;
        }

        public void HideFeedback()
        {
            if (this == null) return;
            var currentMat = transform.GetComponentInChildren<SkinnedMeshRenderer>();
            currentMat.material = normalMaterial;
        }
        
        private DeathState CreateDeathState()
        {
            var deathPreCon = new EntityState(new Dictionary<string, object>()
                {
                    {"dead", true}
                });
            
            var spellState = new DeathState(
                deathPreCon, 100);
            spellState.Init(_animator);
            return spellState;
        }

        private InvokeSpellState CreateInvokeSpellState()
        {
            var spellStatePreCon =
                new EntityState(new Dictionary<string, object>()
                {
                    {"target", PlayerState.Instance.Transform},
                    {"isTargetInSpellRange", true},
                    {"isSpellInCooldown", true},
                    {"doSpell", true}
                });
            
            var spellState = new InvokeSpellState(
                spellStatePreCon, 5);
            spellState.Init(this, _castSpell, _stunSpell);
            return spellState;
        }

        private CastingSpellState CreateCastingSpellState()
        {
            var castingSpellStatePreCon =
                new EntityState(new Dictionary<string, object>()
                {
                    {"target", PlayerState.Instance.Transform},
                    {"isTargetInSpellRange", true},
                    {"isSpellInCooldown", true}
                });
            
            var castingSpellState = new CastingSpellState(
                castingSpellStatePreCon, 4);

            castingSpellState.Init(_animator, (_animator) =>
            {
                _animator.ResetTrigger("CastSpell");
                _animator.SetTrigger("CastSpell");
            }, this, _stunSpell);
            return castingSpellState;
        }

        private HideBehindTankState CreateHideBehindTankState()
        {
            var hideBehindTankPreCon = new EntityState(new Dictionary<string, object>()
            {
                {"target", PlayerState.Instance.Transform},
                {"squad", true}
            });
            var hideBehindTankState = new HideBehindTankState(hideBehindTankPreCon, 3);
            hideBehindTankState.Init(_squad, this, _rigidbody, _speed, _distanceBehindTank, _animator);
            return hideBehindTankState;
        }

        private RunAwayFromTargetState CreateRunAwayFromTargetState()
        {
            var runAwayFromTargetPreCon = new EntityState(new Dictionary<string, object>()
            {
                {"target", PlayerState.Instance.Transform},
                {"isTargetInSpellRange", true},
                {"squad", false}
            });
            var runAwayFromTargetState = new RunAwayFromTargetState(runAwayFromTargetPreCon, 2);
            runAwayFromTargetState.Init(_rigidbody, _speed, _animator);
            return runAwayFromTargetState;
        }

        private DoAnimationWhileLoockingAtTargetState CreateDoAnimationWhileLoockingAtTargetState(Dictionary<string, object> preConditions, int priority)
        {
            var doAnimationWhileLoockingAtTargetPreCon = new EntityState(preConditions);
            
            var doAnimationWhileLoockingAtTarget = new DoAnimationWhileLoockingAtTargetState(doAnimationWhileLoockingAtTargetPreCon, priority);

            doAnimationWhileLoockingAtTarget.Init(_animator, (_animator) =>
            {
                _animator.ResetTrigger("Tilt");
                _animator.SetTrigger("Tilt");
            }, transform);
            return doAnimationWhileLoockingAtTarget;
        }

        private IdleState CreateIdleState()
        {
            var idlePreConditions = new EntityState(new Dictionary<string, object>());
            var defState = new IdleState(idlePreConditions);
            defState.Init(_animator);
            return defState;
        }
        
        private EntityState CreateEntityDefaultValues()
        {
            var entityValues = new Dictionary<string, object>();
            foreach (var state in statesInfo)
            {
                entityValues.Add(state, null);
            }

            var entityState = new EntityState(entityValues);
            return entityState;
        }
    }
}