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
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");


    private bool cooldownEnded = true;

    private void Update()
    {
        if (cooldownEnded)
        {
            _tick += Time.deltaTime;
        }
        eyesMaterial.SetVector(EmissionColor,Color.red * _tick);
        if (_tick >= cooldown)
        {
            Shoot();
            _tick = 0;
            cooldownEnded = false;
        }
    }

    private void Shoot()
    {
        var bulletRef = Instantiate(bullet);
        bulletRef.transform.position = bulletSpawnPoint.position;
        eyesMaterial.SetVector(EmissionColor,Color.red * 0.7f);
        StartCoroutine(CooldownEndedCoroutine(3));
    }

    private IEnumerator CooldownEndedCoroutine(int i)
    {
        yield return new WaitForSeconds(i);
        cooldownEnded = true;
    }

    private void OnDisable()
    {
        Destroy(this);
    }
}
