using System.ComponentModel;

namespace Pbt.Individual.ApplicationUtils
{


    public enum WarehouseType
    {
        /// <summary>
        /// Kho nguồn tạo (Trung Quốc)
        /// </summary>
        Source = 1,


        /// <summary>
        /// Kho đích nhận (Việt Nam) 
        /// </summary>
        Destination = 2

    }




    public enum CustomerType
    {
        [Description("Khách đại lý")]
        Agent = 1,
        [Description("Khách hàng lẻ")]
        Individual = 2
    }

    public enum ShippingLineEnum
    {
        [Description("Hàng lô")]
        Consignment = 1,

        [Description("TMĐT")]
        Ecommerce = 2,
        [Description("Chính ngạch")]
        Official = 3,
        [Description("Xách tay")]
        Portable = 4
    }


    public enum OrderShippingStatusEnum
    {
        /// <summary>
        /// Đã ký gửi, thiếu thông tin
        /// </summary>
        [Description("Đã ký gửi, thiếu thông tin")]
        New = 0,
        /// <summary>
        /// Đã ký gửi
        /// </summary>
        [Description("Đã ký gửi")]
        Sent = 1,

        /// <summary>
        /// Hàng về kho TQ
        /// </summary>
        [Description("Hàng về kho TQ")]
        InChinaWarehouse = 2,

        /// <summary>
        /// Đang vận chuyển quốc tế
        /// </summary>
        [Description("Đang vận chuyển quốc tế")]
        InTransit = 3,

        /// <summary>
        /// Đã đến kho VN
        /// </summary>
        [Description("Đã đến kho VN")]
        InVietnamWarehouse = 4,

        /// <summary>
        /// Đang giao đến khách
        /// </summary>
        [Description("Đang giao đến khách")]
        OutForDelivery = 5,

        /// <summary>
        /// Đã giao
        /// </summary>
        [Description("Đã giao")]
        Delivered = 6,

        /// <summary>
        /// Khiếu nại
        /// </summary>
        [Description("Khiếu nại")]
        Complaint = 7,


        /// <summary>
        /// Hoàn tiền
        /// </summary>
        [Description("Hoàn tiền")]
        Refund = 8,

        /// <summary>
        /// Huỷ
        /// </summary>
        [Description("Huỷ")]
        Cancelled = 9,

        /// <summary>
        /// Hoàn thành đơn
        /// </summary>
        [Description("Hoàn thành đơn")]
        OrderCompleted = 10,

        [Description("Trung chuyển")]
        WarehouseTransfer = 13 // Chuyển kho
    }



    public enum PackageShippingStatusEnum
    {
        [Description("Thiếu thông tin")] MissingInfo = 0,
        /// <summary>
        /// Khởi tạo, đã kí gửi
        /// </summary>
        [Description("Tạo mới")] Initiate = 1,
        /// <summary>
        /// Đang ở kho QT, chờ vc về VN
        /// </summary>
        [Description("Chờ vận chuyển")] WaitingForShipping = 3,
        /// <summary>
        /// Đang vận chuyển về VN
        /// </summary>
        [Description("Đang vận chuyển")] Shipping = 4,
        /// <summary>
        /// 
        /// </summary>
        [Description("Đã về kho VN")] InWarehouseVN = 5,
        /// <summary>
        /// Chờ giao ở VN
        /// </summary>
        [Description("Chờ giao")] WaitingForDelivery = 6,

        /// <summary>
        /// Yêu cầu giao
        /// </summary>
        [Description("Yêu cầu giao")] DeliveryRequest = 7,

        /// <summary>
        /// Đang giao
        /// </summary>
        [Description("Đang giao")] DeliveryInProgress = 8,

        /// <summary>
        /// Đã giao
        /// </summary>
        [Description("Đã giao")] Delivered = 9,
        [Description("Hoàn thành")] Completed = 10, // Hoàn thành
        [Description("Khiếu nại")] Complaint = 11, // Khiếu nại

        [Description("Trung chuyển")]
        WarehouseTransfer = 13 // Chuyển kho

    }
}