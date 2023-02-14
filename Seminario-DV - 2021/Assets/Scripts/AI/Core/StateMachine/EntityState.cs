using System.Collections.Generic;

namespace AI.Core.StateMachine
{
    public class EntityState
    {
        public Dictionary<string, object> states;
        
        public EntityState(Dictionary<string, object> states)
        {
            this.states = states;
        }

        public bool IsSatisfiedBy(EntityState entityState)
        {
            foreach (var state in states)
            {
                if (entityState.states[state.Key] == null) return false;
                if (!Equals(entityState.states[state.Key], state.Value)) return false;
            }
            return true;
        }
    }
}