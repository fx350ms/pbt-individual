using Abp.Application.Services;
using Pbt.Individual.Warehouses.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbt.Warehouses
{
    public interface IWarehouseAppService : IApplicationService
    {
        public Task<List<WarehouseNameAndCodeDto>> GetByCountryAsync(int countryId);
    }
}
