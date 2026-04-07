using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pbt.Individual.Controllers;
using Pbt.Individual.Web.Models.Orders;
using Pbt.Individual.Orders.Dto;
using pbt.Web.Models.Orders;
using Pbt.Individual.Orders;
using System.Threading.Tasks;


namespace Pbt.Individual.Web.Controllers
{
    [Authorize]
    [AbpMvcAuthorize]
    public class OrdersController : IndividualControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        public OrdersController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

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

        public async Task<IActionResult> Detail(long id)
        {
            var (orderDto, packages) = await _orderAppService.GetDetailWithPackagesAsync(id);
            var model = new OrderDetailModel
            {
                Dto =  orderDto,
                Packages = packages
            };
            return View(model);
        }
    }
}
