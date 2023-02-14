using System;
using AI.Core.StateMachine;
using AI.Enemies.Spells;
using MyUtilities;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class CastingSpellState : MyState
    {
        private Animator _animator;
        private Action<Animator> _animations;
        private BaseEnemyWithStateReader _owner;
        private float _timer;
        private ISpell _spellToCast;

        public CastingSpellState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public CastingSpellState(EntityState preConditions, MisionType missionType, int priority = 0) : base(preConditions, missionType, priority)
        {
        }

        public void Init(Animator animator, Action<Animator> animations, BaseEnemyWithStateReader owner, ISpell spellToCast)
        {
            _spellToCast = spellToCast;
            _animator = animator;
            _animations = animations;
            _owner = owner;
        }

        public override void Awake()
        {
            if (!_owner) return;
            _owner.transform.forward =
                Utils.GetDirIgnoringHeight(_owner.transform.position, PlayerState.Instance.Transform.position);
            if (_spellToCast is StunMiniGrootSpell)
            {
                var stunSpell = _spellToCast as StunMiniGrootSpell;
                stunSpell.Init(PlayerState.Instance.Transform.position);
            }
            _timer = 0;
            _animator.ResetTrigger("Idle");
            _animations(_animator);
        }

        public override void Execute()
        {
            _timer += Time.deltaTime;
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
            if(_animator.GetCurrentAnimatorStateInfo(0).length <= _timer)
                _owner.SetWorldState("doSpell", true);
        }

        public override void Sleep()
        {
            var stunspell = _spellToCast as StunMiniGrootSpell;
            stunspell.TurnOff();
        }

        public override bool CanChangeState()
        {
            return true;
        }
    }
}