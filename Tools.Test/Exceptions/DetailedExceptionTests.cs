using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tools.Exceptions;

namespace Tools.Test.Exceptions
{
    [TestClass]
    public class DetailedExceptionTests
    {
        [TestMethod]
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