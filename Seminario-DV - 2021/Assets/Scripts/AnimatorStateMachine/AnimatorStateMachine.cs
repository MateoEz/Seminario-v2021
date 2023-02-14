using System;
using System.Collections.Generic;
using AnimatorStateMachine.AnimatorStates;
using UnityEngine;

namespace AnimatorStateMachine
{
    public class AnimatorStateMachine : StateMachineBehaviour
    {
        [SerializeField] private List<AnimatorStateData> _onStateActions = new List<AnimatorStateData>();
        private List<AnimatorStateData> _cloneList = new List<AnimatorStateData>();

        private void Awake()
        {
            _cloneList.Clear();
            foreach (var state in _onStateActions)
            {
                _cloneList.Add(state.Clone());
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunAllStart(_cloneList, animator, stateInfo);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunAll(_cloneList, animator, stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunAllFinish(_cloneList, animator, stateInfo);
        }

        private void RunAllStart(List<AnimatorStateData> actions, Animator animator, AnimatorStateInfo stateInfo)
        {
            foreach (var action in actions)
            {
                action.StartState(animator, stateInfo);
            }
        }
        
        private void RunAll(List<AnimatorStateData> actions, Animator animator, AnimatorStateInfo stateInfo)
        {
            foreach (var action in actions)
            {
                action.UpdateState(animator, stateInfo);
            }
        }

        private void RunAllFinish(List<AnimatorStateData> actions, Animator animator, AnimatorStateInfo stateInfo)
        {
            foreach (var action in actions)
            {
                action.FinishState(animator, stateInfo);
            }
        }

        public List<AnimatorStateData> GetListOfActions()
        {
            return _cloneList;
        }
    }
}