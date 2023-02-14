using UnityEngine;

namespace DefaultNamespace
{
    public interface IKnockBackable
    {
        void GetKnockedBack(Vector3 force);
        Vector3 GetCurrentPosition();
    }
}