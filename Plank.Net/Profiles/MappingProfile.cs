using AutoMapper;
using PagedList;
using Plank.Net.Contracts;
using Plank.Net.Managers;

namespace Plank.Net.Profiles
{
    public class MappingProfile<T> : Profile
    {
        #region CONSTRUCTORS

        public MappingProfile()
        {
            CreateMap<PostResponse, ApiPostResponse>();

            CreateMap<GetResponse<T>, ApiGetResponse<T>>();

            CreateMap<PostEnumerableResponse<T>, ApiEnumerableResponse<T>>();

            CreateMap<IPagedList<T>, PostEnumerableResponse<T>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }

        #endregion
    }
}
