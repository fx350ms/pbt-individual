using Abp.Modules;
using Abp.Reflection.Extensions;
using Pbt.Individual.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Pbt.Individual.Web.Host.Startup
{
    [DependsOn(
       typeof(IndividualWebCoreModule))]
    public class IndividualWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public IndividualWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IndividualWebHostModule).GetAssembly());
        }
    }
}
