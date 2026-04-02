using Abp.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pbt.Individual.Identity
{
    public class ExternalLoginInfoHelper
    {
        public static (string name, string phoneNumber) GetNameAndphoneNumberFromClaims(List<Claim> claims)
        {
            string name = null;
            string phoneNumber = null;

            var givennameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (givennameClaim != null && !givennameClaim.Value.IsNullOrEmpty())
            {
                name = givennameClaim.Value;
            }

            var phoneNumberClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone);
            if (phoneNumberClaim != null && !phoneNumberClaim.Value.IsNullOrEmpty())
            {
                phoneNumber = phoneNumberClaim.Value;
            }

            if (name == null || phoneNumber == null)
            {
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    var namephoneNumber = nameClaim.Value;
                    if (!namephoneNumber.IsNullOrEmpty())
                    {
                        var lastSpaceIndex = namephoneNumber.LastIndexOf(' ');
                        if (lastSpaceIndex < 1 || lastSpaceIndex > (namephoneNumber.Length - 2))
                        {
                            name = phoneNumber = namephoneNumber;
                        }
                        else
                        {
                            name = namephoneNumber.Substring(0, lastSpaceIndex);
                            phoneNumber = namephoneNumber.Substring(lastSpaceIndex);
                        }
                    }
                }
            }

            return (name, phoneNumber);
        }
    }
}
