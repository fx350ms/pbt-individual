using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pbt.DeliveryRequests;
using pbt.Web.Mvc.Models.DeliveryRequests;
using Pbt.Individual.ApplicationUtils;
using Pbt.Individual.Controllers;
using Pbt.Individual.Customers;
using Pbt.Individual.DeliveryRequests.Dto;
using Pbt.Individual.Packages;
using Pbt.Individual.Users;
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
        private readonly IPackageAppService _packageAppService;
        private readonly ICustomerAppService _customerAppService;
        private readonly IUserAppService _userAppService;
        public DeliveryRequestsController(
            IDeliveryRequestAppService deliveryRequestAppService,
            IWarehouseAppService warehouseAppService,
            IPackageAppService packageAppService,
            ICustomerAppService customerAppService,
            IUserAppService userAppService
            )
        {
            _deliveryRequestAppService = deliveryRequestAppService;
            _warehouseAppService = warehouseAppService;
            _packageAppService = packageAppService;
            _customerAppService = customerAppService;
            _userAppService = userAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(int deliveryRequestId = 0)
        {
            var deliveryRequest = await _deliveryRequestAppService.GetByIdAsync(deliveryRequestId);
            var user = await _userAppService.GetAsync(new EntityDto<long>((long)AbpSession.UserId));
            var customer = await _customerAppService.GetAsync(user.CustomerId.Value);
             if (deliveryRequest != null && deliveryRequest.CustomerId  != customer.Id)
            {
                return NotFound();
            }
            var warehouses = await _warehouseAppService.GetByTypeAsync((int)WarehouseType.Destination);
            var model = new CreateUpdateDeliveryRequestModel
            {
                Warehouses = warehouses,
                WarehouseId = deliveryRequestId == 0 ? 0 : deliveryRequest.WarehouseId,
            };
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var deliveryRequest = await _deliveryRequestAppService.GetByIdAsync(id);
            var model = new DetailDeliveryRequestModel
            {
                DeliveryRequestId = id,
                deliveryRequestDto = deliveryRequest
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPackagesByBagId(int bagId)
        {
            var model = new DeliveryRequestPackagesByBagIdModel()
            {
                BagId = bagId,
                packageViewByBagDtos = await _packageAppService.GetAllPackagesListByBagIdAsync(bagId)
            };
            return PartialView("_PackagesByBagId", model);
        }

    }
}
