using Abp.MultiTenancy;
using Pbt.Individual.Authorization.Users;

namespace Pbt.Individual.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
    }

    public Tenant(string tenancyName, string name)
        : base(tenancyName, name)
    {
    }
}
