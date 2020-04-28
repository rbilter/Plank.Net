using Plank.Net.Data;
using Plank.Net.Tests.Models;
using System.Data.Entity;

namespace Plank.Net.Tests
{
    public class TestDbContext : PlankDbContext
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

        public DbSet<ChildOne> ChildOneEntity { get; set; }

        public DbSet<ChildTwo> ChildTwoEntity { get; set; }

        #endregion
    }
}
