namespace Plank.Net.Data
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    {
        #region PROPERTIES

        IRepository<T> NextRepository { get; }

        #endregion

        #region METHODS

        IRepository<T> RegisterNext(IRepository<T> nextRepository);

        #endregion
    }
}
