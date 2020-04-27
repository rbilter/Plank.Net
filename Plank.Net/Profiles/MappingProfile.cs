using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Contracts;
using Plank.Net.Models;
using X.PagedList;

namespace Plank.Net.Profiles
{
    internal class MappingProfile<TEntity> : Profile where TEntity: IEntity
    {
        #region CONSTRUCTORS

        public MappingProfile()
        {
            CreateMap<ValidationResult, PlankValidationResult>();

            CreateMap<IPagedList<TEntity>, PlankEnumerableResponse<TEntity>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }

        #endregion
    }
}
