using NUnit.Framework;
using System;
using System.Collections.Generic;
using Productivity.Extensions;

namespace Productivity.UnitTests.Extensions
{
    [TestFixture()]
    public class NumbersExTests
    {
        [Test()]
        public void SumMoney_ListOfAmounts_Sums()
        {
            var amounts = new decimal[] { 4.444m, 1.41254m };
            var totalRounded = amounts.SumMoney();
            var multiplied = totalRounded * 100;

            Assert.True(multiplied == Math.Floor(multiplied));
        }

        [Test()]
        public void SumMoney_EmptyListOfAmounts_Zero()
        {
            var amounts = new decimal[] { };

            Assert.True(amounts.SumMoney() == 0);
        }

        [Test()]
        public void SumMoney_NullListOfAmounts_Throws()
        {
            IEnumerable<decimal> amounts = null;
            Assert.Throws<ArgumentException>(() => amounts.SumMoney());
        }
    }
}