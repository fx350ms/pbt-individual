using System;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pbt.Individual.Controllers;
using Pbt.Individual.Packages;
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
        private readonly IPackageAppService _packageAppService;
        public OrdersController(IOrderAppService orderAppService,
         IPackageAppService packageAppService
        )
        {
            _orderAppService = orderAppService;
            _packageAppService = packageAppService;
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
                Dto = orderDto,
                Packages = packages
            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> GetPackagesTrackingByOrder(long orderId)
        {
            var packages = await _packageAppService.GetByOrderIdAsync(orderId);
            return PartialView("_PackageListDetailByOrder", packages);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> N8nCreateQuickOrder([FromBody] CreateQuickOrderDto input)
        {
            // 1. Kiểm tra API Key cố định
            var apiKey = Request.Headers["X-API-KEY"].ToString();
            if (apiKey != "MA_BI_MAT_CUA_BAN")
            {
                return Unauthorized("Không có quyền truy cập");
            }
            try
            {
                var orderId = await _orderAppService.CreateQuickOrderAsync(input);
                return Json(new { success = true, orderId = orderId, message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
