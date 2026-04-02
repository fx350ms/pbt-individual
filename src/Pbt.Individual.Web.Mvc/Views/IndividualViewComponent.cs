using Abp.AspNetCore.Mvc.ViewComponents;

namespace Pbt.Individual.Web.Views;

public abstract class IndividualViewComponent : AbpViewComponent
{
    protected IndividualViewComponent()
    {
        LocalizationSourceName = IndividualConsts.LocalizationSourceName;
    }
}
