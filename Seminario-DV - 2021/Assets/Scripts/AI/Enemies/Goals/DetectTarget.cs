using AI.Core.GOAP.BaseImplementations;
using UnityEngine;

namespace AI.Enemies.Goals
{
    public class DetectTarget : ReGoapGoalAdvanced<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
            goal.Set("hasTarget", true);
        }
    }
}
