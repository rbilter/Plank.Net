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

        public static IEnumerable<IEntityValidator> CreateInstance(Type type)
        {
            return _validators
                .Where(v => v.Item1 == type.Name)
                .Select(v => (IEntityValidator)v.Item2)
                .OrderBy(v => v.Priority);
        }

        public static IEnumerable<IEntityValidator<T>> CreateInstance<T>() where T : Entity
        {
            return _validators
                .Where(v => v.Item1 == typeof(T).Name)
                .Select(v => (IEntityValidator<T>)v.Item2)
                .OrderBy(v => v.Priority);

        }

        #endregion

        #region PRIVATE METHODS

        private static void LoadValidators()
        {
            var asm   = AppDomain.CurrentDomain.GetAssemblies();
            var types = asm.SelectMany(a => a.GetTypes())
                            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterface("IEntityValidator`1") != null)
                            .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var inter = type.GetInterface("IEntityValidator`1");
                var entity = inter.GetTypeInfo().GenericTypeArguments[0];
                
                _validators.Add(new Tuple<string, object>(entity.Name, instance));
            }
        }

        #endregion
    }
}
