using Abp.Zero.EntityFrameworkCore;
using Pbt.Individual.Authorization.Roles;
using Pbt.Individual.Authorization.Users;
using Pbt.Individual.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Pbt.Individual.EntityFrameworkCore;

public class IndividualDbContext : AbpZeroDbContext<Tenant, Role, User, IndividualDbContext>
{
    /* Define a DbSet for each entity of the application */

    public IndividualDbContext(DbContextOptions<IndividualDbContext> options)
        : base(options)
    {
    }
}
