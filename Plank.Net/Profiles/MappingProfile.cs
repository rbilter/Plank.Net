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
            CreateMap<PostResponse, PlankPostResponse>();
            CreateMap<GetResponse<T>, PlankGetResponse<T>>();
            CreateMap<PostEnumerationResponse<T>, PlankEnumerationResponse<T>>();
        }

        #endregion
    }
}
