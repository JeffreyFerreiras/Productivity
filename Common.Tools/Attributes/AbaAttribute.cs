using Common.Tools.Extensions.Validation;
using System.ComponentModel.DataAnnotations;

namespace Common.Tools.Attributes
{
    public class AbaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "ABA number is not valid";

            if (value is string aba)
            {
                return aba.IsValidAba();
            }

            return false;
        }
    }
}