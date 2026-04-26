using Abp.AspNetCore.Mvc.Authorization;
using Pbt.Individual.Controllers;
using Microsoft.AspNetCore.Mvc;
using Pbt.Individual.Packages;
using Pbt.Individual.Web.Mvc.Models.Home;

namespace Pbt.Individual.Web.Controllers;

[AbpMvcAuthorize]
public class HomeController : IndividualControllerBase
{

    private readonly IPackageAppService _packageAppService;
    public HomeController(IPackageAppService packageAppService)
    {
        _packageAppService = packageAppService;
    }

    public ActionResult Index()
    {
        var packageSummary = _packageAppService.GetPackageSummaryByStatusAsync().Result;
        var model = new IndexModel
        {
            PackageSummary = packageSummary
        };
        return View(model);
    }
}
