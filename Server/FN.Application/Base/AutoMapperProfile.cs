using AutoMapper;
using FN.DataAccess.Entities;
using FN.DataAccess.Enums;
using FN.ViewModel.Catalog.Categories;
using FN.ViewModel.Catalog.Products;
using FN.ViewModel.Systems.User;

namespace FN.Application.Base
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserViewModel>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
