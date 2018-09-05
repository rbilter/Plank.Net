using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;

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

            if(createTableOperation.Columns.Any(c => c.Name == "Id"))
            {
                Statement(CreateTriggerStatement(createTableOperation.Name));
            }
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            SetEntityDefaultValue(alterColumnOperation.Column);
            base.Generate(alterColumnOperation);
        }

        #endregion

        #region PRIVATE METHODS

        private static string CreateTriggerStatement(string tableName)
        {
            tableName = tableName.StartsWith("dbo.") ? tableName : $"dbo.{tableName}";

            return $"create trigger upd_trg_{tableName.Replace("dbo.", string.Empty).ToLower()} on {tableName} for update as "
                + "begin "
                + $"set nocount on; "
                + $"update {tableName} set DateLastModified = GETUTCDATE() "
                + $"from {tableName} tbl join "
                + $"inserted ins on tbl.Id = ins.Id;"
                + "end";
        }

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

            if (column.Name == "GlobalId")
            {
                column.DefaultValueSql = "NEWSEQUENTIALID()";
            }
        }

        #endregion
    }
}
