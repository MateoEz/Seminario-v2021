using UnityEngine;

namespace AI.Enemies.Spells
{
    public interface ISpellView
    {
        void SetPosition(Vector3 position);

        void Active();
        void Disactive();
    }
}