using Plank.Net.Data;
using System;
using System.Linq.Expressions;

namespace Plank.Net.Search
{
    public interface ISearchBuilder<TEntity> where TEntity : class, IEntity
    {
        #region PROPERTIES

        int PageNumber { get; set; }

        int PageSize { get; set; }

        Expression<Func<TEntity, bool>> SearchExpression { get; set; }

        #endregion

        #region METHODS

        void Build();

        #endregion
    }
}
