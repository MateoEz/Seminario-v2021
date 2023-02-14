using Player;
using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "NewJumpActiveState", menuName = "AnimatorStates/ActiveJumpState")]
    public class AnimatorActivateJump : AnimatorStateData
    {
        public override void StartState(Animator animator, AnimatorStateInfo stateInfo)
        {
            //PlayerState.Instance.IsInIdleOrMoving = true;
        }

        public override void FinishState(Animator animator, AnimatorStateInfo stateInfo)
        {
            //PlayerState.Instance.IsInIdleOrMoving = false;
        }

        public override AnimatorStateData Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
