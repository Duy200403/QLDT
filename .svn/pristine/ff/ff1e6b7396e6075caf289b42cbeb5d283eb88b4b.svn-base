using AppApi.DTO.Models.ApiRoleMapping;
using AppApi.DTO.Models.MenuItemDto;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class MenuItemProfile : Profile
  {
    public MenuItemProfile()
    {
      //     // map entities to model
      //     CreateMap<Contact, ContactModel>()
      //     .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()));

      //     // map model request to entities
      CreateMap<MenuItemRequest, MenuItem>().ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => Guid.Parse(src.ParentId)));
      CreateMap<MenuItem, MenuItemResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
  }
}