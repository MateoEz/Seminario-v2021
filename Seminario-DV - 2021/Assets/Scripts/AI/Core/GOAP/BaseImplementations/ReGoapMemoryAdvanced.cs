using AI.Core.GOAP.Core;
using UnityEngine;

namespace AI.Core.GOAP.BaseImplementations
{
    public class ReGoapMemoryAdvanced<T, W> : ReGoapMemory<T, W>
    {
        private IReGoapSensor<T, W>[] sensors;

        public float SensorsUpdateDelay = 0.3f;
        private float sensorsUpdateCooldown;
        protected override void Awake()
        {
            base.Awake();
            sensors = GetComponents<IReGoapSensor<T, W>>();
            foreach (var sensor in sensors)
            {
                sensor.Init(this);
            }
        }

        protected virtual void Update()
        {
            if (Time.time > sensorsUpdateCooldown)
            {
                sensorsUpdateCooldown = Time.time + SensorsUpdateDelay;

                foreach (var sensor in sensors)
                {
                    sensor.UpdateSensor();
                }
            }
        }
    }
}
