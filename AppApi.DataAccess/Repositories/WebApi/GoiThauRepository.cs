//using AppApi.DataAccess.Base;
//using AppApi.DataAccess.IRepositories;
//using AppApi.Entities.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AppApi.DataAccess.Repositories.WebApi
//{
//    public class GoiThauRepository : GenericRepository<GoiThauDeXuat>, ICommonRepository
//    {
//        public GoiThauRepository(ApplicationDbContext context, ILogger<GoiThauDeXuat> logger) : base(context, logger)
//        {
//        }
//        public override async Task<GoiThauDeXuat> UpsertAsync(GoiThauDeXuat entity)
//        {
//            try
//            {
//                var existItem = await DbSet
//                    .Where(x => x.Id == entity.Id)
//                    .FirstOrDefaultAsync();

//                // Nếu chưa có thì thêm mới
//                if (existItem == null)
//                {
//                    await AddOneAsync(entity);
//                    return entity;
//                }

//                // Map các field cần update
//                existItem.MaGoiThau = entity.MaGoiThau;
//                existItem.TenGoiThau = entity.TenGoiThau;
//                //existItem.KeHoachMuaSamId = entity.KeHoachMuaSamId;
//                existItem.DonViChuTriId = entity.DonViChuTriId;
//                existItem.HinhThucChonNhaThau = entity.HinhThucChonNhaThau;
//                existItem.LoaiGoiThau = entity.LoaiGoiThau;
//                existItem.GiaTriDuToan = entity.GiaTriDuToan;
//                existItem.NguonKinhPhi = entity.NguonKinhPhi;
//                existItem.TrangThaiHoSoId = entity.TrangThaiHoSoId;
//                existItem.NgayKhoiTao = entity.NgayKhoiTao;
//                existItem.NgayHoanThanh = entity.NgayHoanThanh;
//                existItem.GhiChu = entity.GhiChu;

//                // Audit
//                existItem.UpdatedBy = entity.UpdatedBy;
//                existItem.UpdatedDate = DateTime.UtcNow;

//                return existItem;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "{Repo} UpsertAsync method error", typeof(GoiThauRepository));
//                throw ex;
//            }
//        }
//    }
//}
