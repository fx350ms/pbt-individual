using System.Collections.Generic;
using Pbt.Individual.Orders.Dto;
using Pbt.Individual.Packages.Dto;
namespace pbt.Web.Models.Orders
{
    public class OrderDetailModel
    {
        public OrderDetailDto Dto { get; set; }
        
        public List<PackageOrderViewDto> Packages { get; set; }
    }
}
