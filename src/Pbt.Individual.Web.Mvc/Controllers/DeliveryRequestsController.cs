using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pbt.DeliveryRequests;
using pbt.Web.Models.Orders;
using Pbt.Individual.Controllers;
using Pbt.Individual.Orders;
using Pbt.Individual.Orders.Dto;
using Pbt.Individual.Packages;
using Pbt.Individual.Web.Models.Orders;
using System.Threading.Tasks;


namespace Pbt.Individual.Web.Controllers
{
    [Authorize]
    [AbpMvcAuthorize]
    public class DeliveryRequestsController : IndividualControllerBase
    {
        private readonly IDeliveryRequestAppService _deliveryRequestAppService;
        public DeliveryRequestsController(IDeliveryRequestAppService deliveryRequestAppService
        )
        {
            _deliveryRequestAppService = deliveryRequestAppService; 
        }

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
