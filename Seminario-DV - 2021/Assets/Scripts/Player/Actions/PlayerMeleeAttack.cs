using System;
using UniRx;
using UnityEditor;
using UnityEngine;

public class PlayerMeleeAttack
{
    public void Execute(PlayerView view, bool hasTarget = true)
    {
        view.AttackAnimation();
        if (hasTarget)
        {
            view.Rigidbody.velocity *= 0;
        }
        else
        {
            view.DebuffSpeed();
        }
    }
    
    /*
    public void Execute(float secondsToHit, Vector3 hitCenter, float hitRadius, LayerMask enemiesLayer)
    {
        WaitSeconds(secondsToHit)
            .DoOnCompleted(() => Hit(hitCenter, hitRadius, enemiesLayer))
            .Subscribe();
    }

    
    private void Hit(Vector3 hitCenter, float hitRadius, LayerMask enemiesLayer)
    {
        var enemiesColliders = Physics.OverlapSphere(hitCenter, hitRadius, enemiesLayer);
        for (int i = 0; i < enemiesColliders.Length; i++)
        {
            Debug.Log(enemiesColliders[i].name);
        }
    }

    private IObservable<Unit> WaitSeconds(float seconds)
    {
        return Observable.ReturnUnit().Delay(TimeSpan.FromSeconds(seconds));
    }
    */
}
