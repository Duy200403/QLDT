using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DTO.Models.GoiThauKeHoach
{
    public class GoiThauKeHoachRequest
    {
        public GoiThauKeHoachRequest() { }
        public string? MaGoi { get; set; }
        public string TenGoiThau { get; set; } = null!;
        public int? NamKeHoach { get; set; }

        public Guid DonViDeXuatChinhId { get; set; }
        public Guid DonViMuaSamId { get; set; }

        public string? LoaiMuaSam { get; set; }
        public string? LinhVuc { get; set; }
        public string? HinhThucLCNTDuKien { get; set; }
        public string? LoaiQuyTrinhDuKien { get; set; }

        public decimal? GiaTriDuKien { get; set; }
        public string? TrangThaiKeHoach { get; set; }
    }
    public class GoiThauKeHoachValidator : AbstractValidator<GoiThauKeHoachRequest>
    {
        public GoiThauKeHoachValidator()
        {
            RuleFor(x => x.TenGoiThau).NotEmpty().WithMessage("Tên gói thầu không được để trống");
            RuleFor(x => x.NamKeHoach).GreaterThan(2000).WithMessage("Năm kế hoạch không hợp lệ");
            RuleFor(x => x.DonViDeXuatChinhId).NotEmpty().WithMessage("Đơn vị đề xuất chính không được để trống");
            RuleFor(x => x.DonViMuaSamId).NotEmpty().WithMessage("Đơn vị mua sắm không được để trống");
            RuleFor(x => x.GiaTriDuKien).GreaterThanOrEqualTo(0).WithMessage("Giá trị dự kiến không hợp lệ");
            RuleFor(x => x.TrangThaiKeHoach).NotEmpty().WithMessage("Trạng thái kế hoạch không được để trống");
        }
    }
}
