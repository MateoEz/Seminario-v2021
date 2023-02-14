using System;
using AI.Core.StateMachine;
using UnityEngine;

namespace AI.Enemies.FSM_States
{
    public class PlayAnimationState : MyState
    {
        /*
        public PlayAnimationState(StateMachineDaVinci sm) : base(sm)
        {
        }

        public override void Awake()
        {
            base.Awake();
            var action = _sm.GetGlobalAction();
            action(_sm.GetOwner().GetAnimator());
        }
        */
        public PlayAnimationState(StateMachineDaVinci sm, EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override bool CanChangeState()
        {
            throw new NotImplementedException();
        }
    }
}