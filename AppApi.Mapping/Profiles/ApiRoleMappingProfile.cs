using AppApi.DTO.Models.ApiRoleMapping;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class ApiRoleMappingProfileProfile : Profile
  {
    public ApiRoleMappingProfileProfile()
    {
      //     // map entities to model
      //     CreateMap<Contact, ContactModel>()
      //     .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()));

      //     // map model request to entities
      CreateMap<ApiRoleMappingRequest, ApiRoleMapping>();
      CreateMap<ApiRoleMapping, ApiRoleMappingResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
  }
}