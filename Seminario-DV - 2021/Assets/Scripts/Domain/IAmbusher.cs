using UnityEngine;

namespace Domain
{
    public interface IAmbusher : ISquadMember
    {
        void ResetAnimation();
        Vector3 GetAmbushPosition();
        Vector3 GetCurrentPosition();
        void SetAmbusherExpectedPosition(Vector3 ambushPosition);
    }

    public interface ISquadMember
    {
        void Init(SquadManager squad);
    }
}