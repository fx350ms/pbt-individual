

namespace Pbt.Individual.Orders.Dto
{
    public class CreateOrderListDto
    {
        public long? CustomerId { get; set; }

        /// <summary>
        /// Mã vận đơn
        /// </summary>
        public string WaybillCodes { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
    }
}
