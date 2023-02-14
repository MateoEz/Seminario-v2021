namespace Domain.Services
{
    public interface IEnergyView
    {
        void UseEnergy(float amount);
        bool IsAffordable(float amount);
    }
}