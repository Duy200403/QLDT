using AppApi.DTO.Models.Account;
using AppApi.Entities.Models;
using AutoMapper;

namespace AppApi.Mapping.Profiles
{
  public class AccountProfile : Profile
  {
    public AccountProfile()
    {
      CreateMap<Account, AccountResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
      // .ForMember(dest => dest.Diseases,
      //        opt => opt.MapFrom(src => src.AccountPatientInfo.Diseases))
      // .ForMember(dest => dest.IdPatientInfo,
                       //                  opt => opt.MapFrom(src => src.AccountPatientInfo.Id.ToString()))
                       // .ForMember(dest => dest.PatientNo,
                       //                  opt => opt.MapFrom(src => src.AccountPatientInfo.PatientNo))
                       //       .ForMember(dest => dest.DocNo,
                       //                  opt => opt.MapFrom(src => src.AccountPatientInfo.DocNo))
                       //       .ForMember(dest => dest.DataTypeSex,
                       //                  opt => opt.MapFrom(src => src.AccountPatientInfo.DataTypeSex))
                       //       .ForMember(dest => dest.ObjectName,
                       //                  opt => opt.MapFrom(src => src.AccountPatientInfo.ObjectName))
                       //       .ForMember(dest => dest.IdentityCardNo,
                       //                  opt => opt.MapFrom(src => src.AccountPatientInfo.IdentityCardNo))
                       //       .ForMember(dest => dest.Telephone,
                       //                  opt => opt.MapFrom(src => src.AccountPatientInfo.Telephone))
                       //       .ForMember(dest => dest.BirthDate,
                       //                  opt => opt.MapFrom(src => src.AccountPatientInfo.BirthDate))
                       ;

      // 2) Map request → AccountPatientInfo
      // CreateMap<CreateAccountPatientRequest, AccountPatientInfo>()
      //         .ForMember(dest => dest.PatientNo,      opt => opt.MapFrom(src => src.PatientNo))
      //         .ForMember(dest => dest.DocNo,          opt => opt.MapFrom(src => src.DocNo))
      //         .ForMember(dest => dest.BirthDate,      opt => opt.MapFrom(src => src.BirthDate))
      //         .ForMember(dest => dest.DataTypeSex,    opt => opt.MapFrom(src => src.DataTypeSex))
      //         .ForMember(dest => dest.ObjectName,     opt => opt.MapFrom(src => src.ObjectName))
      //         .ForMember(dest => dest.IdentityCardNo, opt => opt.MapFrom(src => src.IdentityCardNo))
      //         .ForMember(dest => dest.Telephone,      opt => opt.MapFrom(src => src.Telephone));

      // CreateMap<CreateAccountPatientRequest, AccountPatientInfo>();
      //   CreateMap<CreateAccountPatientRequest, Account>()
      //           .ForMember(dest => dest.AccountPatientInfo,
      //                      opt => opt.MapFrom(src => src));
    }
  }
}