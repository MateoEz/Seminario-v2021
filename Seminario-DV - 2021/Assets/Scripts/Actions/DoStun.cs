using UnityEngine;

namespace AI.Enemies.Spells
{
    public class DoStun
    {
        public void Invoke(IStunable entity, float timeStunned)
        {
            Debug.Log(entity);
            entity.GetStunned(timeStunned);
        }
    }
}