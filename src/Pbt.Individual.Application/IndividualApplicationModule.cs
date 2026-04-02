    using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Pbt.Individual.Authorization;
using Pbt.Individual.Sessions;
using PBT.CacheService;

namespace Pbt.Individual;

[DependsOn(
    typeof(IndividualCoreModule),
    typeof(AbpAutoMapperModule))]
public class IndividualApplicationModule : AbpModule
{
    public override void PreInitialize()
    {

        Configuration.Authorization.Providers.Add<IndividualAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(IndividualApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);
        //IocManager.Register<ISessionAppService, SessionAppService>();
        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );


        IocManager.Register<ConfigAppCacheService>();
    }
}
