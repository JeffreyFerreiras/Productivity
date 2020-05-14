using Common.Tools.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Tools.Extensions
{
    public static class NumbersEx
    {
        public static decimal Round(this decimal source) => Math.Round(source, 2, MidpointRounding.ToEven);

        public static decimal SumMoney(this IEnumerable<decimal> numbers)
        {
            Guard.AssertArgs(numbers != null, $"{nameof(numbers)} is null");

            var list = numbers.ToList();
            
            list.ForEach(num => num = num.Round());

            return list.Sum().Round();
        }
    }
}