using System;
using System.Linq;
using AI.Core.GOAP.BaseImplementations;
using AI.Core.GOAP.Core;
using ReGoap.Unity;
using UnityEngine;

namespace AI.Enemies.Sensors
{
    public class PlayerInAttackRange : ReGoapSensor<string, object>
    {
        [SerializeField] private float _attackRange;
        [SerializeField] private LayerMask _playerLayer;
        private ReGoapAgent<string, object> _agent;

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
            var players = Physics.OverlapSphere(transform.position, _attackRange, _playerLayer)
                .Select(col => col.GetComponent<PlayerView>())
                .Where(player => player != null);
            if (players.Any())
            {
                state.Set("isInAttackRange", true);
                return;
            }
            
            state.Set("isInAttackRange", false);
        }
    }
}