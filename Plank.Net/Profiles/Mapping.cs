using AutoMapper;
using Plank.Net.Models;
using System;

namespace Plank.Net.Profiles
{
    internal static class Mapping<TEntity> where TEntity : IEntity
    {
        #region PROPERTIES

        public static IMapper Mapper => Lazy.Value;

        #endregion

        #region PRIVATE METHODS

        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile<TEntity>>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        #endregion
    }
}
