using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using Pbt.Individual.Authorization.Users;
using Pbt.Individual.MultiTenancy;
using PBT.CacheService;
using System;
using System.Threading.Tasks;

namespace Pbt.Individual;

/// <summary>
/// Derive your application services from this class.
/// </summary>
public abstract class IndividualAppServiceBase : ApplicationService
{
    public TenantManager TenantManager { get; set; }

    public UserManager UserManager { get; set; }

    public readonly AppCacheService _cacheService;

    protected IndividualAppServiceBase(AppCacheService cacheService)
    {
        LocalizationSourceName = IndividualConsts.LocalizationSourceName;
        _cacheService = cacheService;
    }

    protected virtual async Task<User> GetCurrentUserAsync()
    { 
        var currentUserId = AbpSession.GetUserId();
        var cacheKey = $"user_{currentUserId}";
        if (_cacheService.TryGetCacheValue(cacheKey, out User cachedUser))
        {
            return cachedUser;
        }
        else
        {
            var user = await UserManager.FindByIdAsync(currentUserId.ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }
            _cacheService.SetCacheValue(cacheKey, user, TimeSpan.FromDays(30));
            return user;
        }
    }

    protected virtual async Task<Tenant> GetCurrentTenantAsync()
    {
        var tenantId = AbpSession.GetTenantId();
        var cacheKey = $"tenant_{tenantId}";

        if (_cacheService.TryGetCacheValue(cacheKey, out Tenant tenant))
        {
            return tenant;
        }
        else
        {
            tenant = await TenantManager.GetByIdAsync(tenantId);
            _cacheService.SetCacheValue(cacheKey, tenant, TimeSpan.FromDays(3));
            return tenant;
        }
    }

    //protected virtual Task<Tenant> GetCurrentTenantAsync()
    //{
    //    return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
    //}

    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}
