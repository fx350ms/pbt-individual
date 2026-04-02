using Abp.AutoMapper;
using Pbt.Individual.Roles.Dto;
using Pbt.Individual.Web.Models.Common;

namespace Pbt.Individual.Web.Models.Roles;

[AutoMapFrom(typeof(GetRoleForEditOutput))]
public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
{
    public bool HasPermission(FlatPermissionDto permission)
    {
        return GrantedPermissionNames.Contains(permission.Name);
    }
}
