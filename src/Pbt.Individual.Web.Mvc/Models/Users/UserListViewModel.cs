using Pbt.Individual.Roles.Dto;
using System.Collections.Generic;

namespace Pbt.Individual.Web.Models.Users;

public class UserListViewModel
{
    public IReadOnlyList<RoleDto> Roles { get; set; }
}
