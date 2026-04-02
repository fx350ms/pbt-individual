using Abp.Application.Services;
using Pbt.Individual.Sessions.Dto;
using System.Threading.Tasks;

namespace Pbt.Individual.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
