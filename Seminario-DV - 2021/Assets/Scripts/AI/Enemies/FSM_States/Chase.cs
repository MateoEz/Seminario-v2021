using System;
using System.Configuration;
using AI.Core.StateMachine;
using UnityEngine;

namespace AI.Enemies.FSM_States
{
    public class Chase : MyState
    {
        /*
        private Transform _target;
        private Transform _myTransform;

        
        public Chase(StateMachineDaVinci sm) : base(sm)
        {
        }

        public override void Awake()
        {
            var owner = _sm.GetOwner();
            _myTransform = owner.GetComponent<Transform>();
            _target = (Transform) owner.GetAgent().GetMemory().GetWorldState().Get("target");
            if(_target == null) throw new Exception("Chase state has no target to chase");
            var action = _sm.GetGlobalAction();
            action(owner.GetAnimator());
        }

        public override void Execute()
        {
            base.Execute();
            var toTarget = _target.position - _myTransform.position;
            _sm.GetOwner().Move(toTarget.normalized);
        }
        */


        public Chase(StateMachineDaVinci sm, EntityState preConditions, int priority = 0) : base(preConditions, priority)
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