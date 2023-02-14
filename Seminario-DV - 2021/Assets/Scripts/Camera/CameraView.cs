using System;
using Domain;
using Player;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    private const string LIGHT_SHAKE = "LightShake";

    [Range(0,0.05f)]
    [SerializeField] private float _sensitivity;

    [SerializeField] private Transform _cameraPivotPoint;

    [SerializeField] private float _offsetFromColliders;

    [SerializeField] private GameConfig _gameConfig;
    public float OffsetFromColliders => _offsetFromColliders;
    
    private Animator _animator;
    private CameraPresenter _cameraPresenter;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _cameraPresenter = new CameraPresenter(this);
    }

    private void Update()
    {
        if (PlayerState.Instance.IsDead) return;
        //if (_gameConfig.Instance.IsPaused) return;
        _cameraPresenter.HorizontalOrbit();
        _cameraPresenter.VerticalOrbit();
        _cameraPresenter.AvoidObstacles();
    }

    public float HorizontalSensitivity
    {
        get
        {
            return _sensitivity;
        }
    }

    [SerializeField] private LayerMask _obstacleLayer;
    public LayerMask ObstaclesLayer => _obstacleLayer;

    public Transform GetCameraPivotPoint()
    {
        return _cameraPivotPoint;
    }

    public void LightShake()
    {
        _animator.SetTrigger(LIGHT_SHAKE);
    }
}