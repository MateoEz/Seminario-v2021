using System.Collections.Generic;
using Actions;
using DefaultNamespace;
using UnityEngine;

namespace Weapons
{
    public class BasicMeleeWeaponWithKnockBack : MonoBehaviour, IMeleeWeapon
    {
        [SerializeField] private ParticleSystem _hitPartcles;
        [SerializeField] private float _knockBackForce;
        [SerializeField] private Transform _ownerTransform;
        
        private readonly List<IDamageable> _damaged = new List<IDamageable>();
        
        private DoDamage _doDamage;
        private DoKnockBack _doKnockBack;

        private void Awake()
        {
            _doDamage = new DoDamage(Camera.main.GetComponentInParent<CameraView>()); //Esto deberia tenerlo un presenter
            _doKnockBack = new DoKnockBack();
        }

        public int Damage { get; set; }
        public Vector3 KnockBackForce { get; set; }

        public void EnableCollision()
        {
            GetComponent<Collider>().enabled = true;
        }

        public void DisableCollision()
        {
            GetComponent<Collider>().enabled = false;
            _damaged.Clear();
        }

        private void OnTriggerStay(Collider other)
        {
            var toDamage = other.GetComponent<IDamageable>();
            if (toDamage == null) return;
            if (_damaged.Contains(toDamage)) return;
            if(_hitPartcles)
                Instantiate(_hitPartcles, other.ClosestPoint(transform.position), Quaternion.Euler(new Vector3(0,0,-180)));
            _doDamage.Execute(Damage, toDamage); //La weapon deberia tener un presenter, inicializarlo, y usarlo para hacer la logica del daño
            _damaged.Add(toDamage);
            var toKnock = other.GetComponent<IKnockBackable>();
            if (toKnock == null) return;
            Vector3 toKnockPosition = toKnock.GetCurrentPosition();
            Vector3 knockDirection = Vector3.Normalize(toKnockPosition - _ownerTransform.position);
            Vector3 knockDirectionWithHight = new Vector3(knockDirection.x, new Vector2(knockDirection.x, knockDirection.z).magnitude, knockDirection.z);
            Vector3 finalKnockVector = knockDirectionWithHight * _knockBackForce;
            _doKnockBack.Execute(finalKnockVector, toKnock);
        }
    }
}