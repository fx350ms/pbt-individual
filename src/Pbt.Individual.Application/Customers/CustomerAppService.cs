using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Pbt.Individual.Authorization.Accounts.Dto;
using Pbt.Individual.Core;
using System;
using System.Data;
using System.Threading.Tasks;

namespace pbt.Customers
{
    [AbpAuthorize]
    [Audited]
    public class CustomerAppService : AbpServiceBase, ICustomerAppService
    {
        private const string CreateCustomerByUserStoredProcedure = "SP_Customers_CreateByUser";

        [AbpAllowAnonymous]
        [AllowAnonymous]
        public async Task<long> CreateFromRegistrationAsync(CreateCustomerDto input)
        {
            if (input == null)
            {
                throw new UserFriendlyException("Thông tin khách hàng không hợp lệ.");
            }

            var customerIdOutput = new SqlParameter("@CustomerId", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };

            var statusOutput = new SqlParameter("@Status", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var messageOutput = new SqlParameter("@Message", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
            {
                new SqlParameter("@UserId", input.UserId ?? (object)DBNull.Value),
                new SqlParameter("@Username", input.Username ?? (object)DBNull.Value),
                new SqlParameter("@CustomerCode", input.Username ?? (object)DBNull.Value),
                new SqlParameter("@FullName", input.FullName ?? (object)DBNull.Value),
                new SqlParameter("@Email", input.Email ?? (object)DBNull.Value),
                new SqlParameter("@PhoneNumber", input.PhoneNumber ?? (object)DBNull.Value),
                new SqlParameter("@WarehouseId", input.WarehouseId ?? (object)DBNull.Value),
                customerIdOutput,
                statusOutput,
                messageOutput
            };

            await ConnectDb.ExecuteNonQueryAsync(CreateCustomerByUserStoredProcedure, CommandType.StoredProcedure, parameters);

            if (customerIdOutput.Value == DBNull.Value || !long.TryParse(customerIdOutput.Value.ToString(), out var customerId) || customerId <= 0)
            {
                throw new UserFriendlyException("Không thể tạo khách hàng từ thông tin đăng ký. Vui lòng kiểm tra Store Procedure SP_Customers_CreateByUser.");
            }

            return customerId;
        }

        public async Task SynchronizeCustomerWithUserAsync(long customerId, string username)
        {

            var prs = new[]
            {
                    new SqlParameter("@CustomerId", customerId),
                    new SqlParameter("@Username", username )
            };

            await ConnectDb.ExecuteNonQueryAsync("SP_Customers_LinkToAccount", CommandType.StoredProcedure, prs);
        }

         
    }
}
