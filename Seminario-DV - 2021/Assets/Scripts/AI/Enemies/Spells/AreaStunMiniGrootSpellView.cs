using UnityEngine;

namespace AI.Enemies.Spells
{
    public class AreaStunMiniGrootSpellView : MonoBehaviour, ISpellView
    {
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Active()
        {
            gameObject.SetActive(true);
            GetComponent<Animator>().SetTrigger("START");
        }

        public void Disactive()
        {
            gameObject.SetActive(false);
        }
    }
}