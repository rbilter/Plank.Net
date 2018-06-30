using Plank.Net.Tests.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Plank.Net.Tests
{
    public sealed class Configuration : DbMigrationsConfiguration<TestDbContext>
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
            base.Seed(context);
            LoadTestModel(context);
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
                var parent1 = new ParentEntity
                {
                    Id        = ids[0],
                    FirstName = "Luke",
                    LastName  = "Skywalker",
                    ChildOne  = new List<ChildOne>
                    {
                        new ChildOne
                        {
                            Id      = ids[0],
                            Address = "Luke Skywalker Address",
                            City    = "Skywalker City"
                        }
                    }
                };

                var parent2 = new ParentEntity
                {
                    Id        = ids[1],
                    FirstName = "Han",
                    LastName  = "Solo",
                    ChildOne  = new List<ChildOne>
                    {
                        new ChildOne
                        {
                            Id      = ids[1],
                            Address = "Han Solo Address",
                            City    = "Solo City"
                        }
                    }
                };

                context.ParentEntity.AddRange(new[] { parent1, parent2 });
                context.SaveChanges();
            }
        }

        #endregion
    }
}
