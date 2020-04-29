using Plank.Net.Tests.Models;
using System;
using System.Linq;

namespace Plank.Net.Tests
{
    public static class TestHelper
    {
        #region MEMBERS

        private static readonly Random random = new Random();

        #endregion

        #region METHODS

        public static int GetParentId()
        {
            using (var context = new TestDbContext())
            {
                return context.ParentEntity.First().Id;
            }
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static ParentEntity GetParentEntity()
        {
            return new ParentEntity()
            {
                FirstName = GetRandomString(10),
                LastName  = GetRandomString(20)
            };
        }

        public static ChildOne GetChildOne()
        {
            return new ChildOne()
            {
                Address = GetRandomString(30),
                City    = GetRandomString(20)
            };
        }

        public static ChildTwo GetChildTwo()
        {
            return new ChildTwo()
            {
            };
        }

        #endregion
    }
}
