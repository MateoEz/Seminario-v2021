public interface ISpell
{
    void Cast();
    float Cooldown { get;}
}

public interface IPlayerSpell : ISpell
{
    float EnergyCost { get; }
}