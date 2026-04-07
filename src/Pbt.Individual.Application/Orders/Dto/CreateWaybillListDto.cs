
using System.ComponentModel.DataAnnotations;

namespace Pbt.Individual.Orders.Dto
{
    public class CreateWaybillListDto
    {

        /// <summary>
        /// Mã vận đơn
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập mã vận đơn")]
        public string WaybillCodes { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Line vận chuyển
        /// </summary>
        public int ShippingLine { get; set; }

        /// <summary>
        ///  Sử dụng bảo hiểm
        /// </summary>
        public bool IsUseInsurance { get; set; }

        /// <summary>
        /// Sử dụng đóng gỗ
        /// </summary>
        public bool IsUseWoodenPackaging { get; set; }

        /// <summary>
        /// Sử dụng chống sốc
        /// </summary>
        public bool IsUseShockproofPackaging { get; set; }


        /// <summary>
        /// Sử dụng vận chuyển nội địa
        /// </summary>
        public bool IsUseDomesticTransportation { get; set; }
    }
}