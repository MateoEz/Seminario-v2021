using DefaultNamespace;
using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "NewOnAirKnockedState", menuName = "AnimatorStates/OnAirKnockedState")]
    public class AnimatorOnAirWhileKnockedState : AnimatorStateData
    {
        private const float GROUNDED_RADIUS = 0.05f;
        
        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<AnimatorOnAirWhileKnockedState>();
            return instance;
        }
        
        public override void UpdateState(Animator animator, AnimatorStateInfo stateInfo)
        {
            var entity = GetEntity(animator);
            var entityTransform = entity.Transform;
            var collider = entity.Transform.GetComponent<BoxCollider>();
            if (Physics.OverlapBox(entityTransform.position + collider.center,
                collider.size * 0.45f +
                Vector3.up * GROUNDED_RADIUS, //El 0.45 es para evitar que sobrepase el borde del collider
                entityTransform.rotation,
                1 << 10).Length > 0)
            {
                animator.SetTrigger("KnockGroundHit");
            }
        }
        
        private IEntityView GetEntity(Animator anim)
        {
            return anim.GetComponent<IEntityView>();
        }
    }
}