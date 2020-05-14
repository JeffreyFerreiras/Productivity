using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Tools.Attributes
{
    public class ZipCodeAttribute : ValidationAttribute
    {
        private const string US_ZIP_REG_EX = @"^\d{5}(?:[-\s]\d{4})?$";
        private const string CA_ZIP_REG_EX = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";

        public override bool IsValid(object value)
        {
            if (value is string zipCode)
            {
                return Regex.Match(zipCode, US_ZIP_REG_EX).Success
                    || Regex.Match(zipCode, CA_ZIP_REG_EX).Success;
            }

            return false;
        }
    }
}