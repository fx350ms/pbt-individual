using System.Collections.Generic;
using Pbt.Individual.Web.ViewModels.DeliveryRequests;

namespace pbt.Web.Mvc.Models.DeliveryRequests
{
    
    public class DeliveryRequestPackagesByBagIdModel
    {
        public int BagId { get; set; }
        public List<PackageViewByBagDto> packageViewByBagDtos { get; set; }
    }
 
}