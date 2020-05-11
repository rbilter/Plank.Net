using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public interface IWriteRepository<T>
    {
        #region METHODS

        Task AddAsync(T entity);

        Task DeleteAsync(int id);

        Task UpdateAsync(T entity);

        #endregion
    }
}
