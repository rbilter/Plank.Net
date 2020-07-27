using Plank.Net.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public abstract class PlankDbContext : DbContext
    {
        #region CONSTRUCTORS

        protected PlankDbContext(string connectionString)
            : base(connectionString)
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        #endregion

        #region METHODS

        public override Task<int> SaveChangesAsync()
        {
            ChangeTracker.Entries()
                .Where(a => (a.State == EntityState.Added || a.State == EntityState.Modified))
                .ToList()
                .ForEach(a =>
                {
                    if (a.Entity is IPopulateComputedColumns timeStamps)
                    {
                        timeStamps.PopulateComputedColumns();
                    }
                });

            return base.SaveChangesAsync();
        }

        #endregion
    }
}
