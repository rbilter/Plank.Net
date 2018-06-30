using AutoMapper;
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
            CreateMap<PostEnumerationResponse<T>, ApiEnumerationResponse<T>>();
        }

        #endregion
    }
}
