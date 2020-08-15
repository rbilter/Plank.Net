using Plank.Net.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        #endregion

        #region METHODS

        public async Task DetachAllEntitiesAsync()
        {
            var entries = await ChangeTracker
                .Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (var entry in entries)
            {
                if (entry.Entity != null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }

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
