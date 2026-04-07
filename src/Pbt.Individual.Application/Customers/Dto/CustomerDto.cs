using Abp.Application.Services.Dto;
using System;

namespace Pbt.Individual.Warehouses.Dto
{
    public class CustomerDto : FullAuditedEntityDto<long>
    {
        public string CustomerCode { get; set; }    // Mã khách hàng
        public string FullName { get; set; }           // Họ và tên đầy đủ
        public string Email { get; set; }              // Email
        public string PhoneNumber { get; set; }        // Số điện thoại
        public string Address { get; set; }            // Địa chỉ
        public string AddressReceipt { get; set; }     // Địa chỉ nhận hàng
        public DateTime? DateOfBirth { get; set; }     // Ngày sinh
        public string Gender { get; set; }             // Giới tính
        public DateTime RegistrationDate { get; set; } // Ngày đăng ký
        public int Status { get; set; }             // Trạng thái
        public string Notes { get; set; }              // Ghi chú
        public string RefCode { get; set; }
        public int? PiorityStorageId { get; set; }
        public long? UserId { get; set; } // ID của tài khoản (User)
        public string Username { get; set; } // Tài khoản khách hàng
        public int? AddressId { get; set; }
        public int? AddressReceiptId { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal CurrentDebt { get; set; }
        public decimal MaxDebt { get; set; }

        public int VipLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// Tiềm năng khách hàng
        /// </summary>
        public int PotentialLevel { get; set; }

 
        /// <summary>
        /// Là Khách hàng đại lý
        /// </summary>
        public bool IsAgent { get; set; }

        /// <summary>
        /// Cấp của đại lý
        /// </summary>
        public int AgentLevel { get; set; }

        /// <summary>
        /// Khách hàng cha
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// Id của nhân viên kinh doanh
        /// </summary>
        public long SaleId { get; set; }
        
        public string BagPrefix { get; set; }
        // WarehouseId
        public int? WarehouseId { get; set; }

        public string WarehouseName { get; set; }


        public decimal? InsurancePercentage { get; set; }

        public string CreationTimeString => CreationTime.ToString("dd/MM/yyyy HH:mm");


        public string ParentUsername { get; set; }  
        public string SaleUsername { get; set; }
    }
}
