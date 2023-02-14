using Domain.Services;
using UnityEngine;

public class CastSpell
{
    private readonly SpellCooldownManager _spellCooldownManager;
    private readonly IEnergyView _energy;
    private float spellEnergyCost;

    public CastSpell(SpellCooldownManager spellCooldownManager, IEnergyView energy)
    {
        _spellCooldownManager = spellCooldownManager;
        _energy = energy;
    }
    public void Invoke(ISpell spell)
    {
        if (!_spellCooldownManager.IsSpellOnCooldown(spell))
        {
            Debug.LogWarning("Se quiso invocar el spell pero el mismo estaba en cooldown");
            return;
        }
        if (spell is IPlayerSpell)
        {
            var playerSpell = spell as IPlayerSpell;
            spellEnergyCost = playerSpell.EnergyCost;
            if (!_energy.IsAffordable(spellEnergyCost)) return;
            _energy.UseEnergy(spellEnergyCost);
        }
        
        _spellCooldownManager.UseSpell(spell);
        spell.Cast();
    }
}