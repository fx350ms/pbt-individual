using System;

namespace Pbt.Individual.DeliveryRequests.Dto
{
    public class CreateDeliveryRequestItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string PackageCode { get; set; }
        public string BagNumber { get; set; }
        public int ItemType { get; set; } // 1: Kiện, 2: Bao
        public decimal Weight { get; set; }

        public int TotalPackages { get; set; }
        public string WaybillNumber { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime? ImportDate { get; set; }

    }
}
