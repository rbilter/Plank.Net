using Plank.Net.Data;
using Plank.Net.Tests.Models;
using System.Collections.Generic;
using System.Linq;

namespace Plank.Net.Tests
{
    public sealed class Configuration : EntityConfiguration<TestDbContext>
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

        private void LoadTestModel(TestDbContext context)
        {
            if(!context.ParentEntity.Any())
            {
                var parent1 = new ParentEntity
                {
                    FirstName = "Luke",
                    LastName  = "Skywalker",
                    ChildOne  = new List<ChildOne>
                    {
                        new ChildOne
                        {
                            Address = "Luke Skywalker Address",
                            City    = "Skywalker City"
                        }
                    }
                };

                var parent2 = new ParentEntity
                {
                    FirstName = "Han",
                    LastName  = "Solo",
                    ChildOne  = new List<ChildOne>
                    {
                        new ChildOne
                        {
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
