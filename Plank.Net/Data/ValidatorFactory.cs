using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plank.Net.Data
{
    public static class ValidatorFactory
    {
        #region MEMBERS

        private static readonly List<Tuple<string, object>> _validators = new List<Tuple<string, object>>();

        #endregion

        #region CONSTRUCTORS

        static ValidatorFactory()
        {
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

        public static IEnumerable<IEntityValidator<TEntity>> CreateInstance<TEntity>() where TEntity : IEntity
        {
            return _validators
                .Where(v => v.Item1 == typeof(TEntity).Name)
                .Select(v => (IEntityValidator<TEntity>)v.Item2)
                .OrderBy(v => v.Priority);

        }

        #endregion

        #region PRIVATE METHODS

        private static void LoadValidators()
        {
            var filter = new List<string> { "Microsoft", "System" };
            var asm   = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && !filter.Any(f => a.FullName.Contains(f)));
            var types = asm.SelectMany(a => a.GetExportedTypes())
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
