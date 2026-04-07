using Abp.Application.Services.Dto;
using Abp.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Pbt.Individual.Core;
using Pbt.Individual.Orders.Dto;
using PBT.CacheService;
using System;
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

        public Task<CreateWaybillListResultDto> CreateWaybillsAsync(CreateWaybillListDto input)
        {
            throw new NotImplementedException();
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
