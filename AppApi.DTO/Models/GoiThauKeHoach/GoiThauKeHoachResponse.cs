namespace AppApi.DTO.Models.GoiThauKeHoach;

public class GoiThauKeHoachResponse
{
    public Guid Id { get; set; }
    public string? MaGoi { get; set; }
    public string? TenGoiThau { get; set; }
    public int NamKeHoach { get; set; }

    public Guid DonViDeXuatChinhId { get; set; }
    public Guid DonViMuaSamId { get; set; }

    public string? LoaiMuaSam { get; set; }
    public string? LinhVuc { get; set; }
    public string? HinhThucLCNTDuKien { get; set; }
    public string? LoaiQuyTrinhDuKien { get; set; }

    public decimal? GiaTriDuKien { get; set; }
    public string? TrangThaiKeHoach { get; set; }
}
