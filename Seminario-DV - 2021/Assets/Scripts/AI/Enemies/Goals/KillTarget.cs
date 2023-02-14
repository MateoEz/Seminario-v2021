using AI.Core.GOAP.BaseImplementations;

namespace AI.Enemies.Goals
{
    public class KillTarget : ReGoapGoalAdvanced<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
            goal.Set("targetDead", true);
        }
    }
}