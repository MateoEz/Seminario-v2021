using System;
using AI.Core.GOAP.BaseImplementations;
using AI.Core.GOAP.Core;
using AI.Enemies.BaseScripts;
using AI.Enemies.FSM_States;
using DefaultNamespace;
using UnityEngine;

namespace AI.Enemies.Actions
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GoapEnemy))]
    public class ChaseTarget : ReGoapAction<string, object>, IObserver
    {
        private GoapEnemy _owner;
        private ReGoapState<string, object> _state;

        protected override void Awake()
        {
            base.Awake();
            _owner = GetComponent<GoapEnemy>();
            preconditions.Set("hasTarget", true);
            effects.Set("isInAttackRange", true);
        }

        protected override void Start()
        {
            base.Start();
            _state = agent.GetMemory().GetWorldState();
            _state.RegisterObserver(this);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            var sm = _owner.GetStateMachine();
            sm.SetGlobalAction((animator) => animator.SetBool("IsMoving", true));
            sm.SetState<Chase>();
        }
        
        public override bool CheckProceduralCondition(GoapActionStackData<string, object> stackData)
        {
            return base.CheckProceduralCondition(stackData) && !stackData.settings.HasKey("target");
        }

        public void OnNotify()
        {
            if (_state.HasKey("isInAttackRange") && (bool) _state.Get("isInAttackRange"))
                doneCallback(this);
            if (_state.HasKey("hasTarget") && _state.Get("hasTarget") != null && (bool) _state.Get("hasTarget"))
                failCallback(this);
        }
    }
}