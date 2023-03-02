using System.Collections.Generic;
using System.Linq;
using AI.Core.StateMachine;
using AI.Enemies.ImplementingStateReader.States;
using DefaultNamespace;
using Domain;
using MyUtilities;
using Player;
using UnityEngine;
using Weapons;

namespace AI.Enemies.ImplementingStateReader
{
    public class GolemEnemy : BaseEnemyWithStateReader, IEntityView, ITarget, IAmbusher
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _attackRange;
        [SerializeField] private float _distanceToChase;
        [SerializeField] private float _attackCooldown;

        [SerializeField] private List<string> statesInfo;
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private Rigidbody _rigidbody;

        [Header("Feedback")]
        [SerializeField] private GameObject _explodeMesh;

        [SerializeField] private ParticleSystem attackParticle;
        [SerializeField] private GameObject swordParticle;
        [SerializeField] private Material targetingMaterial;
        [SerializeField] private Material targetingSwordMaterial;
        [SerializeField] private Material initialMaterial;
        [SerializeField] private Material swordInitialMaterial;
        [SerializeField] private GameObject sword;
        [SerializeField] private GameObject eyes;

        [Header("Offset collider")]
        [SerializeField] private float _colliderOffset;


        private float _lastAttackTime;
        private SquadManager _squad;
        private Vector3 _lastAmbushPositionAssigned;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _colliderOffset);
            Gizmos.DrawWireSphere(transform.position,_distanceToChase);
        }
        
        public override void Start()
        {
            var entityState = CreateEntityDefaultValues();
            
            var defState = CreateIdleState();
            var lookAtTargetState = CreateLookAtTargetState();
            var chaseState = CreateChaseState();
            var waitingForAttack = CreateWaitingToAttackState();
            var motivateState = CreateMotivateToAttackState();
            var ambushState = CreateAmbushState();
            var meleeAttackState = CreateMeleeAttackState();
            var stunnedState = CreateStunnedState();

            var listOfStates = new List<MyState>();
            listOfStates.Add(defState);
            listOfStates.Add(lookAtTargetState);
            if(_squad)
                listOfStates.Add(ambushState);
            listOfStates.Add(chaseState);
            //listOfStates.Add(waitingForAttack);
            //listOfStates.Add(motivateState);
            listOfStates.Add(meleeAttackState);
            listOfStates.Add(stunnedState);
            
            Init(entityState, listOfStates, new MyStateReader(defState));
        }


        private bool _status = false;
        public override void Update()
        {
            base.Update();
            
            if(!_squad || _squad.GetActiveSquadMembers<GolemEnemy>().Count <= 1)
                SetWorldState("onSquad", false);
            else
                SetWorldState("onSquad", true);

            bool attackCooldown = (Time.time - _lastAttackTime) >= _attackCooldown;
            SetWorldState("attackCooldown", attackCooldown);

            var playerColInAttackRange = Physics.OverlapSphere(transform.position, _attackRange, _playerLayer);
            if (playerColInAttackRange.Any())
                SetWorldState("inAttackRange", true);
            else
                SetWorldState("inAttackRange", false);
            if (!_squad)
            {
                
            }

            if (_stateReader.CurrentState() is AmbushState || _stateReader.CurrentState() is DoAnimationWhileLoockingAtTargetState)
            {
                if(_lastAmbushPositionAssigned.IsAtRange(transform.position, 0.5f))
                    SetWorldState("atAmbushPosition", true);
                else
                    SetWorldState("atAmbushPosition", false);
            }

            if (Physics.OverlapSphere(transform.position, _distanceToChase, _playerLayer).Any())
            {
                if (!_status)
                {
                    _status = true;
                    //_squad.Status = true;
                }
                if(_squad)
                    _squad.NotifySquadMembers("target", PlayerState.Instance.Transform);
                else
                    SetWorldState("target", PlayerState.Instance.Transform);
                return;
            }
            else
            {
                if (_status)
                {
                    Debug.Log("entre aca porque no esta en rango");
                    FindObjectOfType<MainSongFade>().SetFightStatus(false);
                    _status = false;

                    if (_squad)
                    {
                        _squad.Status = false;
                    }
                }
            }

            SetWorldState("target", null);
        }

        public void RefreshCooldownAttack(float timeOfAttack)
        {
            _lastAttackTime = timeOfAttack;
        }

        protected override void Die()
        {
            base.Die();
            Debug.LogWarning("GOLEM DIES");
            AudioMaster.Instance.PlayClip("RocasCayendo",0.3f);
            var instance = Instantiate(_explodeMesh);
            instance.transform.localScale = transform.localScale;
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
            
            if (TryGetComponent(out BossBehaviour boss))
            {
                FindObjectOfType<BossFightBehaviour>().SetFightStatus(false);
            }
            if (AchievementsManager.Instance != null)
            {
               AchievementsManager.Instance.TrackAchievement("kill_five_enemies");
               AchievementsManager.Instance.TrackAchievement("kill_ten_enemies");
               AchievementsManager.Instance.TrackAchievement("kill_fifteen_enemies");
            }
            _animator.enabled = false;
            //Destroy(gameObject);
            gameObject.SetActive(false);
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

        private StunnedState CreateStunnedState()
        {
            var stunnedPreConditions =
                new EntityState(new Dictionary<string, object>()
                {
                    {"isStunned", true}
                });
            var stunnedState = new StunnedState(stunnedPreConditions, 100);
            stunnedState.Init(_animator, this);
            return stunnedState;
        }

        private GenericMeleeAttackState CreateMeleeAttackState()
        {
            var attackPreConditions =
                new EntityState(new Dictionary<string, object>()
                {
                    {"inAttackRange", true},
                    {"attackCooldown", true}
                });
            var meleeAttackState = new GenericMeleeAttackState(attackPreConditions, MisionType.Kill, 6);
            meleeAttackState.Init(_animator, this);
            return meleeAttackState;
        }

        private ChaseState CreateChaseState()
        {
            var chasePreConditions =
                new EntityState(new Dictionary<string, object>()
                {
                    {"attackCooldown", true},
                    {"target", PlayerState.Instance.Transform}
                });
            var chaseState = new ChaseState(chasePreConditions, MisionType.GetTargetInRange,  5);
            chaseState.Init(_animator, _speed, transform, _rigidbody);
            return chaseState;
        }

        private DoAnimationWhileLoockingAtTargetState CreateMotivateToAttackState()
        {
            var waitingToAttackPreCon =
                new EntityState(new Dictionary<string, object>()
                {
                    //{"inAttackRange", true},
                    {"attackCooldown", true},
                    {"atAmbushPosition", true}
                });
            var waitingToAttack = new DoAnimationWhileLoockingAtTargetState(waitingToAttackPreCon, 4);
            waitingToAttack.Init(_animator, (_animator) =>
            {
                _animator.SetTrigger("Applause");
                _animator.SetBool("IsMoving", false);
            }, transform);
            return waitingToAttack;
        }

        private DoAnimationWhileLoockingAtTargetState CreateWaitingToAttackState()
        {
            var waitingToAttackPreCon =
                new EntityState(new Dictionary<string, object>()
                {
                    //{"inAttackRange", true},
                    {"atAmbushPosition", true}
                });
            var waitingToAttack = new DoAnimationWhileLoockingAtTargetState(waitingToAttackPreCon, 3);
            waitingToAttack.Init(_animator, (_animator) =>
            {
                _animator.SetTrigger("Idle");
                _animator.SetBool("IsMoving", false);
            }, transform);
            return waitingToAttack;
        }

        private AmbushState CreateAmbushState()
        {
            var ambushPreConditions =
                new EntityState(new Dictionary<string, object>()
                {
                    {"target", PlayerState.Instance.Transform},
                    {"onSquad", true}
                });
            var ambushState = new AmbushState(ambushPreConditions, MisionType.Ambush, 2);
            ambushState.Init(_animator, _speed, transform, _rigidbody, this);
            return ambushState;
        }
        
        private LookAtTargetState CreateLookAtTargetState()
        {
            var lookAtPreConditions = new EntityState(new Dictionary<string, object>()
            {
                {"target", PlayerState.Instance.Transform}
            });
            var lookAtTargetState = new LookAtTargetState(lookAtPreConditions, 1);
            lookAtTargetState.Init(_animator);
            return lookAtTargetState;
        }

        private IdleState CreateIdleState()
        {
            var idlePreConditions = new EntityState(new Dictionary<string, object>());
            var defState = new IdleState(idlePreConditions);
            defState.Init(_animator);
            return defState;
        }

        public override void Init(EntityState myEntityState, List<MyState> states, MyStateReader stateReader)
        {
            _myEntityState = myEntityState;
            _states = states.OrderByDescending(state => state.GetPrority()).ToList();
            _stateReader = stateReader;
        }

        public Transform Transform => transform;
        public ITarget CurrentTarget { get; }
        public Rigidbody Rigidbody => _rigidbody;
        public IMeleeWeapon CurrentWeapon => GetComponentInChildren<IMeleeWeapon>();
        public Transform GetTransform()
        {
            if(this)
                return transform;
            return null;
        }

        public float GetColliderHalfExtent()
        {
            return _colliderOffset;
        }

        public void ShowFeedback()
        {
            var currentMat = transform.GetComponentInChildren<SkinnedMeshRenderer>();
            //if (currentMat == null) return;
            currentMat.material = targetingMaterial;
            sword.GetComponent<MeshRenderer>().material = targetingSwordMaterial;
        }

        public void HideFeedback()
        {
            var currentMat = transform.GetComponentInChildren<SkinnedMeshRenderer>();
            //if (currentMat == null) return;
            currentMat.material = initialMaterial;
            sword.GetComponent<MeshRenderer>().material = swordInitialMaterial;
        }

        public override void OnAttackStart()
        {
            var attackParticleMain = attackParticle.main;
            attackParticleMain.startColor = Color.yellow;
            attackParticle.gameObject.SetActive(true);
            swordParticle.SetActive(true);
            var meshRenderers = eyes.GetComponentsInChildren<MeshRenderer>().ToList();
            foreach (var item in meshRenderers)
            {
                item.material.SetColor("_EmissionColor", Color.red * 30f);
            }
        }

        public override void OnChargedAttack()
        {
            var attackParticleMain = attackParticle.main;
            attackParticleMain.startColor = Color.red;
            attackParticle.gameObject.SetActive(false);
            attackParticle.gameObject.SetActive(true);
        }

        public override void OnAttackEnd()
        {
            attackParticle.gameObject.SetActive(false);
            swordParticle.SetActive(false);
            var meshRenderers = eyes.GetComponentsInChildren<MeshRenderer>().ToList();
            foreach (var item in meshRenderers)
            {
                item.material.SetColor("_EmissionColor", Color.red * 5f);
            }
            RefreshCooldownAttack(Time.time);
        }

        public override Vector3 MiddleBodyPoint
        {
            get
            {
                return transform.position + Vector3.up * _colliderOffset;
            }
        }

        public void Init(SquadManager squad)
        {
            _squad = squad;
        }

        public Vector3 GetAmbushPosition()
        {
            return _squad.GetAmbushPosition(this);
        }
        
        public Vector3 GetCurrentPosition()
        {
            return transform.position;
        }

        public void SetAmbusherExpectedPosition(Vector3 ambushPosition)
        {
            _lastAmbushPositionAssigned = ambushPosition;
        }

        public override void GetDamaged(int damage)
        {
            base.GetDamaged(damage);
            var currentState = _stateReader.CurrentState();
            var attackState = currentState as GenericMeleeAttackState;
            if (attackState != null)
            {
                if (!attackState.IsHitStoppingTheAttack()) return;
            }
            currentState.OnForceQuit();
            SetWorldState("isStunned", true);
            _animator.SetTrigger("GetHit");
            AudioMaster.Instance.PlayClip("GolemHit");

            if (TryGetComponent(out BossBehaviour boss))
            {
                boss.UpdateLifeView();
            }
            // AudioMaster.Instance.PlayClip("GolemHit");
        }
    }
}