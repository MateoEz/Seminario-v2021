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
        Instantiate(bullet, bulletSpawnPoint.position,bulletSpawnPoint.rotation);
    }
}
