using Pbt.Individual.Roles.Dto;
using System.Collections.Generic;

namespace Pbt.Individual.Web.Models.Common;

public interface IPermissionsEditViewModel
{
    List<FlatPermissionDto> Permissions { get; set; }
}