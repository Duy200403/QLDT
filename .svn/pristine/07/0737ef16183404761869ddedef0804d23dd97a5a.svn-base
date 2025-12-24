using AppApi.DTO.Models.Logger;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class LogProfile : Profile
  {
    public LogProfile()
    {
      //     // map entities to model
      //     CreateMap<Contact, ContactModel>()
      //     .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()));

      CreateMap<Log, LogResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
  }
}