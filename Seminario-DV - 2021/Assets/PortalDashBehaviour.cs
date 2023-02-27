using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PortalDashBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject wallBlock;

    private float _tick;
    
    private void Update()
    {
        _tick += Time.deltaTime;
        if (!(_tick > 2)) return;
        wallBlock.SetActive(!wallBlock.activeSelf);
        _tick = 0;
    }
}
