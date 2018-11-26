using AutoMapper;
using PagedList;
using Plank.Net.Contracts;
using Plank.Net.Managers;

namespace Plank.Net.Profiles
{
    internal class MappingProfile<TEntity> : Profile
    {
        #region CONSTRUCTORS

        public MappingProfile()
        {
            CreateMap<PostResponse, PlankPostResponse>();

            CreateMap<GetResponse<TEntity>, PlankGetResponse<TEntity>>();

            CreateMap<PostEnumerableResponse<TEntity>, PlankEnumerableResponse<TEntity>>();

            CreateMap<IPagedList<TEntity>, PostEnumerableResponse<TEntity>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }

        #endregion
    }
}
