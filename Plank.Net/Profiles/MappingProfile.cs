using AutoMapper;
using PagedList;
using Plank.Net.Contracts;
using Plank.Net.Managers;
using System.Linq;

namespace Plank.Net.Profiles
{
    public class MappingProfile<T> : Profile
    {
        #region CONSTRUCTORS

        public MappingProfile()
        {
            CreateMap<PostResponse, ApiPostResponse>();

            CreateMap<GetResponse<T>, ApiGetResponse<T>>();

            CreateMap<PostEnumerableResponse<T>, ApiEnumerableResponse<T>>()
                .ConstructUsing(src => new ApiEnumerableResponse<T>(src.ToList()));

            CreateMap<IPagedList<T>, PostEnumerableResponse<T>>()
                .ConstructUsing(src => new PostEnumerableResponse<T>(src.ToList()));
        }

        #endregion
    }
}
