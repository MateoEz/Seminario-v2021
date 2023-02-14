using System;
using System.Linq;
using AI.Core.GOAP.BaseImplementations;
using AI.Core.GOAP.Core;
using ReGoap.Unity;
using UnityEngine;

namespace AI.Enemies.Sensors
{
    public class PlayerInSight : ReGoapSensor<string, object>
    {
        [SerializeField] private float _sightDistance;
        [SerializeField] private float _sightAngle;
        [SerializeField] private LayerMask _blockSightLayer;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private Transform _eyesTransform;
        private ReGoapAgent<string, object> _agent;
        private bool _test;

        private void Awake()
        {
            _agent = GetComponent<ReGoapAgent<string, object>>();
            Debug.LogWarning(_agent);
        }

        public override void Init(IReGoapMemory<string, object> memory)
        {
            base.Init(memory);
            if (this.memory != null) return;
            this.memory = GetComponent<IReGoapMemory<string, object>>();
        }

        public override void UpdateSensor()
        {
            var state = memory.GetWorldState();

            PlayerView playerInDistance = null;

            var players = Physics.OverlapSphere(transform.position, _sightDistance, _playerLayer)
                .Select(col => col.GetComponent<PlayerView>());
            if (players.Any())
            {
                playerInDistance = players.First();
            }

            if (playerInDistance == null)
            {
                state.Set("hasTarget", null);
                return;
            }

            var sightDir = transform.forward;
            var toPlayer = playerInDistance.transform.position - transform.position;
            if (Vector3.Angle(sightDir, toPlayer.normalized) > _sightAngle)
            {
                state.Set("hasTarget", null);
                return;
            }

            if (Physics.Raycast(_eyesTransform.position, toPlayer.normalized, toPlayer.magnitude, _blockSightLayer))
            {
                state.Set("hasTarget", null);
                return;
            }

            state.Set("target", playerInDistance.transform);
            state.Set("hasTarget", true);
        }
    }
}