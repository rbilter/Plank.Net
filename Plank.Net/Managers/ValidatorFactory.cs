using Plank.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plank.Net.Managers
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
            var asm = Assembly.GetExecutingAssembly();
            var types = from type in asm.GetTypes()
                        where
                            type.IsClass
                            && type.IsAbstract == false
                            && type.GetInterface("IValidator`1") != null
                        select type;

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
