using AI.Core.GOAP.BaseImplementations;

namespace AI.Enemies.Goals
{
    public class SurroundTarget : ReGoapGoalAdvanced<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
            goal.Set("targetSurrounded", true);
        }
    }
}