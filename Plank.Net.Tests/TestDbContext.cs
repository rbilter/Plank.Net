using Plank.Net.Tests.Models;
using System.Data.Entity;

namespace Plank.Net.Tests
{
    internal class TestDbContext : DbContext
    {
        #region CONSTRUCTORS

        public TestDbContext() 
            : base("Plank.Net.Database")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TestDbContext, Configuration>());
        }

        #endregion

        #region PROPERTIES

        public DbSet<ParentEntity> ParentEntity { get; set; }

        public DbSet<ChildOne> ChildEntity { get; set; }

        #endregion
    }
}
