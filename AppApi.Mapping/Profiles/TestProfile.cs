using AppApi.DTO.Models.Test;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class TestProfile : Profile
  {
    public TestProfile()
    {
      //     // map entities to model
      //     CreateMap<Contact, ContactModel>()
      //     .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()));

      //     // map model request to entities
      CreateMap<TestRequest, Test>();
      CreateMap<Test, TestResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
  }
}