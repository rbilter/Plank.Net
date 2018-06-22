using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Plank.Net.Tests
{
    internal sealed class Configuration : DbMigrationsConfiguration<TestDbContext>
    {
        #region  CONSTRUCTORS

        public Configuration()
        {
            AutomaticMigrationsEnabled        = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        #endregion

        #region METHODS

        protected override void Seed(TestDbContext context)
        {
            LoadTestModel(context);

            base.Seed(context);
        }

        #endregion

        #region PRIVATE METHODS

        private List<Guid> GetGuids()
        {
            return new List<Guid>()
            {
                Guid.Parse("8BDE13A5-DB5B-46FC-8437-0E914EBED531"),
                Guid.Parse("CAE47288-FBBD-463A-B2D5-436A04A6094D")
            };
        }

        private void LoadTestModel(TestDbContext context)
        {
            var ids = GetGuids();
            if(context.ParentEntity.FirstOrDefault(m => ids.Contains(m.Id)) == null)
            {
                var models = new[]
                {
                    new ParentEntity
                    {
                        Id        = ids[0],
                        FirstName = "Luke",
                        LastName  = "Skywalker"
                    },
                    new ParentEntity
                    {
                        Id        = ids[1],
                        FirstName = "Han",
                        LastName  = "Solo"
                    }
                };

                context.ParentEntity.AddRange(models);
                context.SaveChanges();
            }
        }

        #endregion
    }
}
