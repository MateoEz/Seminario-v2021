using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BossAttackBehaviour : MonoBehaviour
{
    [SerializeField] private TorretaBullet bullet;
    [SerializeField] private float cooldown;
    [SerializeField] private Transform bulletSpawnPoint;

    void Start()
    {
        InvokeRepeating(nameof(Shoot),cooldown,cooldown);
    }
    
    private void Shoot()
    {
        var bulletRef = Instantiate(bullet);
        bulletRef.transform.position = bulletSpawnPoint.position;
    }

    private void OnDisable()
    {
        Destroy(this);
    }
}
