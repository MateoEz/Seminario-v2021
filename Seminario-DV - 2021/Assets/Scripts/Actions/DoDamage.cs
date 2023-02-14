using UnityEngine;
using Weapons;

namespace Actions
{
    public class DoDamage
    {
        private readonly CameraView _cameraAnimator;
        
        public DoDamage(CameraView cameraAnimator)
        {
            _cameraAnimator = cameraAnimator;
        }
        
        public void Execute(int damage, IDamageable damageable)
        {
            _cameraAnimator.LightShake();
            damageable.GetDamaged(damage);
        }
    }
}