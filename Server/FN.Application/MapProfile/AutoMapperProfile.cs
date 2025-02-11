using AutoMapper;
using FN.DataAccess.Entities;
using FN.ViewModel.Systems.User;

namespace FN.Application.MapProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserViewModel>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
