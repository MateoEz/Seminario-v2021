using System;
using System.Collections.Generic;
using Actions;
using UnityEngine;

namespace Weapons
{
    public class PlayerOneHandSword : MonoBehaviour, IMeleeWeapon
    {
        [SerializeField] private ParticleSystem _hitPartcles;
        
        private readonly List<IDamageable> _damaged = new List<IDamageable>();
        
        private DoDamage _doDamage;

        private void Awake()
        {
            _doDamage = new DoDamage(Camera.main.GetComponentInParent<CameraView>()); //Esto deberia tenerlo un presenter
        }

        public int Damage { get; set; }

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
            Instantiate(_hitPartcles, transform.position/*other.ClosestPoint(transform.position)*/, Quaternion.Euler(new Vector3(0,0,-180)));
            _doDamage.Execute(Damage, toDamage); //La weapon deberia tener un presenter, inicializarlo, y usarlo para hacer la logica del daño
            _damaged.Add(toDamage);
        }
    }
}