using Abp.Application.Services;
using Pbt.Individual.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace Pbt.Individual.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
