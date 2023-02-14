using AI.Core.GOAP.BaseImplementations;
using AI.Core.GOAP.Core;
using AI.Core.StateMachine;
using UnityEngine;

namespace AI.Enemies.BaseScripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(IReGoapAgent<string,object>))]
    [RequireComponent(typeof(Rigidbody))]
    
    public abstract class GoapEnemy : Enemy
    {
        protected Animator _animator;
        protected StateMachineDaVinci _stateMachine;
        protected Rigidbody _rigi;
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigi = GetComponent<Rigidbody>();
            _stateMachine = new StateMachineDaVinci(this);
        }

        public StateMachineDaVinci GetStateMachine()
        {
            return _stateMachine;
        }

        public IReGoapAgent<string, object> GetAgent()
        {
            return GetComponent<IReGoapAgent<string, object>>();
        }

        public void Move(Vector3 dir)
        {
            var realDir = new Vector3(dir.x, 0, dir.z).normalized;
            transform.forward = realDir;
            _rigi.velocity = transform.forward * _speed;
        }

        public Animator GetAnimator()
        {
            return _animator;
        }
    }
}