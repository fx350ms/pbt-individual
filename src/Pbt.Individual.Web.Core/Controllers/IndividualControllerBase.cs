using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Pbt.Individual.Controllers
{
    public abstract class IndividualControllerBase : AbpController
    {
        protected IndividualControllerBase()
        {
            LocalizationSourceName = IndividualConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
