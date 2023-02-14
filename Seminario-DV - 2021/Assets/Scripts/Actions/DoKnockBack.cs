using DefaultNamespace;
using UnityEngine;

namespace Actions
{
    public class DoKnockBack
    {
        public void Execute(Vector3 knockBackForce, IKnockBackable entity)
        {
            entity.GetKnockedBack(knockBackForce);
        }
    }
}