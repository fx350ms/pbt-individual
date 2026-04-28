using Abp.Authorization.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pbt.Individual.Web.Models.Users;

public class UpdateViewModel
{
    [Required]
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxPhoneNumberLength)]
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; }

    public int WarehouseId { get; set; }

    public int CNWarehouseId { get; set; }
}