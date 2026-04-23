using System.Collections.Generic;
using Abp.Application.Services;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Pbt.Individual.DeliveryRequests.Dto;
using Pbt.Individual.Application.DeliveryRequests.Dto;

namespace pbt.DeliveryRequests
{
    public interface IDeliveryRequestAppService : IApplicationService
    {
        Task<DeliveryRequestDto> GetByIdAsync(int id);
        Task<DeliveryRequestDto> CreateAsync(CreateDeliveryRequestDto input);
        Task<PagedResultDto<DeliveryRequestDto>> GetPagedAsync(PagedDeliveryRequestsResultRequestDto input);
        Task<JsonResult> AddItemAsync(DeliveryRequestItemDto item);
        Task<JsonResult> DeleteItemAsync(int deliveryRequestItemId);
        Task<JsonResult> SubmitAsync(SubmitDeliveryRequestDto input);
        Task<List<DeliveryRequestItemDto>> GetItemsByRequestIdAsync(int deliveryRequestId);
    }
}

