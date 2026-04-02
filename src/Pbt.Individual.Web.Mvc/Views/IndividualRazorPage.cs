using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Pbt.Individual.Web.Views;

public abstract class IndividualRazorPage<TModel> : AbpRazorPage<TModel>
{
    [RazorInject]
    public IAbpSession AbpSession { get; set; }

    protected IndividualRazorPage()
    {
        LocalizationSourceName = IndividualConsts.LocalizationSourceName;
    }
}
