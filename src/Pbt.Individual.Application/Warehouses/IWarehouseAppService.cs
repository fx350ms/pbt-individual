using Abp.Application.Services;
using Pbt.Individual.Warehouses.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pbt.Individual.Warehouses
{
    public interface IWarehouseAppService : IApplicationService
    {
        Task<List<WarehouseDto>> GetByTypeAsync(int warehouseType);
    }
}
