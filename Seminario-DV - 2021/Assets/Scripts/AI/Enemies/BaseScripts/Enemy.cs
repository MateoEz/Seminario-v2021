using Player;
using UnityEngine;

namespace AI.Enemies.BaseScripts
{
    public abstract class Enemy : MonoBehaviour, IDamageable, ITarget
    {
        [SerializeField] protected int _health;
        [SerializeField] protected int _damage;
        [SerializeField] protected float _speed;

        public virtual void GetDamaged(int damage)
        {
            _health -= damage;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public float GetColliderHalfExtent()
        {
            throw new System.NotImplementedException();
        }

        public void ShowFeedback()
        {
            throw new System.NotImplementedException();
        }

        public void HideFeedback()
        {
            throw new System.NotImplementedException();
        }
    }
}
