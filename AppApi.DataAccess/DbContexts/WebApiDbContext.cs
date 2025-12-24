
using AppApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace AppApi.DataAccess.Base
{
    /// <summary>
    /// DbContext for the Web API service, managing EmailConfig, FileManager, Logs, and LoginHistory.
    /// </summary>
    public class WebApiDbContext : ApplicationDbContext
    {
        public virtual DbSet<EmailConfig> EmailConfigs { get; set; }
        public virtual DbSet<FileManager> FileManagers { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<AccountGroupRole> AccountGroupRoles { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<DonVi> DonVis { get; set; } = null!;
        public virtual DbSet<DeXuatMuaSam> DeXuatMuaSams { get; set; } = null!;
        public virtual DbSet<DeXuatChiTiet> DeXuatChiTiets { get; set; } = null!;
        public virtual DbSet<GoiThauKeHoach> GoiThauKeHoaches { get; set; } = null!;
        public virtual DbSet<GoiThauDeXuat> GoiThauDeXuats { get; set; } = null!;
        public virtual DbSet<HoSoThau> HoSoThaus { get; set; } = null!;
        public virtual DbSet<BuocQuyTrinh> BuocQuyTrinhs { get; set; } = null!;
        public virtual DbSet<TienTrinhHoSo> TienTrinhHoSos { get; set; } = null!;
        public virtual DbSet<FileDinhKem> FileDinhKems { get; set; } = null!;
        public virtual DbSet<ThongTinKyThuat> ThongTinKyThuats { get; set; } = null!;
        public virtual DbSet<ThongTinKyThuatChiTiet> ThongTinKyThuatChiTiets { get; set; } = null!;
        public virtual DbSet<DuToanGoiThau> DuToanGoiThaus { get; set; } = null!;
        public virtual DbSet<DuToanChiTiet> DuToanChiTiets { get; set; } = null!;
        public virtual DbSet<NhaThau> NhaThaus { get; set; } = null!;
        public virtual DbSet<HoSoDuThau> HoSoDuThaus { get; set; } = null!;
        public virtual DbSet<ThuMoiBaoGia> ThuMoiBaoGias { get; set; } = null!;
        public virtual DbSet<ThuMoiBaoGiaChiTiet> ThuMoiBaoGiaChiTiets { get; set; } = null!;
        public virtual DbSet<ThuMoiThau> ThuMoiThaus { get; set; } = null!;
        public virtual DbSet<ThuMoiThauChiTiet> ThuMoiThauChiTiets { get; set; } = null!;
        public virtual DbSet<ToChuyenGia> ToChuyenGias { get; set; } = null!;
        public virtual DbSet<ThanhVienToChuyenGia> ThanhVienToChuyenGias { get; set; } = null!;
        public virtual DbSet<ThamDinh> ThamDinhs { get; set; } = null!;
        public virtual DbSet<HopDong> HopDongs { get; set; } = null!;
        public virtual DbSet<ThanhToan> ThanhToans { get; set; } = null!;
        public virtual DbSet<NghiemThu> NghiemThus { get; set; } = null!;
        public virtual DbSet<DanhGiaNhaThau> DanhGiaNhaThaus { get; set; } = null!;
        public virtual DbSet<TraoDoi> TraoDois { get; set; } = null!;
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // concurrent write 
            modelBuilder.Entity<FileManager>().Property(c => c.Timestamp).IsRowVersion();
            modelBuilder.Entity<EmailConfig>().Property(c => c.Timestamp).IsRowVersion();
            modelBuilder.Entity<Test>().Property(c => c.Timestamp).IsRowVersion();
            // ========= QLĐT: cấu hình quan hệ tránh multiple cascade paths =========

            // DonViCha - DonViCon: KHÔNG cascade delete
            modelBuilder.Entity<DonVi>()
                .HasOne(d => d.DonViCha)
                .WithMany(d => d.DonViCon)
                .HasForeignKey(d => d.DonViChaId)
                .OnDelete(DeleteBehavior.NoAction);

            // DonVi (Đề xuất chính) - GoiThauKeHoach
            modelBuilder.Entity<GoiThauKeHoach>()
                .HasOne(g => g.DonViDeXuatChinh)
                .WithMany(d => d.GoiThauKeHoach_DeXuatChinh)
                .HasForeignKey(g => g.DonViDeXuatChinhId)
                .OnDelete(DeleteBehavior.NoAction);

            // DonVi (Mua sắm) - GoiThauKeHoach
            modelBuilder.Entity<GoiThauKeHoach>()
                .HasOne(g => g.DonViMuaSam)
                .WithMany(d => d.GoiThauKeHoach_MuaSam)
                .HasForeignKey(g => g.DonViMuaSamId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<HoSoThau>()
                .HasOne(x => x.DonViDeXuat)
                .WithMany()
                .HasForeignKey(x => x.DonViDeXuatId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<HoSoThau>()
                .HasOne(x => x.DonViMuaSam)
                .WithMany()
                .HasForeignKey(x => x.DonViMuaSamId)
                .OnDelete(DeleteBehavior.NoAction);
            // Nếu sau này gặp lỗi multiple cascade paths ở chỗ khác
            // thì ta cũng làm tương tự: cấu hình .OnDelete(DeleteBehavior.NoAction)
        }
    }
}

