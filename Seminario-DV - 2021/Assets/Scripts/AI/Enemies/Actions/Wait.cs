using System;
using System.Collections.Generic;
using AI.Core.GOAP.BaseImplementations;
using AI.Core.GOAP.Core;
using AI.Enemies.BaseScripts;
using AI.Enemies.FSM_States;
using DefaultNamespace;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace AI.Enemies.Actions
{
    public class Wait : ReGoapAction<string, object>, IObserver
    {
        private GoapEnemy _owner;
        [SerializeField] private List<IReGoapSensor<string, object>> _sensors;
        private ReGoapState<string, object> _state;

        protected override void Awake()
        {
            base.Awake();
            _owner = GetComponent<GoapEnemy>();
            effects.Set("hasTarget", true);
        }

        protected override void Start()
        {
            base.Start();
            _state = agent.GetMemory().GetWorldState();
            _state.RegisterObserver(this);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done,
            Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            var sm = _owner.GetStateMachine();
            sm.SetGlobalAction((animator) => animator.SetBool("IsMoving", false));
            sm.SetState<Idle>();
        }

        public void OnNotify()
        {
            if (_state.HasKey("hasTarget") && _state.Get("hasTarget") != null &&(bool) _state.Get("hasTarget"))
                doneCallback(this);
        }
    }
}