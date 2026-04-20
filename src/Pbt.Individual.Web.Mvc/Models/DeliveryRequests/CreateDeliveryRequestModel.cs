using Pbt.Individual.Warehouses.Dto;
using System.Collections.Generic;

namespace Pbt.Individual.Web.Models.DeliveryRequests;

public class CreateUpdateDeliveryRequestModel
{
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public List<WarehouseDto> Warehouses { get; set; }
}
