using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "NewTriggerParameter", menuName = "AnimatorStates/TriggerParameter")]
    public class TriggerParameter : AnimatorStateData
    {
        [Range(0,1)]
        [SerializeField] private float normalizedTime;
        [SerializeField] private string triggerName;
        
        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<TriggerParameter>();
            instance.normalizedTime = normalizedTime;
            instance.triggerName = triggerName;
            return instance;
        }
        
        public override void UpdateState(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime >= normalizedTime)
            {
                animator.SetTrigger(triggerName);
            }
        }
    }
}
