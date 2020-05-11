namespace Plank.Net.Managers
{
    public interface IManager<T> : IReadManager<T>, IWriteManager<T>
    {
    }
}
