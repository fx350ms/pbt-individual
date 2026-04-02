using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Pbt.Individual.Configuration;
using Pbt.Individual.EntityFrameworkCore;
using Pbt.Individual.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace Pbt.Individual.Migrator;

[DependsOn(typeof(IndividualEntityFrameworkModule))]
public class IndividualMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public IndividualMigratorModule(IndividualEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(IndividualMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            IndividualConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(IndividualMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
