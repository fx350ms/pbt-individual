using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using Pbt.Individual.Authorization.Accounts.Dto;


namespace pbt.Customers
{
    /// <summary>
    /// Interface for Customer Application Service, providing methods for managing customer data.
    /// </summary>
    public interface ICustomerAppService : IApplicationService
    {
        Task<long> CreateFromRegistrationAsync(CreateCustomerDto input);
        Task SynchronizeCustomerWithUserAsync(long customerId, string username);
    }
}
