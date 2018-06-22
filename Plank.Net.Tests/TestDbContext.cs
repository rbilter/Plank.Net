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

        public DbSet<ChildEntity> ChildEntity { get; set; }

        #endregion
    }
}
