using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.Extensions.Reflection;

namespace Tools.Test.Extensions.Reflection
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        public void DeepCopy_ValidObject_Clones()
        {
            var obj = Helper.GetFake();
            var clone = obj.DeepClone();

            foreach(var prop in obj.GetType().GetTypeInfo().GetProperties())
            {
                var originalValue = prop.GetValue(obj);
                var cloneValue = prop.GetValue(clone);

                Assert.IsTrue(cloneValue.Equals(originalValue));
            }
        }

        [TestMethod]
        public void DeepCopy_ValidClomplexObject_Clones()
        {
            var obj = Helper.GetFakeMultiType();
            var clone = obj.DeepClone();

            foreach(var prop in obj.GetType().GetTypeInfo().GetProperties())
            {
                var originalValue = prop.GetValue(obj);
                var cloneValue = prop.GetValue(clone);

                if(originalValue == null) continue;

                if(originalValue is List<FakeTest>)
                {
                    var orig = (List<FakeTest>)originalValue;
                    var clon = (List<FakeTest>)cloneValue;

                    Assert.IsFalse(cloneValue == originalValue);
                    orig.SequenceEqual(clon);
                }
                else if(originalValue is string[])
                {
                    var orig = (string[])originalValue;
                    var clon = (string[])cloneValue;

                    Assert.IsFalse(cloneValue == originalValue);
                    Assert.IsTrue(orig.SequenceEqual(clon));
                }
                else
                {
                    Assert.IsTrue(cloneValue.Equals(originalValue)); //Deep comparison
                    Assert.IsFalse(cloneValue == originalValue); // Reference comparison

                    originalValue = null;

                    Assert.IsFalse(cloneValue.Equals(originalValue)); //Make sure references were not simply copied.
                }
            }
        }
    }
}
