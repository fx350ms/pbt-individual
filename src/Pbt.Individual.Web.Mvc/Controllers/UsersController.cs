using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Configuration.Startup;
using Abp.Runtime.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pbt.Individual.ApplicationUtils;
using Pbt.Individual.Controllers;
using Pbt.Individual.Customers;
using Pbt.Individual.Users;
using Pbt.Individual.Warehouses;
using Pbt.Individual.Web.Models.Users;

namespace Pbt.Individual.Web.Mvc.Controllers
{
    public class UsersController : IndividualControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly ICustomerAppService _customerAppService;

        private readonly string[] _roles;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IWarehouseAppService _warehouseAppService;

        public UsersController(IUserAppService userAppService,
            ICustomerAppService customerAppService,
            IWarehouseAppService warehouseAppService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userAppService = userAppService;
            _customerAppService = customerAppService;
            _warehouseAppService = warehouseAppService;
            _roles = httpContextAccessor.HttpContext.User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role) // Lọc claims loại "role"
                .Select(c => c.Value)
                .ToArray();
        }
        //
        // public async Task<ActionResult> Index()
        // {
        //     var roles = (await _userAppService.GetRoles()).Items;
        //     var warehouses = await _warehouseAppService.GetByTypeAsync("VI");
        //     // remove admin role from roles if _roles not admin
        //     if (!_roles.Contains("admin"))
        //     {
        //         roles = roles.Where(r => r.NormalizedName != "ADMIN" && r.NormalizedName != "SALEADMIN").ToList();
        //     }
        //     var saleUsers = await _userAppService.GetUsersSaleForLookupByCurrentUser();
        //     var model = new UserListViewModel
        //     {
        //         Roles = roles,
        //         Warehouses = warehouses,
        //     };
        //     return View(model);
        // }
        //
        // public async Task<ActionResult> EditModal(long userId)
        // {
        //     var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
        //     var roles = (await _userAppService.GetRoles()).Items;
        //     var warehouses = await _warehouseAppService.GetFull();
        //     var model = new EditUserModalViewModel
        //     {
        //         User = user,
        //         Roles = roles,
        //         Warehouses = warehouses
        //     };
        //     return PartialView("_EditModal", model);
        // }

        public ActionResult ChangePassword()
        {
            return View();
        }
        
        public async Task<ActionResult> UpdateInformation()
        {
            var model = new UpdateViewModel();
            var warehouses = await _warehouseAppService.GetByTypeAsync((int)WarehouseType.Destination); // Assuming 1 is the type for warehouses in Vietnam
            ViewBag.Warehouses = warehouses;
            var cnWarehouses = await _warehouseAppService.GetByTypeAsync((int)WarehouseType.Source); // Assuming 2 is the type for warehouses in China
            ViewBag.CNWarehouses = cnWarehouses;
            var userId = User.Identity.GetUserId().Value; // Get current user ID
            var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
            model.Name = user.Name;
            model.PhoneNumber = user.PhoneNumber;
            model.EmailAddress = user.EmailAddress;
            model.CNWarehouseId = user.WarehouseId;
            var customer = await _customerAppService.GetAsync(user.CustomerId ?? 0);
            model.WarehouseId = (int)customer.WarehouseId;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateInformation(UpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var warehouses = await _warehouseAppService.GetByTypeAsync((int)WarehouseType.Destination);
                    ViewBag.Warehouses = warehouses;
                    var cnWarehouses = await _warehouseAppService.GetByTypeAsync((int)WarehouseType.Source);
                    ViewBag.CNWarehouses = cnWarehouses;
                    return View(model);
                }

                var userId = User.Identity.GetUserId().Value;
                var result = await _userAppService.UpdateUserInfoAsync(
                    userId,
                    model.Name,
                    model.PhoneNumber,
                    model.EmailAddress,
                    model.WarehouseId,
                    model.CNWarehouseId
                );
                if (result)
                {
                    TempData["Alerts"] = L("UpdateSuccess");
                    return RedirectToAction("UpdateInformation");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var warehousesError = await _warehouseAppService.GetByTypeAsync((int)WarehouseType.Destination);
            ViewBag.Warehouses = warehousesError;
            var cnWarehousesError = await _warehouseAppService.GetByTypeAsync((int)WarehouseType.Source);
            ViewBag.CNWarehouses = cnWarehousesError;
            return View(model);
        }

        // public ActionResult ResetPassword(long userId)
        // {
        //     return PartialView("_ResetPassword", new ResetUserPasswordDto() { UserId = userId });
        // }

        // public async Task<IActionResult> Information()
        // {
        //     var userId = User.Identity.GetUserId().Value; //
        //     var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
        //     var customer = await _customerAppService.GetByUserId(user.Id);
        //
        //     var model = new UserInformationViewModel()
        //     {
        //         Customer = customer,
        //         User = user
        //     };
        //     return View(model);
        // }


    }
}
