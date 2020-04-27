using Plank.Net.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public abstract class AbstractDbContext : DbContext
    {
        #region CONSTRUCTORS

        protected AbstractDbContext(string connectionString)
            : base(connectionString)
        {

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
                    if (a.Entity is IPopulateTimeStamps timeStamps)
                    {
                        timeStamps.PopulateTimeStamps();
                    }
                });

            return base.SaveChangesAsync();
        }

        #endregion
    }
}
