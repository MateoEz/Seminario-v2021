using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] blockWalls;
    [SerializeField] private GameObject bossLifeContainer;
    [SerializeField] private CameraView cameraView;
    private bool _currentStatus;


    private float _tick;
    private void Update()
    {
        if (_currentStatus)
        {
            _tick += Time.deltaTime;
            if (_tick > 0.7f)
            {
                cameraView.LightShake();
                _tick = 0;
            }
        }
    }

    public void SetFightStatus(bool status)
    {
        foreach (var block in blockWalls)
        {
            block.SetActive(status);
            bossLifeContainer.SetActive(status);
        }

        _currentStatus = status;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {
            SetFightStatus(true);

        }
    }
}
