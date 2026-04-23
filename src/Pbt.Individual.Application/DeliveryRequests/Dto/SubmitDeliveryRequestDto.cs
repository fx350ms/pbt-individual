namespace Pbt.Individual.DeliveryRequests.Dto
{
    public class SubmitDeliveryRequestDto
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public int ShippingMethod { get; set; }
        public string Address { get; set; }
    }
}
