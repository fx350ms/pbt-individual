using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using Abp;
using Pbt.Individual.Warehouses.Dto;
using Pbt.Individual.Core;
using Microsoft.Data.SqlClient;
using PBT.CacheService;
using Pbt.Individual;

namespace pbt.Warehouses
{
    public class WarehouseAppService : AbpServiceBase, IWarehouseAppService
    {
        private readonly IAppCacheService _appCacheService;

        public WarehouseAppService(IAppCacheService appCacheService)
        {
            _appCacheService = appCacheService;
        }

        public async Task<List<WarehouseDto>> GetByTypeAsync(int warehouseType)
        {
            var key = string.Format(CacheKey.Warehouses_ByType, warehouseType);
            if (_appCacheService.TryGetCacheValue(key, out List<WarehouseDto> cachedData))
            {
                return cachedData;
            }

            var prs = new[]
            {
                new SqlParameter("WarehouseType", warehouseType)
            };
            var data = await ConnectDb.GetListAsync<WarehouseDto>("SP_Warehouses_GetByType", System.Data.CommandType.StoredProcedure, prs);
    
            _appCacheService.SetCacheValue(key, data);
            return data;
        }
    }
}
