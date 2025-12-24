using AppApi.DTO.Models.RoleDto;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class RoleProfile : Profile
  {
    public RoleProfile()
    {
      //     // map entities to model
      //     CreateMap<Contact, ContactModel>()
      //     .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()));

      //     // map model request to entities
      CreateMap<RoleRequest, Role>().ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => Guid.Parse(src.ParentId)));
      CreateMap<Role, RoleResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

      CreateMap<RoleTreeRequest, Role>().ForMember(x => x.ParentId, opt => opt.MapFrom(src => parseGuid(src.ParentId)));
    }

    Guid parseGuid(string parentId)
    {
      return parentId != "" ? Guid.Parse(parentId) : Guid.Empty;
    }
  }
}