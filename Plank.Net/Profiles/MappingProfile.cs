﻿using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using PagedList;
using Plank.Net.Contracts;
using Plank.Net.Data;
using System.Linq;

namespace Plank.Net.Profiles
{
    internal class MappingProfile<TEntity> : Profile where TEntity: IEntity
    {
        #region CONSTRUCTORS

        public MappingProfile()
        {
            CreateMap<ValidationResult, PlankValidationResult>();

            CreateMap<IPagedList<TEntity>, PagedListCache>()
                .ForMember(dest => dest.Ids, opt => opt.MapFrom(src => src.Select(i => i.Id)));

            CreateMap<IPagedList<TEntity>, PlankEnumerableResponse<TEntity>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }

        #endregion
    }
}
