using AI.Core.StateMachine;

namespace AI.Enemies.FSM_States
{
    public class Idle : MyState
    {
        /*
        public Idle(StateMachineDaVinci sm) : base(sm)
        {
        }

        public override void Awake()
        {
            base.Awake();
            var action = _sm.GetGlobalAction();
            action(_sm.GetOwner().GetAnimator());
        }
        */
        public Idle(StateMachineDaVinci sm, EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public override bool CanChangeState()
        {
            throw new System.NotImplementedException();
        }
    }
}