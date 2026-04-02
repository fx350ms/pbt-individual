using Abp.Authorization;
using Abp.Runtime.Session;
using Pbt.Individual.Configuration.Dto;
using PBT.CacheService;
using System.Threading.Tasks;

namespace Pbt.Individual.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : IndividualAppServiceBase, IConfigurationAppService
{

    public ConfigurationAppService(ConfigAppCacheService cacheService) : base(cacheService)
    {
    }

    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        
    }
}
