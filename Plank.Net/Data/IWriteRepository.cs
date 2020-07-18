using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public interface IWriteRepository<T>
    {
        #region METHODS

        Task AddAsync(T entity);

        Task BulkAddAsync(IEnumerable<T> entities);

        Task DeleteAsync(int id);

        Task UpdateAsync(T entity);

        #endregion
    }
}
