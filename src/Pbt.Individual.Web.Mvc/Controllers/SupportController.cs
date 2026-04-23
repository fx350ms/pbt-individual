using Microsoft.AspNetCore.Mvc;
using Pbt.Individual.Controllers;

namespace Pbt.Individual.Web.Mvc.Controllers
{
    public class SupportController : IndividualControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}