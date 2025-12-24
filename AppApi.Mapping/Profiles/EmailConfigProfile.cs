using AppApi.DTO.Models.EmailConfig;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class EmailConfigProfile : Profile
  {
    public EmailConfigProfile()
    {
      //     // map entities to model
      //     CreateMap<Contact, ContactModel>()
      //     .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()));

      //     // map model request to entities
      CreateMap<EmailConfigRequest, EmailConfig>();
      CreateMap<EmailConfig, EmailConfigResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
  }
}