using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pbt.DeliveryRequests;
using Pbt.Individual.ApplicationUtils;
using Pbt.Individual.Controllers;
using Pbt.Individual.Warehouses;
using Pbt.Individual.Web.Models.DeliveryRequests;
using System.Threading.Tasks;

namespace Pbt.Individual.Web.Controllers
{
    [Authorize]
    [AbpMvcAuthorize]
    public class DeliveryRequestsController : IndividualControllerBase
    {
        private readonly IDeliveryRequestAppService _deliveryRequestAppService;
        private readonly IWarehouseAppService _warehouseAppService;
        public DeliveryRequestsController(IDeliveryRequestAppService deliveryRequestAppService,
                                          IWarehouseAppService warehouseAppService)
        {
            _deliveryRequestAppService = deliveryRequestAppService;
            _warehouseAppService = warehouseAppService;
        }

        public ActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Create()
        {

            var warehouses = await _warehouseAppService.GetByTypeAsync((int)WarehouseType.Destination);
            // Kiểm tra xem có yêu cầu giao nào đang chờ xử lý không, nếu có thì lấy ycg đó lên để hiển thị thông tin
            var model = new CreateUpdateDeliveryRequestModel
            {
                Warehouses = warehouses
            };
            return View(model);
        }
    }
}
