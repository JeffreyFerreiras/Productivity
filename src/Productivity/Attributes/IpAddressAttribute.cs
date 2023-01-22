using Productivity.Extensions.Validation;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Productivity.Attributes
{
    /// <summary>
    /// Determines if a property is a valid IP address
    /// </summary>
    public class IpAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string ip = value as string;

            if (!ip.IsValid())
            {
                return false;
            }

            if (IPAddress.TryParse(ip, out IPAddress ipAddress))
            {
                return true;
            }

            return false;
        }
    }
}