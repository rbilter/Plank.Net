using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public abstract class AbstractRepository<T> : IRepository<T>
    {
        #region PROPERTIES

        public IRepository<T> NextRepository { get; set; }

        #endregion

        #region METHODS

        public abstract Task AddAsync(T entity);

        public abstract Task DeleteAsync(int id);

        public abstract Task<T> GetAsync(int id);

        public IRepository<T> RegisterNext(IRepository<T> nextRepository)
        {
            NextRepository = nextRepository;
            return NextRepository;
        }

        public abstract Task<IPagedList<T>> SearchAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);

        public abstract Task UpdateAsync(T entity);

        #endregion
    }
}
