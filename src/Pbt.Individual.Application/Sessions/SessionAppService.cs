using Abp.Auditing;
using Microsoft.Extensions.Configuration;
using Pbt.Individual.Sessions.Dto;
using PBT.CacheService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pbt.Individual.Sessions;

public class SessionAppService : IndividualAppServiceBase, ISessionAppService
{
    private readonly IConfiguration _configuration;
    public SessionAppService(AppCacheService cacheService,
          IConfiguration configuration
        )
        : base(cacheService)
    {
        _configuration = configuration;
    }

    [DisableAuditing]
    public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
    {
        var output = new GetCurrentLoginInformationsOutput
        {
            Application = new ApplicationInfoDto
            {
                Version = AppVersionHelper.Version,
                ReleaseDate = AppVersionHelper.ReleaseDate,
                Features = new Dictionary<string, bool>()
            }
        };

        if (AbpSession.TenantId.HasValue)
        {
            output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
        }

        if (AbpSession.UserId.HasValue)
        {
            output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
        }

        return output;
    }
}
