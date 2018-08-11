using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Plank.Net.Data
{
    public abstract class EntityConfiguration<T> : DbMigrationsConfiguration<T> where T : DbContext
    {
        public EntityConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new EntitySqlServerMigrationSqlGenerator());
        }
    }
}
