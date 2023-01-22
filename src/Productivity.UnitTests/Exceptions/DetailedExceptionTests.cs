using NUnit.Framework;
using Productivity.Exceptions;
using System;

namespace Productivity.UnitTests.Exceptions
{
    [TestFixture]
    public class DetailedExceptionTests
    {
        [Test]
        public void ToString_ReturnsDetailedToString()
        {
            try
            {
                throw new DetailedException("First", new Exception("inner exception", new NullReferenceException("null reference!")));
            }
            catch (DetailedException e)
            {
                string message = e.ToString();

                System.Diagnostics.Debug.WriteLine(e);

                Assert.IsNotNull(message);
                Assert.IsTrue(message.Length > 0);
            }
        }
    }
}