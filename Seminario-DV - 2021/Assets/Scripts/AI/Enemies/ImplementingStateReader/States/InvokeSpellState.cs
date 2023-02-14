using AI.Core.StateMachine;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class InvokeSpellState : MyState
    {
        private CastSpell _castSpell;
        private ISpell _spell;
        private BaseEnemyWithStateReader _owner;

        public InvokeSpellState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public InvokeSpellState(EntityState preConditions, MisionType missionType, int priority = 0) : base(preConditions, missionType, priority)
        {
        }
        
        public void Init(BaseEnemyWithStateReader owner, CastSpell castSpell, ISpell spell)
        {
            _owner = owner;
            _castSpell = castSpell;
            _spell = spell;
        }

        public override void Awake()
        {
            base.Awake();
            _castSpell.Invoke(_spell);
            _owner.SetWorldState("isSpellInCooldown", false);
            _owner.SetWorldState("doSpell", false);
        }

        public override void Execute()
        {
            
        }

        public override bool CanChangeState()
        {
            return true;
        }
    }
}