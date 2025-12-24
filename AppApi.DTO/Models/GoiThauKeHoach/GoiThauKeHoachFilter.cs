using AppApi.DTO.Common;

namespace AppApi.DTO.Models.GoiThauKeHoach;

public class GoiThauKeHoachFilter : PagingRequestBase
{
    public string? Keyword { get; set; }            // MaGoi/TenGoiThau
    public int? NamKeHoach { get; set; }            // lọc theo năm kế hoạch
    public string? TrangThaiKeHoach { get; set; }   // lọc theo trạng thái kế hoạch
    public Guid? DonViDeXuatChinhId { get; set; }  // lọc theo đơn vị đề xuất chính
    public Guid? DonViMuaSamId { get; set; }    // lọc theo đơn vị mua sắm
}