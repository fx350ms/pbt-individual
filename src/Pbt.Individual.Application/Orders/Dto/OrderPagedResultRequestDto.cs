using System;
using Abp.Application.Services.Dto;

namespace Pbt.Individual.Orders.Dto
{
    public class OrderPagedResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public int Status { get; set; } = -1;
        public int ShippingLine { get; set; } = -1;
        private static readonly string DateFormat = "dd-MM-yyyy HH:mm";

        // CreateDate
        #region Ngày tạo đơn hàng
        public string StartCreateDateStr { get; set; }
        public string EndCreateDateStr { get; set; }

        public DateTime? StartCreateDate
        {
            get
            {
                if (string.IsNullOrEmpty(StartCreateDateStr)) return null;
                if (DateTime.TryParseExact(StartCreateDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var startDate))
                {
                    return startDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        public DateTime? EndCreateDate
        {
            get
            {
                if (string.IsNullOrEmpty(EndCreateDateStr)) return null;
                if (DateTime.TryParseExact(EndCreateDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var endDate))
                {
                    return endDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }


        // Ngày nhập kho trung quốc 
        public string StartMatchDateStr { get; set; }
        public string EndMatchDateStr { get; set; }


        public DateTime? MatchStartDate
        {
            get
            {
                if (string.IsNullOrEmpty(StartMatchDateStr)) return null;
                if (DateTime.TryParseExact(StartMatchDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var startDate))
                {
                    return startDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        public DateTime? MatchEndDate
        {
            get
            {
                if (string.IsNullOrEmpty(EndMatchDateStr)) return null;
                if (DateTime.TryParseExact(EndMatchDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var endDate))
                {
                    return endDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        #endregion

        // Ngày xuất kho TQ
        public string StartExportDateStr { get; set; }
        public string EndExportDateStr { get; set; }

        public DateTime? ExportStartDate
        {
            get
            {
                if (string.IsNullOrEmpty(StartExportDateStr)) return null;
                if (DateTime.TryParseExact(StartExportDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var startDate))
                {
                    return startDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        public DateTime? ExportEndDate
        {
            get
            {
                if (string.IsNullOrEmpty(EndExportDateStr)) return null;
                if (DateTime.TryParseExact(EndExportDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var endDate))
                {
                    return endDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        // Ngày nhập kho VN

        public string StartImportDateStr { get; set; }
        public string EndImportDateStr { get; set; }

        public DateTime? ImportStartDate
        {
            get
            {
                if (string.IsNullOrEmpty(StartImportDateStr)) return null;
                if (DateTime.TryParseExact(StartImportDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var startDate))
                {
                    return startDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        public DateTime? ImportEndDate
        {
            get
            {
                if (string.IsNullOrEmpty(EndImportDateStr)) return null;
                if (DateTime.TryParseExact(EndImportDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var endDate))
                {
                    return endDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        // Ngày giao hàng

        public string StartDeliveryDateStr { get; set; }
        public string EndDeliveryDateStr { get; set; }

        public DateTime? DeliveryStartDate
        {
            get
            {
                if (string.IsNullOrEmpty(StartDeliveryDateStr)) return null;
                if (DateTime.TryParseExact(StartDeliveryDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var startDate))
                {
                    return startDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        public DateTime? DeliveryEndDate
        {
            get
            {
                if (string.IsNullOrEmpty(EndDeliveryDateStr)) return null;
                if (DateTime.TryParseExact(EndDeliveryDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var endDate))
                {
                    return endDate;
                }
                return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
            }
        }

        
        // // Ngày yêu cầu giao
        // public string RequestDeliveryStartDateStr { get; set; }
        // public string RequestDeliveryEndDateStr { get; set; }

        // public DateTime? RequestDeliveryStartDate
        // {
        //     get
        //     {
        //         if (string.IsNullOrEmpty(RequestDeliveryStartDateStr)) return null;
        //         if (DateTime.TryParseExact(RequestDeliveryStartDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var startDate))
        //         {
        //             return startDate;
        //         }
        //         return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
        //     }
        // }

        // public DateTime? RequestDeliveryEndDate
        // {
        //     get
        //     {
        //         if (string.IsNullOrEmpty(RequestDeliveryEndDateStr)) return null;
        //         if (DateTime.TryParseExact(RequestDeliveryEndDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var endDate))
        //         {
        //             return endDate;
        //         }
        //         return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
        //     }
        // }
    }
}