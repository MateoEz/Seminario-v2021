using DefaultNamespace;
using MyUtilities;
using Player;
using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "NewLookAtPlayerState", menuName = "AnimatorStates/LookAtPlayerState")]
    public class AnimatorLookAtPlayerState : AnimatorStateData
    {
        private IEntityView _entity;

        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<AnimatorLookAtPlayerState>();
            return instance;
        }

        public override void StartState(Animator animator, AnimatorStateInfo stateInfo)
        {
            _entity = GetEntity(animator);
        }

        public override void UpdateState(Animator animator, AnimatorStateInfo stateInfo)
        {
            _entity.Transform.forward =
                Utils.GetDirIgnoringHeight(_entity.Transform.position, PlayerState.Instance.Transform.position);
        }

        private IEntityView GetEntity(Animator anim)
        {
            return anim.GetComponent<IEntityView>();
        }
    }
}