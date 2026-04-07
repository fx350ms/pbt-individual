
using System.Collections.Generic;
using Pbt.Individual.Orders.Dto;
namespace pbt.Web.Models.Orders
{
    public class OrderDetailModel
    {
        public OrderDto Dto { get; set; }
        
        public List<PackageOrderViewDto> Packages { get; set; }
    }
}
