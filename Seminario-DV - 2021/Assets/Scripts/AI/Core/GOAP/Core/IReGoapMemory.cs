namespace AI.Core.GOAP.Core
{
    public interface IReGoapMemory<T, W>
    {
        ReGoapState<T, W> GetWorldState();
    }
}