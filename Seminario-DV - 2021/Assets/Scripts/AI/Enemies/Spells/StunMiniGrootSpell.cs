using System.Linq;
using Domain;
using Player;
using UnityEngine;

namespace AI.Enemies.Spells
{
    public class StunMiniGrootSpell : ISpell
    {
        private Vector3 _spellPosition;
        private DoStun _stun;
        private readonly GameConfig _config;
        private readonly StunMiniGrootSpellView _view;
        private readonly AreaStunMiniGrootSpellView _areaView;

        public StunMiniGrootSpell(GameConfig config, StunMiniGrootSpellView view, AreaStunMiniGrootSpellView areaView)
        {
            _config = config;
            _view = view;
            _areaView = areaView;
            _stun = new DoStun();
        }
        
        public void Init(Vector3 spellPosition)
        {
            _spellPosition = spellPosition;
            _areaView.SetPosition(_spellPosition);
            _areaView.Active();
            _view.Init(_config.Instance.StunnedTimeOfMiniGrootSpell);
        }

        public void TurnOff()
        {
            _areaView.Disactive();
        }
        
        public void Cast()
        {
            Debug.Log("Player pos: " + PlayerState.Instance.Transform.position);
            Debug.Log("Spell pos: " + _spellPosition);
            
            var playerColliders = Physics.OverlapSphere(_spellPosition, 3, 1 << 16).Where( x=> x.GetComponent<IStunable>() != null).ToArray();
            if (playerColliders.Length > 0)
            {
                _view.SetPosition(playerColliders[0].transform.position);
                _stun.Invoke(playerColliders[0].GetComponent<IStunable>(), _config.Instance.StunnedTimeOfMiniGrootSpell);
            }
            else
            {
                _view.SetPosition(_spellPosition);
            }
            _view.Active();
            _areaView.Disactive();
        }

        public float Cooldown => 5f;
    }
}