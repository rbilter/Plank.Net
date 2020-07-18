using Plank.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Plank.Net.Search
{
    public interface ISearchBuilder<TEntity> where TEntity : class, IEntity
    {
        #region PROPERTIES

        List<Expression<Func<TEntity, object>>> Includes { get; }

        int PageNumber { get; }

        int PageSize { get; }

        #endregion

        #region METHODS

        Expression<Func<TEntity, bool>> Build();

        #endregion
    }
}
