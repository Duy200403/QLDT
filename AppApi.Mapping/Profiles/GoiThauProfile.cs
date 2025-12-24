using AppApi.DTO.Models.GoiThau;
using AppApi.Entities.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Mapping.Profiles
{
    public class GoiThauProfile : Profile
    {
        public GoiThauProfile()
        {
            CreateMap<GoiThauDeXuat, GoiThauResponse>();

            CreateMap<GoiThauRequest, GoiThauDeXuat>()
                .ForMember(d => d.Id, opt => opt.Ignore())           // Id set ở service/controller
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.UpdatedDate, opt => opt.Ignore())
                .ForMember(d => d.UpdatedBy, opt => opt.Ignore())
                .ForMember(d => d.IsDeleted, opt => opt.Ignore());
        }
    }
}
