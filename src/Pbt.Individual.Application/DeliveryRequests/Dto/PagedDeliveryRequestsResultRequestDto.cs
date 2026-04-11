using Abp.Application.Services.Dto;
using System;
namespace Pbt.Individual.Application.DeliveryRequests.Dto;

public class PagedDeliveryRequestsResultRequestDto : PagedResultRequestDto
{
    public string Code { get; set; } = "";
    public int Status { get; set; } = -1;
    public string StartDateStr { get; set; }
    public string EndDateStr { get; set; }
    public long CustomerId { get; set; } = -1;

    private static readonly string DateFormat = "dd-MM-yyyy";
    public DateTime? StartDate
    {
        get
        {
            if (string.IsNullOrEmpty(StartDateStr)) return null;
            if (DateTime.TryParseExact(StartDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var startDate))
            {
                return startDate;
            }
            return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
        }
    }

    public DateTime? EndDate
    {
        get
        {
            if (string.IsNullOrEmpty(EndDateStr)) return null;
            if (DateTime.TryParseExact(EndDateStr, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var endDate))
            {
                return endDate;
            }
            return null; // Hoặc bạn có thể trả về một giá trị mặc định nếu cần
        }
    }

}