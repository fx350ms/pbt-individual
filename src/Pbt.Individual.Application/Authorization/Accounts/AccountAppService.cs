using Abp.Configuration;
using Abp.UI;
using Abp.Zero.Configuration;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using pbt.Customers;
using Pbt.Individual.Authorization.Accounts.Dto;
using Pbt.Individual.Authorization.Users;
using Pbt.Individual.Core;
using PBT.CacheService;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Pbt.Individual.Authorization.Accounts;

public class AccountAppService : IndividualAppServiceBase, IAccountAppService
{
    // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
    public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";
    private readonly IPasswordHasher<User> _passwordHasher;

    private readonly UserRegistrationManager _userRegistrationManager;
    private readonly ICustomerAppService _customerAppService;

    public AccountAppService(
        AppCacheService cacheService,
        UserRegistrationManager userRegistrationManager,
        ICustomerAppService customerAppService,
        IPasswordHasher<User> passwordHasher
      ) : base(cacheService)
    {
        _userRegistrationManager = userRegistrationManager;
        _customerAppService = customerAppService;
        _passwordHasher = passwordHasher;
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
        // var user = await _userRegistrationManager.RegisterAsync(
        //     input.Name,
        //     input.PhoneNumber,
        //     input.EmailAddress,
        //     input.UserName,
        //     input.Password,
        //     true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
        // );

        var user = new User
        {
            TenantId = 1,
            Name = input.Name,
            Surname = "",
            PhoneNumber = input.PhoneNumber,
            EmailAddress = input.EmailAddress,
            IsActive = true,
            UserName = input.UserName,
            IsEmailConfirmed = true,
            CreationTime = DateTime.Now,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var statusPr = new SqlParameter("@Status", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        var msgPr = new SqlParameter("@Msg", SqlDbType.NVarChar, -1)
        {
            Direction = ParameterDirection.Output
        };
        var prs = new[]
                {
            new SqlParameter("@TenantId", AbpSession.TenantId ?? 0),
            new SqlParameter("@Name", input.Name),
            new SqlParameter("@PhoneNumber", input.PhoneNumber),
            new SqlParameter("@EmailAddress", input.EmailAddress),
            new SqlParameter("@UserName", input.UserName),
            new SqlParameter("@Password", _passwordHasher.HashPassword(user,input.Password)),
            statusPr,
            msgPr
        };

        await ConnectDb.ExecuteNonQueryAsync("SP_AbpUsers_Register", CommandType.StoredProcedure, prs);

        if ((int)statusPr.Value != 0)
        {
            throw new UserFriendlyException((string)msgPr.Value);
        }


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

 
}
