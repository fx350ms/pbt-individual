using Abp.AutoMapper;
using Pbt.Individual.Sessions.Dto;

namespace Pbt.Individual.Web.Views.Shared.Components.TenantChange;

[AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
public class TenantChangeViewModel
{
    public TenantLoginInfoDto Tenant { get; set; }
}
