using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Microsoft.Extensions.Configuration;
using Pbt.Individual.Authorization.Roles;
using Pbt.Individual.Authorization.Users;
using Pbt.Individual.Configuration;
using Pbt.Individual.Localization;
using Pbt.Individual.MultiTenancy;
using Pbt.Individual.Timing;

namespace Pbt.Individual;

[DependsOn(typeof(AbpZeroCoreModule))]
public class IndividualCoreModule : AbpModule
{

    private readonly IConfiguration _configuration;

    public IndividualCoreModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = false;

        // Declare entity types
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);

        IndividualLocalizationConfigurer.Configure(Configuration.Localization);

        // Enable this line to create a multi-tenant application.
        Configuration.MultiTenancy.IsEnabled = IndividualConsts.MultiTenancyEnabled;

        // Configure roles
        AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

        Configuration.Settings.Providers.Add<AppSettingProvider>();

     //   Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));

        Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = IndividualConsts.DefaultPassPhrase;
        SimpleStringCipher.DefaultPassPhrase = IndividualConsts.DefaultPassPhrase;
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(IndividualCoreModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
    }
}
