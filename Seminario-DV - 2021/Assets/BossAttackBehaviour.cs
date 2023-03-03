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
    [SerializeField] private Material eyesMaterial;

    private float _initialEmission;
    private float _tick;
    void Start()
    {
    }

    private void Update()
    {
        _tick += Time.deltaTime;

        eyesMaterial.SetVector("_EmissionColor",Color.red * _tick * 3);
        if (_tick >= cooldown)
        {
            Shoot();
            _tick = 0;
        }
    }

    private void Shoot()
    {
        var bulletRef = Instantiate(bullet);
        bulletRef.transform.position = bulletSpawnPoint.position;
        eyesMaterial.SetVector("_EmissionColor",Color.red * 0.7f);
    }

    private void OnDisable()
    {
        Destroy(this);
    }
}
