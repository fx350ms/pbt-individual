using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Pbt.Individual.EntityFrameworkCore;
using Pbt.Individual.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Pbt.Individual.Web.Tests;

[DependsOn(
    typeof(IndividualWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class IndividualWebTestModule : AbpModule
{
    public IndividualWebTestModule(IndividualEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(IndividualWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(IndividualWebMvcModule).Assembly);
    }
}