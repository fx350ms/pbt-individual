using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pbt.Individual.Packages;
using System.Collections.Generic;
using Pbt.Individual.Packages.Dto;

namespace Pbt.Individual.Packages
{
    public interface IPackageAppService : IApplicationService
    {
        Task<List<PackageOrderViewDto>>  GetByOrderIdAsync(long orderId);
        
    }
}