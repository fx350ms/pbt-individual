using System.Collections.Generic;
using Abp.Application.Services;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using DeliveryRequestDto = Pbt.Individual.DeliveryRequests.Dto.DeliveryRequestDto;
using SubmitDeliveryRequestDto = Pbt.Individual.DeliveryRequests.Dto.SubmitDeliveryRequestDto;
using DeliveryRequestItemDto = Pbt.Individual.Application.DeliveryRequests.Dto.DeliveryRequestItemDto;
using PagedDeliveryRequestsResultRequestDto = Pbt.Individual.Application.DeliveryRequests.Dto.PagedDeliveryRequestsResultRequestDto;
 

namespace pbt.DeliveryRequests
{
    public interface IDeliveryRequestAppService : IApplicationService
    {
        Task<DeliveryRequestDto> GetByIdAsync(int id);
        Task<DeliveryRequestDto> CreateAsync();
        Task<PagedResultDto<DeliveryRequestDto>> GetPagedAsync(PagedDeliveryRequestsResultRequestDto input);
        Task<JsonResult> AddItemAsync(DeliveryRequestItemDto item);
        Task<JsonResult> DeleteItemAsync(int deliveryRequestItemId);
        Task<JsonResult> SubmitAsync(SubmitDeliveryRequestDto input);
        Task<List<DeliveryRequestItemDto>> GetItemsByRequestIdAsync(int deliveryRequestId);
    }
}

