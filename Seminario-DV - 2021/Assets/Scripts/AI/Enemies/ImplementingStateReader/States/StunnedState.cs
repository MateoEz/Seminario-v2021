using System.Linq;
using AI.Core.StateMachine;
using AnimatorStateMachine.AnimatorStates.ActionsScripts;
using DefaultNamespace;
using MyUtilities;
using Player;
using UnityEngine;

namespace AI.Enemies.ImplementingStateReader.States
{
    public class StunnedState : MyState, IObserver
    {
        private bool _canChangeState;
        private Animator _animator;
        private BaseEnemyWithStateReader _owner;

        public StunnedState(EntityState preConditions, int priority = 0) : base(preConditions, priority)
        {
        }

        public StunnedState(EntityState preConditions, MisionType missionType, int priority = 0) : base(preConditions, missionType, priority)
        {
        }
        
        public void Init(Animator animator, BaseEnemyWithStateReader owner)
        {
            _owner = owner;
            _animator = animator;
            var animStatesMachines = _animator.GetBehaviours<AnimatorStateMachine.AnimatorStateMachine>();
            foreach (var sm in animStatesMachines)
            {
                var actions = sm.GetListOfActions().Where(state => state.GetType() == typeof(AnimatorStunnedState)).ToList();
                foreach (AnimatorStunnedState action in actions)
                {
                    action.RegisterObserver(this);
                }
            }
        }

        public override void Awake()
        {
            _canChangeState = false;
        }

        public override void Execute()
        {
            LookAtPlayer();
        }

        private void LookAtPlayer()
        {
            _owner.transform.forward = Utils.GetDirIgnoringHeight(_owner.transform.position, PlayerState.Instance.Transform.position);
        }

        public override bool CanChangeState()
        {
            return _canChangeState;
        }

        public void OnNotify()
        {
            _canChangeState = true;
            _owner.SetWorldState("isStunned", false);
        }
    }
}