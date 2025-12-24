using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DTO.Models.NhaThau
{
    public class NhaThauRequest
    {
        public NhaThauRequest() { }
        public string TenNhaThau { get; set; } = null!;
        public string? MaSoThue { get; set; }
        public string? DiaChi { get; set; }
        public string? DaiDien { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
    }

    public class NhaThauValidator : AbstractValidator<NhaThauRequest>
    {
        public NhaThauValidator()
        {
            RuleFor(x => x.TenNhaThau)
                .NotEmpty().WithMessage("Tên nhà thầu không được để trống")
                .MaximumLength(255).WithMessage("Tên nhà thầu không được quá 255 ký tự");

            RuleFor(x => x.MaSoThue)
                .MaximumLength(50).WithMessage("Mã số thuế không được quá 50 ký tự")
                .When(x => !string.IsNullOrWhiteSpace(x.MaSoThue));

            RuleFor(x => x.SoDienThoai)
                .MaximumLength(50).WithMessage("Số điện thoại không được quá 50 ký tự")
                .When(x => !string.IsNullOrWhiteSpace(x.SoDienThoai));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email không đúng định dạng")
                .MaximumLength(255).WithMessage("Email không được quá 255 ký tự")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }
}
