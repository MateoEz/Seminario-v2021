using AI.Enemies.BaseScripts;
using AI.Enemies.FSM_States;
using DefaultNamespace;
using Player;
using UnityEngine;
using Weapons;

namespace AI.Enemies.ConcreteEnemies
{
    public class Golem : GoapEnemy, IEntityView
    {
        [SerializeField] protected float _attackRange;
        [SerializeField] protected float _distanceToAdvanceInAttack;

        public Transform Transform => transform;

        public ITarget CurrentTarget => null;

        public Rigidbody Rigidbody => GetComponent<Rigidbody>();

        public IMeleeWeapon CurrentWeapon => GetComponentInChildren<IMeleeWeapon>();

        protected override void Awake()
        {
            base.Awake();
            /*
            _stateMachine.AddState(new Idle(_stateMachine));
            _stateMachine.AddState(new Chase(_stateMachine));
            _stateMachine.AddState(new PlayAnimationState(_stateMachine));
            */
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}
