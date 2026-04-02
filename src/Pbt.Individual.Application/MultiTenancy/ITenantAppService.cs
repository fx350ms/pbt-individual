using Abp.Application.Services;
using Pbt.Individual.MultiTenancy.Dto;

namespace Pbt.Individual.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

