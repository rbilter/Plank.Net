using AutoMapper;
using PagedList;
using Plank.Net.Contracts;
using Plank.Net.Data;
using Plank.Net.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Plank.Net.Profiles
{
    internal class MappingProfile<TEntity> : Profile where TEntity: IEntity
    {
        #region CONSTRUCTORS

        public MappingProfile()
        {
            CreateMap<PostResponse<TEntity>, PlankPostResponse<TEntity>>();

            CreateMap<DeleteResponse, PlankDeleteResponse>();

            CreateMap<GetResponse<TEntity>, PlankGetResponse<TEntity>>();

            CreateMap<PostEnumerableResponse<TEntity>, PlankEnumerableResponse<TEntity>>();

            CreateMap<IPagedList<TEntity>, PagedListCache>()
                .ForMember(dest => dest.Ids, opt => opt.MapFrom(src => src.Select(i => i.Id)));

            CreateMap<IPagedList<TEntity>, PostEnumerableResponse<TEntity>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }

        #endregion
    }
}
