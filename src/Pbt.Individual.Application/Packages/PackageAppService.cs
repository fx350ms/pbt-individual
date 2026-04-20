using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Pbt.Individual.Core;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PBT.CacheService;
using Pbt.Individual.Packages.Dto;

namespace Pbt.Individual.Packages
{
    public class PackageAppService : IndividualAppServiceBase, IPackageAppService
    {
        public PackageAppService(AppCacheService cacheService) : base(cacheService)
        {
        }

        public async Task<List<PackageOrderViewDto>> GetByOrderIdAsync(long orderId)
        {
            var pr = new[]
            {
                new SqlParameter { ParameterName = "OrderId", Value = orderId }
            };

             var packages = await ConnectDb.GetListAsync<PackageOrderViewDto>("SP_Packages_GetByOrderId", System.Data.CommandType.StoredProcedure, pr);
            return packages;
        }
        
    }
}