using System;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class DummyEnemy : MonoBehaviour, IDamageable, ITarget
    {
        [SerializeField] private float _colliderOffset;

        [SerializeField] private Material initialMaterial;
        [SerializeField] private Material targetingMaterial;

        public void GetDamaged(int damage)
        {
            float random = Random.Range(0.02f, 0.08f);
            AudioMaster.Instance.PlayClip("HitDummy",random,0.6f);
            Debug.Log("Damage received: " + damage);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public float GetColliderHalfExtent()
        {
            return _colliderOffset;
        }

        public void ShowFeedback()
        {
            var currentMat = transform.GetComponent<MeshRenderer>();
            if (currentMat)
            {
                currentMat.material = targetingMaterial;
            }
          
        }

        public void HideFeedback()
        {

            var currentMat = transform.GetComponent<MeshRenderer>();
            if (currentMat)
            {
                currentMat.material = initialMaterial;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _colliderOffset);
        }
    }
}