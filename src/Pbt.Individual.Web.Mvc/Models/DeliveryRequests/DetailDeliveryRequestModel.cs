using Pbt.Individual.DeliveryRequests.Dto;

namespace Pbt.Individual.Web.Models.DeliveryRequests;

public class DetailDeliveryRequestModel
{
    public long DeliveryRequestId { get; set; }
    public DeliveryRequestDto deliveryRequestDto { get; set; }
}
