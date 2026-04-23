using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pbt.Individual.Orders.Dto;
using Pbt.Individual.Packages.Dto;

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

    Task<OrderDto> GetAsync(long id);

    Task<OrderDetailDto> GetDetailAsync(long id);

    Task<(OrderDetailDto, List<PackageOrderViewDto>)> GetDetailWithPackagesAsync(long id);
    /// <summary>
    /// Tạo đơn hàng nhanh
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<long> CreateQuickOrderAsync(CreateQuickOrderDto input);
  }

}