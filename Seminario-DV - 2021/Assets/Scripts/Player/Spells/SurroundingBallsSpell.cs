using System;
using Domain;
using UnityEngine;

public class SurroundingBallsSpell : IPlayerSpell
{
    private GameObject _spellPrefab = Resources.Load<GameObject>("Spells/SurroundingBalls");
    private Action<GameObject> _instantiateAction;
    private GameConfig _config;
    public float EnergyCost => _config.Instance.SurroundingBallsEnergyCost;
    public float Cooldown => _config.Instance.SurroundingBallsCooldown;

    public void Init(GameConfig config, Action<GameObject> instantiateAction)
    {
        _config = config;
        _instantiateAction = instantiateAction;
    }
    
    public void Cast()
    {
        if(!_spellPrefab) Debug.LogError("Estas castiando el spell sin hacerle .Init() antes...");
        _instantiateAction(_spellPrefab);
    }

}