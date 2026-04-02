using Abp.AspNetCore.Mvc.Authorization;
using Pbt.Individual.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Pbt.Individual.Web.Controllers;

[AbpMvcAuthorize]
public class HomeController : IndividualControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}
