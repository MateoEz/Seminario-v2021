using System;
using System.Collections.Generic;
using System.Linq;
using MyUtilities;
using UnityEngine;

namespace Domain.Services
{
    public class AmbushService
    {
        private const float AMBUSH_DISTANCE = 7f;

        private List<IAmbusher> _ambushers;
        
        private Transform _target;

        public void Init(List<IAmbusher> ambushers, Transform target)
        {
            _ambushers = ambushers;
            //_nearestAmbusher = ambushers.OrderBy(ambusher => Vector3.Distance(target.position, ambusher.GetCurrentPosition())).First();
            _target = target;
        }


        private bool _alreadyReset = false;
        public Vector3 AmbushPositionOf(IAmbusher ambusher)
        {
            if (_ambushers.Count <= 1 && !_alreadyReset) 
            {
                ambusher.ResetAnimation();
                _alreadyReset = true;
                throw new Exception("Estan intentando ambushear menos de 1");
            }

            var initialAngle = Vector3.Angle(new Vector3(1, 0, 0).normalized,
                Utils.GetDirIgnoringHeight(_target.position, _ambushers[0].GetCurrentPosition()));
            
            var angleToIncrease = 360f / _ambushers.Count;
            
            var positionsToBeOcupated = new List<Vector3>();

            for (int i = 0; i < _ambushers.Count; i++)
            {
                positionsToBeOcupated.Add(GetDirVector(initialAngle, i, angleToIncrease) * AMBUSH_DISTANCE + _target.position);
            }
            
            return positionsToBeOcupated[_ambushers.IndexOf(ambusher)];
        }

        private Vector3 GetDirVector(float initialAngle, int i, float angleToIncrease)
        {
            var parsedAngle = initialAngle + i * angleToIncrease;
            if (parsedAngle >= 360) parsedAngle -= 360;
            return new Vector3(Mathf.Cos((parsedAngle) * Mathf.Deg2Rad),
                0, Mathf.Sin((parsedAngle) * Mathf.Deg2Rad));
        }
    }
}