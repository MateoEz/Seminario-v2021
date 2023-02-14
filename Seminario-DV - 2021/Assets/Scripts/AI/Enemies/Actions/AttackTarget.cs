using System;
using AI.Core.GOAP.BaseImplementations;
using AI.Core.GOAP.Core;
using AI.Enemies.BaseScripts;
using AI.Enemies.FSM_States;

namespace AI.Enemies.Actions
{
    public class AttackTarget : ReGoapAction<string, object>
    {
        private GoapEnemy _owner;

        protected override void Awake()
        {
            base.Awake();
            _owner = GetComponent<GoapEnemy>();
            preconditions.Set("isInAttackRange", true);
            effects.Set("targetDead", true);
        }
        
        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next,
            ReGoapState<string, object> settings, ReGoapState<string, object> goalState,
            Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);
            var sm = _owner.GetStateMachine();
            if (sm.IsActualState<PlayAnimationState>()) return;
            sm.SetGlobalAction((animator) => animator.SetTrigger("Attack"));
            sm.SetState<PlayAnimationState>();
            doneCallback(this);
        }
    }
}