namespace Weapons
{
    public interface IMeleeWeapon
    {
        int Damage { set; get; }
        void EnableCollision();
        void DisableCollision();
    }
}