using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plank.Net.Data
{
    public static class ValidatorFactory
    {
        #region MEMBERS

        public static List<Tuple<string, object>> _validators;

        #endregion

        #region CONSTRUCTORS

        static ValidatorFactory()
        {
            _validators = new List<Tuple<string, object>>();
            LoadValidators();
        }

        #endregion

        #region METHODS

        public static IEnumerable<IValidator> CreateInstance(Type type)
        {
            return _validators
                .Where(v => v.Item1 == type.Name)
                .Select(v => (IValidator)v.Item2);
        }

        public static IEnumerable<IValidator<T>> CreateInstance<T>() where T : Entity
        {
            return _validators
                .Where(v => v.Item1 == typeof(T).Name)
                .Select(v => (IValidator<T>)v.Item2);

        }

        #endregion

        #region PRIVATE METHODS

        private static void LoadValidators()
        {
            var asm   = AppDomain.CurrentDomain.GetAssemblies();
            var types = asm.SelectMany(a => a.GetTypes())
                            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterface("IValidator`1") != null)
                            .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var inter = type.GetInterface("IValidator`1");
                var entity = inter.GetTypeInfo().GenericTypeArguments[0];
                
                _validators.Add(new Tuple<string, object>(entity.Name, instance));
            }
        }

        #endregion
    }
}
