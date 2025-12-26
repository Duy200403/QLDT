
using AppApi.Entities.Models;
using AppApi.Entities.Models.Base;
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
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IAuditEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "p");
                    var deletedProperty = System.Linq.Expressions.Expression.Property(parameter, nameof(IAuditEntity.IsDeleted));
                    var condition = System.Linq.Expressions.Expression.Equal(deletedProperty, System.Linq.Expressions.Expression.Constant(false));
                    var lambda = System.Linq.Expressions.Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
                // concurrent write 
                modelBuilder.Entity<FileManager>().Property(c => c.Timestamp).IsRowVersion();
            modelBuilder.Entity<EmailConfig>().Property(c => c.Timestamp).IsRowVersion();
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
                .OnDelete(DeleteBehavior.NoAction);
            // CẤU HÌNH MỚI: Định dạng tiền tệ chính xác (18 số, 4 số thập phân)
            // Áp dụng cho HopDong
            modelBuilder.Entity<HopDong>()
                .Property(h => h.GiaTriHopDong)
                .HasColumnType("decimal(18, 4)");

            // Áp dụng tương tự cho các bảng khác có tiền: DuToanGoiThau, ThanhToan, v.v.
            modelBuilder.Entity<ThanhToan>()
                .Property(t => t.GiaTri)
                .HasColumnType("decimal(18, 4)");
            // Nếu sau này gặp lỗi multiple cascade paths ở chỗ khác
            // thì ta cũng làm tương tự: cấu hình .OnDelete(DeleteBehavior.NoAction)
            modelBuilder.Entity<VatTu>()
                .HasOne(v => v.Kho)
                .WithMany(k => k.VatTus)
                .HasForeignKey(v => v.KhoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VatTu>()
                .HasOne(v => v.Nganh)
                .WithMany(n => n.VatTus)
                .HasForeignKey(v => v.NganhId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

