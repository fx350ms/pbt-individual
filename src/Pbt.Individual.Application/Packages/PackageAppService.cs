using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Pbt.Individual.Core;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PBT.CacheService;
using Pbt.Individual.Packages.Dto;
using Pbt.Individual.Web.ViewModels.DeliveryRequests;
using System.Data;
using System;
using Abp.Authorization;
using Pbt.Individual.ApplicationUtils;

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

        public async Task<List<PackageViewByBagDto>> GetAllPackagesListByBagIdAsync(int bagId)
        {
            try
            {
                var data = await ConnectDb.GetListAsync<PackageViewByBagDto>(
                 "SP_Packages_GetByBagId_ForBagDetail",
                 CommandType.StoredProcedure,
                new[] {
                    new SqlParameter("@BagId", SqlDbType.Int) { Value = bagId }
                 }
             );
                return data;
            }
            catch (Exception ex)
            {
                Logger.Error($"[GetAllPackagesListByBagId] Exception: {ex}");
                return new List<PackageViewByBagDto>();
            }
        }


        public async Task<PagedResultDto<PackageItemForCreateNewDeliveryRequestDto>> GetAllPackagesForCreateNewDeliveryRequestAsync()
        {
            try
            {
                var currentUser = await GetCurrentUserAsync();
                if (currentUser is null || !currentUser.CustomerId.HasValue)
                {
                    throw new AbpAuthorizationException("You are not authorized to access delivery requests.");
                }
                var prs = new[]
                    {
                    new SqlParameter("@CustomerId",   currentUser.CustomerId.Value),
                    new SqlParameter("@WarehouseId",  -1),
                    new SqlParameter("@Status", (int) PackageShippingStatusEnum.InWarehouseVN) 
                };


                var data = await ConnectDb.GetListAsync<PackageItemForCreateNewDeliveryRequestDto>(
                 "SP_Packages_GetForCreateNewDeliveryRequest",
                 CommandType.StoredProcedure,
               prs
             );
                return new PagedResultDto<PackageItemForCreateNewDeliveryRequestDto>
                {
                    Items = data,
                    TotalCount = data.Count
                };
            }
            catch (Exception ex)
            {
                Logger.Error($"[GetAllPackagesListByBagId] Exception: {ex}");
                return new PagedResultDto<PackageItemForCreateNewDeliveryRequestDto>
                {
                    Items = new List<PackageItemForCreateNewDeliveryRequestDto>(),
                    TotalCount = 0
                };
            }
        }

    }
}