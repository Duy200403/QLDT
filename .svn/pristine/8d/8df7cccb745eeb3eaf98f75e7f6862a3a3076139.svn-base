using AppApi.DTO.Models.FileManager;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class FileManagerProfile : Profile
  {
    public FileManagerProfile()
    {
      CreateMap<FileManager, FileManagerResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
    }
  }
}