using PagedList;
using System;
using System.Linq.Expressions;

namespace Plank.Net.Data
{
    public interface IRepository<T> where T : Entity
    {
        #region METHODS

        Guid Create(T entity);

        Guid Delete(Guid id);

        T Get(Guid id);

        IPagedList<T> Search(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);

        Guid Update(T entity);

        Guid Update(T entity, params Expression<Func<T, object>>[] properties);

        #endregion
    }
}
