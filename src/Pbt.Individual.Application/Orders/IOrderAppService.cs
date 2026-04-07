using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pbt.Individual.Orders.Dto;

namespace Pbt.Individual.Orders
{
  public interface IOrderAppService : IApplicationService
  {
    /// <summary>
    /// Tạo đơn hàng mới  
    /// </summary>
    Task<CreateWaybillListResultDto> CreateWaybillsAsync(CreateWaybillListDto input);

    /// <summary>
    /// Danh sách đơn hàng của khách hàng hiện tại
    /// </summary>
    Task<PagedResultDto<OrderDto>> GetCustomerOrdersAsync(OrderPagedResultRequestDto input);
  }

}