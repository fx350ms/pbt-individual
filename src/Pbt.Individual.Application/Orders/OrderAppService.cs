using Abp.Application.Services.Dto;
using Abp.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Pbt.Individual.Core;
using Pbt.Individual.Orders.Dto;
using Pbt.Individual.Packages.Dto;
using PBT.CacheService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pbt.Individual.Orders
{

    [Authorize]
    [AbpAuthorize]
    public class OrderAppService : IndividualAppServiceBase, IOrderAppService
    {

        public OrderAppService(AppCacheService cacheService) : base(cacheService)
        {
        }

        public async Task<CreateWaybillListResultDto> CreateWaybillsAsync(CreateWaybillListDto input)
        {
            // Implementation of creating waybills based on the provided input
            // Validate thông tin đầu vào
            if (string.IsNullOrWhiteSpace(input.WaybillCodes))
            {
                throw new ArgumentException("Waybill codes cannot be empty.");
            }

            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null || currentUser.CustomerId == null)
            {
                throw new InvalidOperationException("Current user does not have an associated customer ID.");
            }
            // Convert lại chỗi mã vận đơn thành đúng format: 'code1,code2,code3'
            var waybillCodes = input.WaybillCodes.Split(new[] { "\r\n", "\r", "\n", Environment.NewLine, ";", "," }, StringSplitOptions.None);

            input.WaybillCodes = string.Join(",", waybillCodes).Trim();

            Logger.Info($"Creating waybills for customer ID: {currentUser.CustomerId.Value}, data: {input}");
            try
            {

                var statusCodePr = new SqlParameter("@StatusCode", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var messagePr = new SqlParameter("@Message", System.Data.SqlDbType.NVarChar, -1) { Direction = System.Data.ParameterDirection.Output };
                var successCountPr = new SqlParameter("@SuccessCount", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var errorCountPr = new SqlParameter("@ErrorCount", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var customerNamePr = new SqlParameter("@CustomerName", System.Data.SqlDbType.NVarChar, 256) { Direction = System.Data.ParameterDirection.Output };

                var prs = new[]
                {
                new SqlParameter() { ParameterName = "@CustomerId", Value =  currentUser.CustomerId.Value},
                new SqlParameter() { ParameterName = "@WaybillCodes", Value = input.WaybillCodes },
                new SqlParameter() { ParameterName = "@Note", Value = input.Note ?? string.Empty },
                new SqlParameter() { ParameterName = "@CreatorUserName", Value = currentUser.UserName },
                statusCodePr,
                messagePr,
                successCountPr,
                errorCountPr,
                customerNamePr
            };

                await ConnectDb.ExecuteQueryAsync("SP_Orders_CreateListByWaybill", System.Data.CommandType.StoredProcedure, prs);

                var result = new CreateWaybillListResultDto
                {
                    StatusCode = statusCodePr.Value != DBNull.Value ? (int)statusCodePr.Value : 0,
                    Message = messagePr.Value != DBNull.Value ? (string)messagePr.Value : string.Empty,
                    TotalCreatedSuccess = successCountPr.Value != DBNull.Value ? (int)successCountPr.Value : 0,
                    TotalFailed = errorCountPr.Value != DBNull.Value ? (int)errorCountPr.Value : 0,
                    TotalWaybills = (successCountPr.Value != DBNull.Value ? (int)successCountPr.Value : 0) + (errorCountPr.Value != DBNull.Value ? (int)errorCountPr.Value : 0),
                };
                return result;

            }
            catch (Exception ex)
            {
                Logger.Error("Error occurred while creating waybills.", ex);
                throw;
            }
        }

        public async Task<OrderDto> GetAsync(long id)
        {
            var idPr = new SqlParameter("@Id", System.Data.SqlDbType.BigInt) { Value = id };
            var data = await ConnectDb.GetItemAsync<OrderDto>("SP_Orders_GetById", System.Data.CommandType.StoredProcedure,
             new[] { idPr });
            var currentUser = await GetCurrentUserAsync();
            if (data != null && data.CustomerId != currentUser.CustomerId)
            {
                throw new AbpAuthorizationException("You are not authorized to access this order.");
            }
            return data;
        }

        public async Task<OrderDetailDto> GetDetailAsync(long id)
        {
            var idPr = new SqlParameter("@Id", System.Data.SqlDbType.BigInt) { Value = id };
            var data = await ConnectDb.GetItemAsync<OrderDetailDto>("SP_Orders_GetDetailById", System.Data.CommandType.StoredProcedure,
             new[] { idPr });
            var currentUser = await GetCurrentUserAsync();
            if (data != null && data.CustomerId != currentUser.CustomerId)
            {
                throw new AbpAuthorizationException("You are not authorized to access this order.");
            }
            return data;
        }


        public async Task<(OrderDetailDto, List<PackageOrderViewDto>)> GetDetailWithPackagesAsync(long id)
        {
            var order = await GetDetailAsync(id);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            var orderIdPr = new SqlParameter("@OrderId", System.Data.SqlDbType.BigInt) { Value = id };
//             SP_Packages_GetByOrderId
// @OrderId BIGINT 

            List<PackageOrderViewDto> packages = await ConnectDb.GetListAsync<PackageOrderViewDto>("SP_Packages_GetByOrderId", System.Data.CommandType.StoredProcedure, new[] { orderIdPr });
            return (order, packages);
        }


        public async Task<PagedResultDto<OrderDto>> GetCustomerOrdersAsync(OrderPagedResultRequestDto input)
        {
            var currentUser = await GetCurrentUserAsync();

            var totalCountParameter = new SqlParameter("@TotalCount", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
            var prs = new[]
            {
                new SqlParameter() { ParameterName = "@CustomerId", Value =  currentUser.CustomerId ?? 0 },
                new SqlParameter() { ParameterName = "@Keyword", Value = input.Keyword ??  string.Empty },
                new SqlParameter() { ParameterName = "@Status", Value = input.Status },
                new SqlParameter() { ParameterName = "@ShippingLine", Value = input.ShippingLine },
                new SqlParameter() { ParameterName = "@StartCreateDate", Value = input.StartCreateDateStr ?? (object)DBNull.Value },
                new SqlParameter() { ParameterName = "@EndCreateDate", Value = input.EndCreateDateStr ?? (object)DBNull.Value },
                new SqlParameter() { ParameterName = "@StartMatchDate", Value = input.StartMatchDateStr ?? (object)DBNull.Value },
                new SqlParameter() { ParameterName = "@EndMatchDate", Value = input.EndMatchDateStr ?? (object)DBNull.Value },
                new SqlParameter() { ParameterName = "@StartExportDate", Value = input.StartExportDateStr ?? (object)DBNull.Value },
                new SqlParameter() { ParameterName = "@EndExportDate", Value = input.EndExportDateStr ?? (object)DBNull.Value },
                new SqlParameter() { ParameterName = "@StartImportDate", Value = input.StartImportDateStr ?? (object)DBNull.Value },
                new SqlParameter() { ParameterName = "@EndImportDate", Value = input.EndImportDateStr    ?? (object)DBNull.Value },
                new SqlParameter() { ParameterName = "@SkipCount", Value = input.SkipCount },
                new SqlParameter() { ParameterName = "@MaxResultCount", Value = input.MaxResultCount },
                totalCountParameter
            };

            var data = await ConnectDb.GetListAsync<OrderDto>("SP_Orders_GetPagedMyOrders", System.Data.CommandType.StoredProcedure, prs);
            var totalCount = (int)totalCountParameter.Value;
            return new PagedResultDto<OrderDto>(totalCount, data);
        }


    }
}
