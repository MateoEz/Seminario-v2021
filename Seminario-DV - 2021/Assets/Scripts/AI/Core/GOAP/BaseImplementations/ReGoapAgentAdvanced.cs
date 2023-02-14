﻿namespace AI.Core.GOAP.BaseImplementations
{
    public class ReGoapAgentAdvanced<T, W> : ReGoapAgent<T, W>
    {
        protected virtual void Update()
        {
            possibleGoalsDirty = true;

            if (currentActionState == null)
            {
                if (!IsPlanning)
                    CalculateNewGoal();
                return;
            }
        }
    }
}