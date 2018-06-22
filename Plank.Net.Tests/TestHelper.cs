using Microsoft.Practices.EnterpriseLibrary.Validation;
using Moq;
using Plank.Net.Managers;
using System;
using System.Linq;

namespace Plank.Net.Tests
{
    internal static class TestHelper
    {
        #region MEMBERS

        private static Random random = new Random();

        #endregion

        #region METHODS

        public static Mock<IValidator<ParentEntity>> GetFailValidator()
        {
            var validator = new Mock<IValidator<ParentEntity>>();
            var fail      = new ValidationResults();
            fail.AddResult(new ValidationResult("There was a problem", null, null, null, null));
            validator.Setup(m => m.Validate(It.IsAny<ParentEntity>())).Returns(fail);

            return validator;
        }

        public static Mock<IValidator<ParentEntity>> GetPassValidator()
        {
                var validator     = new Mock<IValidator<ParentEntity>>();
                var pass          = new ValidationResults();
                validator.Setup(m => m.Validate(It.IsAny<ParentEntity>())).Returns(pass);

                return validator;
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

        public static ChildEntity GetChildEntity()
        {
            return new ChildEntity()
            {
                Address = GetRandomString(30),
                City    = GetRandomString(20)
            };
        }

        #endregion
    }
}
