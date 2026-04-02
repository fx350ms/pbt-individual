using Abp.Modules;
using Abp.Reflection.Extensions;
using Pbt.Individual.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Pbt.Individual.Web.Startup;

[DependsOn(typeof(IndividualWebCoreModule))]
public class IndividualWebMvcModule : AbpModule
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfigurationRoot _appConfiguration;

    public IndividualWebMvcModule(IWebHostEnvironment env)
    {
        _env = env;
        _appConfiguration = env.GetAppConfiguration();
    }

    public override void PreInitialize()
    {
        Configuration.Navigation.Providers.Add<IndividualNavigationProvider>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(IndividualWebMvcModule).GetAssembly());
    }
}
