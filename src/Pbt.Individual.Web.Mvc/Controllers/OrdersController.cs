using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pbt.Individual.Controllers;


namespace Pbt.Individual.Web.Controllers
{
    [Authorize]
    [AbpMvcAuthorize]
    public class OrdersController : IndividualControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}
