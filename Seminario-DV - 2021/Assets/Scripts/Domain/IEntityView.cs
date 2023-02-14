using Player;
using UnityEngine;
using Weapons;

namespace DefaultNamespace
{
    public interface IEntityView
    {
        Transform Transform { get; }
        ITarget CurrentTarget { get; }
        Rigidbody Rigidbody { get; }
        IMeleeWeapon CurrentWeapon { get; }
    }
}