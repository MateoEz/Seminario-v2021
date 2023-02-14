using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Core.StateMachine
{
    public abstract class BaseEnemyWithStateReader : MonoBehaviour, IDamageable, IBallSurroundingTarget
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private int _maxHealth;
        private int _currentHealth;

        protected EntityState _myEntityState;
        protected List<MyState> _states;
        protected MyStateReader _stateReader;

        public bool IsDead { get; set; }
        public MisionType CurrentMision => _stateReader.CurrentState().MissionType;

        public abstract void Init(EntityState myEntityState, List<MyState> states, MyStateReader stateReader);

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public abstract void Start();

        public virtual void Update()
        {
            _stateReader.Update();
        }

        public void SetWorldState(string key, object value)
        {
            if (_myEntityState.states.ContainsKey(key))
            {
                _myEntityState.states[key] = value;
            }
            else
            {
                _myEntityState.states.Add(key, value);
            }

            CheckChangeState();
        }

        private void CheckChangeState()
        {
            foreach (var state in _states)
            {
                if (state.GetPreConditions().IsSatisfiedBy(_myEntityState) && !state.IsBlocked)
                {
                    _stateReader.SetState(state);
                    return;
                }
            }

            _stateReader.SetDefault();
        }

        public virtual void GetDamaged(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            IsDead = true;
        }

        public abstract void OnAttackStart();
        public abstract void OnChargedAttack();
        public abstract void OnAttackEnd();

        public void BlockMisionType(MisionType misionMisionType)
        {
            foreach (var state in _states)
            {
                if (state.MissionType == misionMisionType) state.Block();
            }

            CheckChangeState();
        }

        public void UnlockMisionType(MisionType misionMisionType)
        {
            foreach (var state in _states)
            {
                if (state.MissionType == misionMisionType) state.Unlock();
            }

            CheckChangeState();
        }

        public EnemyType GetType()
        {
            return _type;
        }

        public abstract Vector3 MiddleBodyPoint { get; }
    }

    public enum EnemyType
    {
        Tank,
        Fearful
    }
}