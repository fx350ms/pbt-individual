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
            Roles = new List<UserRole>()
        };

        user.SetNormalizedNames();
        user.Password = _passwordHasher.HashPassword(user, plainPassword);

        var createdUserId = await CreateUserByStoredProcedureAsync(user);
        var createdUser = await _userManager.GetUserByIdAsync(createdUserId);

        await _userManager.InitializeOptionsAsync(tenant.Id);

        foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
        {
            await _userManager.AddToRoleAsync(createdUser, defaultRole.Name);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return createdUser;
    }

    private async Task<long> CreateUserByStoredProcedureAsync(User user)
    {
        var userIdParameter = new SqlParameter("@UserId", SqlDbType.BigInt)
        {
            Direction = ParameterDirection.Output
        };

        var parameters = new[]
        {
            new SqlParameter("@TenantId", user.TenantId ?? (object)DBNull.Value),
            new SqlParameter("@UserName", user.UserName),
            new SqlParameter("@NormalizedUserName", user.NormalizedUserName),
            new SqlParameter("@Name", user.Name),
            new SqlParameter("@Surname", user.Surname),
            new SqlParameter("@EmailAddress", user.EmailAddress),
            new SqlParameter("@NormalizedEmailAddress", user.NormalizedEmailAddress),
            new SqlParameter("@PhoneNumber", (object?)user.PhoneNumber ?? DBNull.Value),
            new SqlParameter("@Password", user.Password),
            new SqlParameter("@SecurityStamp", (object?)user.SecurityStamp ?? Guid.NewGuid().ToString("N")),
            new SqlParameter("@ConcurrencyStamp", (object?)user.ConcurrencyStamp ?? Guid.NewGuid().ToString()),
            new SqlParameter("@IsActive", user.IsActive),
            new SqlParameter("@IsEmailConfirmed", user.IsEmailConfirmed),
            new SqlParameter("@IsLockoutEnabled", user.IsLockoutEnabled),
            new SqlParameter("@IsPhoneNumberConfirmed", user.IsPhoneNumberConfirmed),
            new SqlParameter("@IsTwoFactorEnabled", user.IsTwoFactorEnabled),
            new SqlParameter("@CreationTime", user.CreationTime),
            new SqlParameter("@IsDeleted", user.IsDeleted),
            new SqlParameter("@AccessFailedCount", user.AccessFailedCount),
            userIdParameter
        };

        await ConnectDb.ExecuteNonQueryAsync("SP_Users_CreateByRegistration", CommandType.StoredProcedure, parameters);

        if (userIdParameter.Value == DBNull.Value || !long.TryParse(userIdParameter.Value.ToString(), out var userId) || userId <= 0)
        {
            throw new UserFriendlyException("Không thể tạo user bằng Store Procedure SP_Users_CreateByRegistration.");
        }

        return userId;
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
