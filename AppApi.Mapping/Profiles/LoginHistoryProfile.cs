using AppApi.DTO.Models.LoginHistory;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class LoginHistoryProfile : Profile
  {
    public LoginHistoryProfile()
    {
      //     // map entities to model
      //     CreateMap<Contact, ContactModel>()
      //     .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()));


       CreateMap<LoginHistory, LoginHistoryResponse>();
    }
  }
}