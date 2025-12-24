using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DTO.Models.NhaThau
{
    public class NhaThauResponse
    {
        public Guid Id { get; set; }
        public string TenNhaThau { get; set; } = null!;
        public string? MaSoThue { get; set; }
        public string? DiaChi { get; set; }
        public string? DaiDien { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
    }
}
