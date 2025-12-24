using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DTO.Models.BuocQuyTrinh
{
    public class BuocQuyTrinhRequest
    {
        public BuocQuyTrinhRequest() { }
        public string MaBuoc { get; set; } = null!;
        public string TenBuoc { get; set; } = null!;
        public int ThuTu { get; set; }
        public string? GhiChu { get; set; }
        public string TrangThai { get; set; } = null!; // ví dụ: "Đang áp dụng" / "Ngừng áp dụng"
    }

    public class BuocQuyTrinhValidator : AbstractValidator<BuocQuyTrinhRequest>
    {
        public BuocQuyTrinhValidator()
        {
            RuleFor(x => x.MaBuoc)
                .NotEmpty().WithMessage("Mã bước không được để trống")
                .MaximumLength(20).WithMessage("Mã bước không được quá 20 ký tự");

            RuleFor(x => x.TenBuoc)
                .NotEmpty().WithMessage("Tên bước không được để trống")
                .MaximumLength(255).WithMessage("Tên bước không được quá 255 ký tự");

            RuleFor(x => x.ThuTu)
                .GreaterThanOrEqualTo(1).WithMessage("Thứ tự phải >= 1");

            RuleFor(x => x.TrangThai)
                .NotEmpty().WithMessage("Trạng thái không được để trống")
                .MaximumLength(20).WithMessage("Trạng thái không được quá 20 ký tự");
        }
    }
}
