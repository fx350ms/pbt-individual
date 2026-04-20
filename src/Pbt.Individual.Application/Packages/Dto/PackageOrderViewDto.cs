using System;
using Abp.Application.Services.Dto;

namespace Pbt.Individual.Packages.Dto
{


    public class PackageOrderViewDto : FullAuditedEntityDto<int>
    {
        public string ProductNameVi { get; set; }

        public string ProductNameCn { get; set; }

        public string TrackingNumber { get; set; }
        public long OrderId { get; set; }
        public string PackageNumber { get; set; }

        public string BagNumber
        {
            get; set;
        }

        public int? ShippingPartnerId { get; set; }
        public int? ShippingLineId { get; set; }



        public int? ProductGroupTypeId { get; set; }
        public int? CategoryId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
        public bool IsPriceUpdate { get; set; } = false;
        public string WeightUpdateReason { get; set; }
        public decimal? PriceCN { get; set; }

        public decimal? TotalFee { get; set; }

        public int? Length { get; set; } // cm
        public int? Width { get; set; } // cm
        public int? Height { get; set; } // cm
        public decimal? Dimention
        {
            get { return (decimal?)(Length * Width * Height) / 1000000; }
        }

        public decimal? Weight { get; set; } // kg
        public string WeightString => Weight.HasValue ? Weight.Value.ToString("0.##") : string.Empty;
        public decimal? Volume { get; set; } // thể tích 
        public bool IsInsured { get; set; }
        public bool IsWoodenCrate { get; set; }
        public long? WoodenPackingId { get; set; } // nhóm đóng gỗ chung id
        public bool IsShockproof { get; set; }
        public bool? IsDomesticShipping { get; set; }
        public decimal? DomesticShippingFee { get; set; }
        public decimal? DomesticShippingFeeRMB { get; set; }
        public DateTime? MatchTime { get; set; } // thời gian khớp

        public string MatchTimeFormat
        {
            get { return MatchTime is not null ? MatchTime?.ToString("dd/MM/yyyy HH:mm") : null; }
        } // thời gian xuất kho

        public DateTime? DeliveryTime { get; set; } // thời gian xuất kho

        public DateTime? ExportDate { get; set; } // thời gian xuất kho ở TQ
        public DateTime? ImportDate { get; set; } // thời gian nhập kho ở VN
        public DateTime? BaggingDate { get; set; } // Thời gian được đưa vào bao
        public DateTime? TransferWarehouseTime { get; set; } // Thời gian chuyển kho
        public DateTime? LastUnbagTime { get; set; } // Thời gian bỏ bao lần cuối

        public int WarehouseStatus { get; set; } // trạng thái kho


        public int ShippingStatus { get; set; } // trạng thái vận chuyên


        public DateTime CreationTime { get; set; } // thời gian tạo
        public string CreationTimeFormat => CreationTime.ToString("dd/MM/yyyy HH:mm"); // thời gian tạo

        /// <summary>
        /// Kho hiện tại
        /// </summary>
        public int? WarehouseId { get; set; }

        /// <summary>
        /// Kiện lỗi
        /// </summary>
        public bool IsDefective { get; set; }
        /// <summary>
        /// Giá trị bảo hiểm
        /// </summary>
        public decimal InsuranceValue { get; set; }
        public string Note { get; set; }

        public decimal? UnitPrice { get; set; }  // giá vận chuyển trên 1 kg
        /// <summary>
        /// Tổng giá trị hàng hóa, bao gồm cả phí vận chuyển, bảo hiểm, đóng gói, v.v.
        /// </summary>
        public decimal? TotalPrice { get; set; } // Tổng tất cả chi phí
        public decimal? ShockproofFee { get; set; } // phí quấn bọt khí
        public decimal? InsuranceFee { get; set; } // phí bảo hiểm
        public decimal? WoodenPackagingFee { get; set; } // phí đóng gỗ

        public int? BagId { get; set; }
        public int? LastBagId { get; set; } // bao cuối cùng kiện nằm trong đó


        public string ShippingPartnerName { get; set; }



        public string OrderCode { get; set; }

        public virtual string WaybillCodeFake { get; set; }
        public long? ParentId { get; set; }

        public long? costPerKg { get; set; } // giá trên kg
        public long? CustomerId { get; set; }

        public long? CustomerFakeId { get; set; }

        public string CustomerFakeName { get; set; }
        public string CustomerFakePhone { get; set; }
        public string CustomerFakeAddress { get; set; }
        public int? DeliveryNoteId { get; set; } // phiếu xuất kho
        public string DeliveryNoteCode { get; set; } // mã phiếu xuất kho

        public bool? IsQuickBagging { get; set; }

        public decimal? OriginShippingCost { get; set; } // 1: KG, 2: M3

        public string WarehouseName { get; set; }


        /// <summary>
        /// Đại diện cho bao để hiển thị cân nặng bì của bao
        /// </summary>
        /// 
        public bool IsRepresentForWeightCover { get; set; }

        public int? WarehouseCreateId { get; set; } // Kho tạo
        public string WarehouseCreateName { get; set; } // Tên kho tạo

        public int? WarehouseDestinationId { get; set; } // Kho đích
        public string WarehouseDestinationName { get; set; } // Tên kho đích

        public decimal? CostFee { get; set; } // Giá vốn 
        public int UnitType { get; set; } // 1: KG, 2: M3

        /// <summary>
        /// Mã vận đơn
        /// </summary>
        public string WaybillNumber { get; set; }


        public string CustomerName { get; set; }

    }
}