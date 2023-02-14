using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPlayerFeedback : MonoBehaviour
{
    [SerializeField] private ParticleSystem _dashParticles;
    private ParticleSystem _currentActiveParticle;

    private void OnEnable()
    {
        _currentActiveParticle = Instantiate(_dashParticles, transform);
    }

    public void DisablePlayerFeedback()
    {
        Debug.LogWarning("Disable dashPlayerFeedback");
        if (_currentActiveParticle != null)
        {
            _currentActiveParticle.Stop();
            _currentActiveParticle.transform.parent = transform.parent;
        }
    }
}
