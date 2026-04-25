using Abp.Application.Services.Dto;
using System;

namespace Pbt.Individual.DeliveryRequests.Dto
{
    public class DeliveryRequestDto : FullAuditedEntityDto<int>
    {
        public string RequestCode { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; }
        public int Status { get; set; }
        public int ShippingMethod { get; set; }
        public long AddressId { get; set; }
        public string Note { get; set; }
        public decimal? Weight { get; set; }
        public int? TotalPackage { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentStatus { get; set; }
        public int PaymentMethod { get; set; }
        public int PackageCount { get; set; }

        public string Address { get; set; }

        public decimal TotalWeight { get; set; }

        public DateTime? RequestTime { get; set; }
    }
}
