using AutoMapper;
using PagedList;
using Plank.Net.Contracts;
using Plank.Net.Managers;

namespace Plank.Net.Profiles
{
    public class MappingProfile<TEntity> : Profile
    {
        #region CONSTRUCTORS

        public MappingProfile()
        {
            CreateMap<PostResponse, ApiPostResponse>();

            CreateMap<GetResponse<TEntity>, ApiGetResponse<TEntity>>();

            CreateMap<PostEnumerableResponse<TEntity>, ApiEnumerableResponse<TEntity>>();

            CreateMap<IPagedList<TEntity>, PostEnumerableResponse<TEntity>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }

        #endregion
    }
}
