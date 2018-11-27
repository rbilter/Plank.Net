using Plank.Net.Data;
using System;
using System.Linq.Expressions;

namespace Plank.Net.Search
{
    public interface ISearchBuilder<TEntity> where TEntity : class, IEntity
    {
        #region PROPERTIES

        int PageNumber { get; }

        int PageSize { get; }

        #endregion

        #region METHODS

        Expression<Func<TEntity, bool>> Build();

        #endregion
    }
}
