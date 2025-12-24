using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppApi.DataAccess.Migrations.WebApi
{
    /// <inheritdoc />
    public partial class Add_QLDT_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "NVARCHAR(250)", nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(1000)", nullable: true),
                    Pseudonym = table.Column<string>(type: "NVARCHAR(1000)", nullable: true),
                    FullName = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    DonViId = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Salt = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TimeLock = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    IsLock = table.Column<bool>(type: "bit", nullable: false),
                    VerifacationCode = table.Column<string>(type: "NVARCHAR(10)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuocQuyTrinh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaBuoc = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    TenBuoc = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    NhomQuyTrinh = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    ThuTu = table.Column<int>(type: "int", nullable: false),
                    RoleXuLyChinh = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    BatBuoc = table.Column<bool>(type: "bit", nullable: false),
                    LoaiForm = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuocQuyTrinh", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonVi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaDonVi = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    TenDonVi = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    LoaiDonVi = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    TrangThai = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    DonViChaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThuTu = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonVi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonVi_DonVi_DonViChaId",
                        column: x => x.DonViChaId,
                        principalTable: "DonVi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NhaThau",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenNhaThau = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    MaSoThue = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    DiaChi = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    LienHe = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    DienThoai = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    TrangThai = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaThau", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoginHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    LoginDate = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginHistory_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraoDoi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiDoiTuong = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    DoiTuongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuocMa = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    NguoiGuiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NguoiNhanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleNguoiNhan = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    NoiDung = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    ThoiGianGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraoDoi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TraoDoi_Account_NguoiGuiId",
                        column: x => x.NguoiGuiId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraoDoi_Account_NguoiNhanId",
                        column: x => x.NguoiNhanId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeXuatMuaSam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NamKeHoach = table.Column<int>(type: "int", nullable: false),
                    MaDeXuat = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    DonViDeXuatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenDeXuat = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    LyDo = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    MoTaChung = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    TongGiaTriUocTinh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    NguoiTaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeXuatMuaSam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeXuatMuaSam_Account_NguoiTaoId",
                        column: x => x.NguoiTaoId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeXuatMuaSam_DonVi_DonViDeXuatId",
                        column: x => x.DonViDeXuatId,
                        principalTable: "DonVi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoiThauKeHoach",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NamKeHoach = table.Column<int>(type: "int", nullable: false),
                    MaGoi = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    TenGoiThau = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DonViDeXuatChinhId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonViMuaSamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiMuaSam = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LinhVuc = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    GiaTriDuKien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HinhThucLCNTDuKien = table.Column<string>(type: "NVARCHAR(200)", nullable: true),
                    LoaiQuyTrinhDuKien = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    TrangThaiKeHoach = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    NguoiTaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoiThauKeHoach", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoiThauKeHoach_Account_NguoiTaoId",
                        column: x => x.NguoiTaoId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoiThauKeHoach_DonVi_DonViDeXuatChinhId",
                        column: x => x.DonViDeXuatChinhId,
                        principalTable: "DonVi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoiThauKeHoach_DonVi_DonViMuaSamId",
                        column: x => x.DonViMuaSamId,
                        principalTable: "DonVi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountRole",
                columns: table => new
                {
                    AccountsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRole", x => new { x.AccountsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_AccountRole_Account_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountRole_Role_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeXuatChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeXuatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenHangHoaDichVu = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    MaHang = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    SoLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonViTinh = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    DonGiaUocTinh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeXuatChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeXuatChiTiet_DeXuatMuaSam_DeXuatId",
                        column: x => x.DeXuatId,
                        principalTable: "DeXuatMuaSam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoiThauDeXuat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoiThauKeHoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeXuatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoiThauDeXuat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoiThauDeXuat_DeXuatMuaSam_DeXuatId",
                        column: x => x.DeXuatId,
                        principalTable: "DeXuatMuaSam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoiThauDeXuat_GoiThauKeHoach_GoiThauKeHoachId",
                        column: x => x.GoiThauKeHoachId,
                        principalTable: "GoiThauKeHoach",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoSoThau",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoiThauKeHoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoHoSo = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    TenGoiThau = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DonViDeXuatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonViMuaSamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiMuaSam = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    LinhVuc = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    HinhThucLCNT = table.Column<string>(type: "NVARCHAR(200)", nullable: true),
                    LoaiQuyTrinh = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    GiaTriDuToan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NguonVon = table.Column<string>(type: "NVARCHAR(200)", nullable: true),
                    ThoiGianTu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianDen = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThaiTong = table.Column<string>(type: "NVARCHAR(30)", nullable: false),
                    BuocHienTai = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    NguoiTaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoThau", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoSoThau_Account_NguoiTaoId",
                        column: x => x.NguoiTaoId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HoSoThau_DonVi_DonViDeXuatId",
                        column: x => x.DonViDeXuatId,
                        principalTable: "DonVi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HoSoThau_DonVi_DonViMuaSamId",
                        column: x => x.DonViMuaSamId,
                        principalTable: "DonVi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HoSoThau_GoiThauKeHoach_GoiThauKeHoachId",
                        column: x => x.GoiThauKeHoachId,
                        principalTable: "GoiThauKeHoach",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DanhGiaNhaThau",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NhaThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThoiDiemDanhGia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TieuChiJson = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    DiemTong = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NhanXet = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    XepLoai = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaNhaThau", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhGiaNhaThau_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiaNhaThau_NhaThau_NhaThauId",
                        column: x => x.NhaThauId,
                        principalTable: "NhaThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DuToanGoiThau",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GiaTriTruocThue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienThue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaTriSauThue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    NguoiLapId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuToanGoiThau", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuToanGoiThau_Account_NguoiLapId",
                        column: x => x.NguoiLapId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DuToanGoiThau_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HopDong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NhaThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoHopDong = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    NgayKy = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GiaTriHopDong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThoiGianTu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianDen = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BaoHanhThongTin = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    TrangThai = table.Column<string>(type: "NVARCHAR(30)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopDong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HopDong_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HopDong_NhaThau_NhaThauId",
                        column: x => x.NhaThauId,
                        principalTable: "NhaThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoSoDuThau",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NhaThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Loai = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    GiaDuThau = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiaDeXuat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiaSauDieuChinh = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiemKyThuat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiemTaiChinh = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    XepHang = table.Column<int>(type: "int", nullable: true),
                    KetLuan = table.Column<string>(type: "NVARCHAR(30)", nullable: true),
                    TrangThai = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoDuThau", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoSoDuThau_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoSoDuThau_NhaThau_NhaThauId",
                        column: x => x.NhaThauId,
                        principalTable: "NhaThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongTinKyThuat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MoTaChung = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    NguoiNhapId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinKyThuat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongTinKyThuat_Account_NguoiNhapId",
                        column: x => x.NguoiNhapId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ThongTinKyThuat_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThuMoiBaoGia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Loai = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HanNhan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiDungTomTat = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuMoiBaoGia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThuMoiBaoGia_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThuMoiThau",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Loai = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HanNhan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiDungTomTat = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuMoiThau", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThuMoiThau_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TienTrinhHoSo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuocQuyTrinhId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrangThaiBuoc = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    DonViXuLyThucTeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiXuLyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YKien = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    GhiChu = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TienTrinhHoSo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TienTrinhHoSo_Account_NguoiXuLyId",
                        column: x => x.NguoiXuLyId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TienTrinhHoSo_BuocQuyTrinh_BuocQuyTrinhId",
                        column: x => x.BuocQuyTrinhId,
                        principalTable: "BuocQuyTrinh",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TienTrinhHoSo_DonVi_DonViXuLyThucTeId",
                        column: x => x.DonViXuLyThucTeId,
                        principalTable: "DonVi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TienTrinhHoSo_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DuToanChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DuToanGoiThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenHangHoaDichVu = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    SoLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonGiaDuToan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CanCu = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    GhiChu = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuToanChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuToanChiTiet_DuToanGoiThau_DuToanGoiThauId",
                        column: x => x.DuToanGoiThauId,
                        principalTable: "DuToanGoiThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HopDongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DotThanhToan = table.Column<int>(type: "int", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GiaTri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaiLieuChungTu = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    GhiChu = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThanhToan_HopDong_HopDongId",
                        column: x => x.HopDongId,
                        principalTable: "HopDong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongTinKyThuatChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThongTinKyThuatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenHangHoaDichVu = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    MaHang = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    SoLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonViTinh = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    YeuCauKyThuat = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    GhiChu = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinKyThuatChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongTinKyThuatChiTiet_ThongTinKyThuat_ThongTinKyThuatId",
                        column: x => x.ThongTinKyThuatId,
                        principalTable: "ThongTinKyThuat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThuMoiBaoGia_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThuMoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NhaThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HinhThucGui = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    ThongTinLienHe = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    TrangThai = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuMoiBaoGia_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThuMoiBaoGia_ChiTiet_NhaThau_NhaThauId",
                        column: x => x.NhaThauId,
                        principalTable: "NhaThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThuMoiBaoGia_ChiTiet_ThuMoiBaoGia_ThuMoiId",
                        column: x => x.ThuMoiId,
                        principalTable: "ThuMoiBaoGia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThuMoiThau_ChiTiet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThuMoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NhaThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HinhThucGui = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    ThongTinLienHe = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    TrangThai = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuMoiThau_ChiTiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThuMoiThau_ChiTiet_NhaThau_NhaThauId",
                        column: x => x.NhaThauId,
                        principalTable: "NhaThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThuMoiThau_ChiTiet_ThuMoiThau_ThuMoiId",
                        column: x => x.ThuMoiId,
                        principalTable: "ThuMoiThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileDinhKem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TienTrinhHoSoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LoaiFile = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    TenHienThi = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    TenFileGoc = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DuongDan = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    NgayUpload = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiUploadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDinhKem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileDinhKem_Account_NguoiUploadId",
                        column: x => x.NguoiUploadId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileDinhKem_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileDinhKem_TienTrinhHoSo_TienTrinhHoSoId",
                        column: x => x.TienTrinhHoSoId,
                        principalTable: "TienTrinhHoSo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThamDinh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TienTrinhHoSoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LoaiThamDinh = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    NoiDung = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    KetLuan = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    KienNghi = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    NgayThamDinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiThamDinhId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThamDinh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThamDinh_Account_NguoiThamDinhId",
                        column: x => x.NguoiThamDinhId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ThamDinh_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThamDinh_TienTrinhHoSo_TienTrinhHoSoId",
                        column: x => x.TienTrinhHoSoId,
                        principalTable: "TienTrinhHoSo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NghiemThu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HopDongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayNghiemThu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiDung = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    KetQua = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    FileBienBanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NghiemThu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NghiemThu_FileDinhKem_FileBienBanId",
                        column: x => x.FileBienBanId,
                        principalTable: "FileDinhKem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NghiemThu_HopDong_HopDongId",
                        column: x => x.HopDongId,
                        principalTable: "HopDong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToChuyenGia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoThauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiTo = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    SoQuyetDinh = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    NgayQuyetDinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileQuyetDinhId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToChuyenGia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToChuyenGia_FileDinhKem_FileQuyetDinhId",
                        column: x => x.FileQuyetDinhId,
                        principalTable: "FileDinhKem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ToChuyenGia_HoSoThau_HoSoThauId",
                        column: x => x.HoSoThauId,
                        principalTable: "HoSoThau",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThanhVienToChuyenGia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToChuyenGiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoTen = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DonVi = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    VaiTro = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhVienToChuyenGia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThanhVienToChuyenGia_ToChuyenGia_ToChuyenGiaId",
                        column: x => x.ToChuyenGiaId,
                        principalTable: "ToChuyenGia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_RolesId",
                table: "AccountRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaNhaThau_HoSoThauId",
                table: "DanhGiaNhaThau",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaNhaThau_NhaThauId",
                table: "DanhGiaNhaThau",
                column: "NhaThauId");

            migrationBuilder.CreateIndex(
                name: "IX_DeXuatChiTiet_DeXuatId",
                table: "DeXuatChiTiet",
                column: "DeXuatId");

            migrationBuilder.CreateIndex(
                name: "IX_DeXuatMuaSam_DonViDeXuatId",
                table: "DeXuatMuaSam",
                column: "DonViDeXuatId");

            migrationBuilder.CreateIndex(
                name: "IX_DeXuatMuaSam_NguoiTaoId",
                table: "DeXuatMuaSam",
                column: "NguoiTaoId");

            migrationBuilder.CreateIndex(
                name: "IX_DonVi_DonViChaId",
                table: "DonVi",
                column: "DonViChaId");

            migrationBuilder.CreateIndex(
                name: "IX_DuToanChiTiet_DuToanGoiThauId",
                table: "DuToanChiTiet",
                column: "DuToanGoiThauId");

            migrationBuilder.CreateIndex(
                name: "IX_DuToanGoiThau_HoSoThauId",
                table: "DuToanGoiThau",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_DuToanGoiThau_NguoiLapId",
                table: "DuToanGoiThau",
                column: "NguoiLapId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDinhKem_HoSoThauId",
                table: "FileDinhKem",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDinhKem_NguoiUploadId",
                table: "FileDinhKem",
                column: "NguoiUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDinhKem_TienTrinhHoSoId",
                table: "FileDinhKem",
                column: "TienTrinhHoSoId");

            migrationBuilder.CreateIndex(
                name: "IX_GoiThauDeXuat_DeXuatId",
                table: "GoiThauDeXuat",
                column: "DeXuatId");

            migrationBuilder.CreateIndex(
                name: "IX_GoiThauDeXuat_GoiThauKeHoachId",
                table: "GoiThauDeXuat",
                column: "GoiThauKeHoachId");

            migrationBuilder.CreateIndex(
                name: "IX_GoiThauKeHoach_DonViDeXuatChinhId",
                table: "GoiThauKeHoach",
                column: "DonViDeXuatChinhId");

            migrationBuilder.CreateIndex(
                name: "IX_GoiThauKeHoach_DonViMuaSamId",
                table: "GoiThauKeHoach",
                column: "DonViMuaSamId");

            migrationBuilder.CreateIndex(
                name: "IX_GoiThauKeHoach_NguoiTaoId",
                table: "GoiThauKeHoach",
                column: "NguoiTaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HopDong_HoSoThauId",
                table: "HopDong",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_HopDong_NhaThauId",
                table: "HopDong",
                column: "NhaThauId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoDuThau_HoSoThauId",
                table: "HoSoDuThau",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoDuThau_NhaThauId",
                table: "HoSoDuThau",
                column: "NhaThauId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoThau_DonViDeXuatId",
                table: "HoSoThau",
                column: "DonViDeXuatId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoThau_DonViMuaSamId",
                table: "HoSoThau",
                column: "DonViMuaSamId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoThau_GoiThauKeHoachId",
                table: "HoSoThau",
                column: "GoiThauKeHoachId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoThau_NguoiTaoId",
                table: "HoSoThau",
                column: "NguoiTaoId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginHistory_AccountId",
                table: "LoginHistory",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_NghiemThu_FileBienBanId",
                table: "NghiemThu",
                column: "FileBienBanId");

            migrationBuilder.CreateIndex(
                name: "IX_NghiemThu_HopDongId",
                table: "NghiemThu",
                column: "HopDongId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThamDinh_HoSoThauId",
                table: "ThamDinh",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_ThamDinh_NguoiThamDinhId",
                table: "ThamDinh",
                column: "NguoiThamDinhId");

            migrationBuilder.CreateIndex(
                name: "IX_ThamDinh_TienTrinhHoSoId",
                table: "ThamDinh",
                column: "TienTrinhHoSoId");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_HopDongId",
                table: "ThanhToan",
                column: "HopDongId");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhVienToChuyenGia_ToChuyenGiaId",
                table: "ThanhVienToChuyenGia",
                column: "ToChuyenGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinKyThuat_HoSoThauId",
                table: "ThongTinKyThuat",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinKyThuat_NguoiNhapId",
                table: "ThongTinKyThuat",
                column: "NguoiNhapId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinKyThuatChiTiet_ThongTinKyThuatId",
                table: "ThongTinKyThuatChiTiet",
                column: "ThongTinKyThuatId");

            migrationBuilder.CreateIndex(
                name: "IX_ThuMoiBaoGia_HoSoThauId",
                table: "ThuMoiBaoGia",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_ThuMoiBaoGia_ChiTiet_NhaThauId",
                table: "ThuMoiBaoGia_ChiTiet",
                column: "NhaThauId");

            migrationBuilder.CreateIndex(
                name: "IX_ThuMoiBaoGia_ChiTiet_ThuMoiId",
                table: "ThuMoiBaoGia_ChiTiet",
                column: "ThuMoiId");

            migrationBuilder.CreateIndex(
                name: "IX_ThuMoiThau_HoSoThauId",
                table: "ThuMoiThau",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_ThuMoiThau_ChiTiet_NhaThauId",
                table: "ThuMoiThau_ChiTiet",
                column: "NhaThauId");

            migrationBuilder.CreateIndex(
                name: "IX_ThuMoiThau_ChiTiet_ThuMoiId",
                table: "ThuMoiThau_ChiTiet",
                column: "ThuMoiId");

            migrationBuilder.CreateIndex(
                name: "IX_TienTrinhHoSo_BuocQuyTrinhId",
                table: "TienTrinhHoSo",
                column: "BuocQuyTrinhId");

            migrationBuilder.CreateIndex(
                name: "IX_TienTrinhHoSo_DonViXuLyThucTeId",
                table: "TienTrinhHoSo",
                column: "DonViXuLyThucTeId");

            migrationBuilder.CreateIndex(
                name: "IX_TienTrinhHoSo_HoSoThauId",
                table: "TienTrinhHoSo",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_TienTrinhHoSo_NguoiXuLyId",
                table: "TienTrinhHoSo",
                column: "NguoiXuLyId");

            migrationBuilder.CreateIndex(
                name: "IX_ToChuyenGia_FileQuyetDinhId",
                table: "ToChuyenGia",
                column: "FileQuyetDinhId");

            migrationBuilder.CreateIndex(
                name: "IX_ToChuyenGia_HoSoThauId",
                table: "ToChuyenGia",
                column: "HoSoThauId");

            migrationBuilder.CreateIndex(
                name: "IX_TraoDoi_NguoiGuiId",
                table: "TraoDoi",
                column: "NguoiGuiId");

            migrationBuilder.CreateIndex(
                name: "IX_TraoDoi_NguoiNhanId",
                table: "TraoDoi",
                column: "NguoiNhanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountRole");

            migrationBuilder.DropTable(
                name: "DanhGiaNhaThau");

            migrationBuilder.DropTable(
                name: "DeXuatChiTiet");

            migrationBuilder.DropTable(
                name: "DuToanChiTiet");

            migrationBuilder.DropTable(
                name: "GoiThauDeXuat");

            migrationBuilder.DropTable(
                name: "HoSoDuThau");

            migrationBuilder.DropTable(
                name: "LoginHistory");

            migrationBuilder.DropTable(
                name: "NghiemThu");

            migrationBuilder.DropTable(
                name: "ThamDinh");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "ThanhVienToChuyenGia");

            migrationBuilder.DropTable(
                name: "ThongTinKyThuatChiTiet");

            migrationBuilder.DropTable(
                name: "ThuMoiBaoGia_ChiTiet");

            migrationBuilder.DropTable(
                name: "ThuMoiThau_ChiTiet");

            migrationBuilder.DropTable(
                name: "TraoDoi");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "DuToanGoiThau");

            migrationBuilder.DropTable(
                name: "DeXuatMuaSam");

            migrationBuilder.DropTable(
                name: "HopDong");

            migrationBuilder.DropTable(
                name: "ToChuyenGia");

            migrationBuilder.DropTable(
                name: "ThongTinKyThuat");

            migrationBuilder.DropTable(
                name: "ThuMoiBaoGia");

            migrationBuilder.DropTable(
                name: "ThuMoiThau");

            migrationBuilder.DropTable(
                name: "NhaThau");

            migrationBuilder.DropTable(
                name: "FileDinhKem");

            migrationBuilder.DropTable(
                name: "TienTrinhHoSo");

            migrationBuilder.DropTable(
                name: "BuocQuyTrinh");

            migrationBuilder.DropTable(
                name: "HoSoThau");

            migrationBuilder.DropTable(
                name: "GoiThauKeHoach");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "DonVi");
        }
    }
}
