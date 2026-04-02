using Pbt.Individual.Roles.Dto;
using System.Collections.Generic;

namespace Pbt.Individual.Web.Models.Roles;

public class RoleListViewModel
{
    public IReadOnlyList<PermissionDto> Permissions { get; set; }
}
