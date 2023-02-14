using UnityEngine;

namespace Player
{
    public interface ITarget
    {
        Transform GetTransform();
        float GetColliderHalfExtent();
        void ShowFeedback();
        void HideFeedback();
    }
}