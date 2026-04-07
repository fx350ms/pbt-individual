using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using Pbt.Individual.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pbt.Individual.Web.Models.Account;

public class RegisterViewModel : IValidatableObject
{
    [Required]
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxPhoneNumberLength)]
    public string PhoneNumber { get; set; }

    [StringLength(AbpUserBase.MaxUserNameLength)]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; }

    [StringLength(AbpUserBase.MaxPlainPasswordLength)]
    [DisableAuditing]
    public string Password { get; set; }

    public bool IsExternalLogin { get; set; }

    public string ExternalLoginAuthSchema { get; set; }

    /// <summary>
    /// Kho nhận hàng ở Việt Nam
    /// </summary>
    public int WarehouseId { get; set; }
    
    /// <summary>
    /// Kho nhận hàng ở Trung Quốc
    /// </summary>
    public int CNWarehouseId { get; set; }

    

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!UserName.IsNullOrEmpty())
        {
            if (!UserName.Equals(EmailAddress) && ValidationHelper.IsEmail(UserName))
            {
                yield return new ValidationResult("Username cannot be an email address unless it's the same as your email address!");
            }
        }
    }
}
