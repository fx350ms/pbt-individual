using Abp.Configuration;
using Abp.Zero.Configuration;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using pbt.Customers;
using Pbt.Individual.Authorization.Accounts.Dto;
using Pbt.Individual.Authorization.Users;
using Pbt.Individual.Core;
using PBT.CacheService;
using System.Data;
using System.Threading.Tasks;

namespace Pbt.Individual.Authorization.Accounts;

public class AccountAppService : IndividualAppServiceBase, IAccountAppService
{
    // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
    public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

    private readonly UserRegistrationManager _userRegistrationManager;
    private readonly ICustomerAppService _customerAppService;

    public AccountAppService(
        AppCacheService cacheService,
        UserRegistrationManager userRegistrationManager,
        ICustomerAppService customerAppService
      ) : base(cacheService)
    {
        _userRegistrationManager = userRegistrationManager;
        _customerAppService = customerAppService;
    }
    public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
    {
        var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
        if (tenant == null)
        {
            return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
        }

        if (!tenant.IsActive)
        {
            return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
        }

        return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
    }

    public async Task<RegisterOutput> Register(RegisterInput input)
    {
        var user = await _userRegistrationManager.RegisterAsync(
            input.Name,
            input.PhoneNumber,
            input.EmailAddress,
            input.UserName,
            input.Password,
            true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
        );
        var customer = new CreateCustomerDto
        {
            Username = input.UserName,
            FullName = input.Name,
            Email = input.EmailAddress,
            PhoneNumber = input.PhoneNumber,
            UserId = user.Id,
            WarehouseId = input.WarehouseId,
        };

        await _customerAppService.CreateFromRegistrationAsync(customer);

        var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

        return new RegisterOutput
        {
            CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
        };
    }

    public async Task UpdateCustomerIdForUserAsync(long userId, long customerId)
    {
        await ConnectDb.ExecuteNonQueryAsync("SP_AbpUsers_UpdateCustomerId", CommandType.StoredProcedure, new[]
       {
            new SqlParameter("@UserId", userId),
            new SqlParameter("@CustomerId", customerId)
        });
    }


    // public async Task SynchronizeCustomerWithUserAsync(long customerId, string username)
    // {

    //     var prs = new[]
    //     {
    //             new SqlParameter("@CustomerId", customerId),
    //             new SqlParameter("@Username", username )
    //     };

    //     await ConnectDb.ExecuteNonQueryAsync("SP_Customers_LinkToAccount", CommandType.StoredProcedure, prs);
    // }
}
