using Productivity.Extensions.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Productivity.Attributes
{
    /// <summary>
    /// Aba Attribute to be used for validating if a property is a valid ABA routing number. Can only be used with string types.
    /// </summary>
    /// <exception cref="ArgumentException">Can only be used with string types</exception>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AbaAttribute : ValidationAttribute
    {
        /// <summary>
        /// Attribute that validates wether the value is a valid aba routing number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            ErrorMessage = "ABA number is not valid";

            if (value is string aba)
            {
                return aba.IsValidAba();
            }

            throw new ArgumentException($"{nameof(AbaAttribute)} can only be used on string types");
        }
    }
}