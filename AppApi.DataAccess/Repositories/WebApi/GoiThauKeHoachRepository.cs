using AppApi.DataAccess.Base;
using AppApi.DataAccess.IRepositories;
using AppApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DataAccess.Repositories.WebApi
{
    public class GoiThauKeHoachRepository : GenericRepository<GoiThauKeHoach>, ICommonRepository
    {
        public GoiThauKeHoachRepository(ApplicationDbContext context, ILogger<GoiThauKeHoach> logger)
            : base(context, logger)
        {
        }

        public override async Task<GoiThauKeHoach> UpsertAsync(GoiThauKeHoach entity)
        {
            try
            {
                var existItem = await DbSet.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

                if (existItem == null)
                {
                    await AddOneAsync(entity);
                    return entity;
                }

                // Map field-by-field (an toàn, rõ ràng)
                existItem.MaGoi = entity.MaGoi;
                existItem.TenGoiThau = entity.TenGoiThau;
                existItem.NamKeHoach = entity.NamKeHoach;

                existItem.DonViDeXuatChinhId = entity.DonViDeXuatChinhId;
                existItem.DonViMuaSamId = entity.DonViMuaSamId;

                existItem.LoaiMuaSam = entity.LoaiMuaSam;
                existItem.LinhVuc = entity.LinhVuc;
                existItem.HinhThucLCNTDuKien = entity.HinhThucLCNTDuKien;
                existItem.LoaiQuyTrinhDuKien = entity.LoaiQuyTrinhDuKien;

                existItem.GiaTriDuKien = entity.GiaTriDuKien;
                existItem.TrangThaiKeHoach = entity.TrangThaiKeHoach;

                existItem.UpdatedBy = entity.UpdatedBy;
                existItem.UpdatedDate = DateTime.UtcNow;

                return existItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpsertAsync method error", typeof(GoiThauKeHoachRepository));
                throw;
            }
        }
    }
}
