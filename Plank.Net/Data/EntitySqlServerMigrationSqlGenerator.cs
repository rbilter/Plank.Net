using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;

namespace Plank.Net.Data
{
    public sealed class EntitySqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        #region METHODS

        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetEntityDefaultValue(addColumnOperation.Column);
            base.Generate(addColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetEntityDefaultValue(createTableOperation.Columns);
            base.Generate(createTableOperation);
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            SetEntityDefaultValue(alterColumnOperation.Column);
            base.Generate(alterColumnOperation);
        }

        #endregion

        #region PRIVATE METHODS

        private static void SetEntityDefaultValue(IEnumerable<ColumnModel> columns)
        {
            foreach (var columnModel in columns)
            {
                SetEntityDefaultValue(columnModel);
            }
        }

        private static void SetEntityDefaultValue(PropertyModel column)
        {
            if (column.Name == "DateCreated" || column.Name == "DateLastModified")
            {
                column.DefaultValueSql = "GETUTCDATE()";
            }

            if (column.Name == "Id")
            {
                column.DefaultValueSql = "NEWSEQUENTIALID()";
            }
        }

        #endregion
    }
}
