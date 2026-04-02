using Abp.Authorization;
using Pbt.Individual.Authorization.Roles;
using Pbt.Individual.Authorization.Users;

namespace Pbt.Individual.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
