using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pbt.Individual.Controllers;
using Pbt.Individual.Web.Models.Orders;
using Pbt.Individual.Orders.Dto;


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
            var model = new CreateOrderModel
            {
                Dto = new CreateWaybillListDto()
                {
                    WaybillCodes = ""
                }
            };
            return View(model);
        }
    }
}
