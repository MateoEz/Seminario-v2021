using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates
{
    public abstract class AnimatorStateData : ScriptableObject
    {
        public virtual void StartState(Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public virtual void UpdateState(Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public virtual void FinishState(Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public abstract AnimatorStateData Clone();
    }
}
