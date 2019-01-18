﻿namespace Plank.Net.Contracts
{
    public class PlankPostResponse<TEntity>
    {
        #region CONSTRUCTORS

        public PlankPostResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public TEntity Item { get; set; }

        public PlankValidationResults ValidationResults { get; set; }

        #endregion
    }
}
