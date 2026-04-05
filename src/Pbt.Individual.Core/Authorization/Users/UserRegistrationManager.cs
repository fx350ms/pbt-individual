using Abp.Authorization.Users;
using Abp.Domain.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.Data.SqlClient;
using Pbt.Individual.Authorization.Roles;
using Pbt.Individual.Core;
using Pbt.Individual.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Pbt.Individual.Authorization.Users;

public class UserRegistrationManager : DomainService
{
    public IAbpSession AbpSession { get; set; }

    private readonly TenantManager _tenantManager;
    private readonly UserManager _userManager;
    private readonly RoleManager _roleManager;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRegistrationManager(
        TenantManager tenantManager,
        UserManager userManager,
        RoleManager roleManager,
        IPasswordHasher<User> passwordHasher)
    {
        _tenantManager = tenantManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _passwordHasher = passwordHasher;

        AbpSession = NullAbpSession.Instance;
    }

    public async Task<User> RegisterAsync(string name, string phoneNumber, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed)
    {
        CheckForTenant();

        var tenant = await GetActiveTenantAsync();

        var user = new User
        {
            TenantId = tenant.Id,
            Name = name,
            Surname = "",
            PhoneNumber = phoneNumber,
            EmailAddress = emailAddress,
            IsActive = true,
            UserName = userName,
            IsEmailConfirmed = isEmailConfirmed,
            CreationTime = DateTime.Now,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Roles = new List<UserRole>(),
            Password = _passwordHasher.HashPassword(null, plainPassword)
        };

        user.SetNormalizedNames();
       // user.Password = _passwordHasher.HashPassword(user, plainPassword);

        var statusPr = new SqlParameter("@Status", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        var msgPr = new SqlParameter("@Msg", SqlDbType.NVarChar, -1)
        {
            Direction = ParameterDirection.Output
        };
        var userIdPr = new SqlParameter("@UserId", SqlDbType.BigInt)
        {
            Direction = ParameterDirection.Output
        };
        var prs = new[]
                {
            new SqlParameter("@TenantId", AbpSession.TenantId ?? 0),
            new SqlParameter("@Name", user.Name),
            new SqlParameter("@PhoneNumber", user.PhoneNumber),
            new SqlParameter("@EmailAddress", user.EmailAddress),
            new SqlParameter("@UserName", user.UserName),
            new SqlParameter("@Password", user.Password),
            userIdPr,
            statusPr,
            msgPr
        };

        await ConnectDb.ExecuteNonQueryAsync("SP_AbpUsers_Register", CommandType.StoredProcedure, prs);

        if ((int)statusPr.Value != 0)
        {
            throw new UserFriendlyException((string)msgPr.Value);
        }
        var userId = userIdPr.Value == DBNull.Value || !long.TryParse(userIdPr.Value.ToString(), out var id) || id <= 0
            ? throw new UserFriendlyException("Không thể tạo user bằng Store Procedure SP_AbpUsers_Register.")
            : id;
        user.Id = userId;
        return user;
    }
 

    private void CheckForTenant()
    {
        if (!AbpSession.TenantId.HasValue)
        {
            throw new InvalidOperationException("Can not register host users!");
        }
    }

    private async Task<Tenant> GetActiveTenantAsync()
    {
        if (!AbpSession.TenantId.HasValue)
        {
            return null;
        }

        return await GetActiveTenantAsync(AbpSession.TenantId.Value);
    }

    private async Task<Tenant> GetActiveTenantAsync(int tenantId)
    {
        var tenant = await _tenantManager.FindByIdAsync(tenantId);
        if (tenant == null)
        {
            throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
        }

        if (!tenant.IsActive)
        {
            throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
        }

        return tenant;
    }

    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}
