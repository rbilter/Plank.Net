using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Plank.Net.Data
{
    public abstract class PlankConfiguration<T> : DbMigrationsConfiguration<T> where T : DbContext
    {
        protected PlankConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new PlankSqlServerMigrationSqlGenerator());
        }
    }
}
