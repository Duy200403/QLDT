using AppApi.DTO.Models.GoiThauKeHoach;
using AppApi.Entities.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Mapping.Profiles
{
    public class GoiThauKeHoachProfile : Profile
    {
        public GoiThauKeHoachProfile()
        {
            CreateMap<GoiThauKeHoachRequest, GoiThauKeHoach>();
            CreateMap<GoiThauKeHoach, GoiThauKeHoachResponse>();
        }
    }
}
