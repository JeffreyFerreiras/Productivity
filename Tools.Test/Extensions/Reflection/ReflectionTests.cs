using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tools.Extensions.Reflection;

namespace Tools.Test.Extensions.Reflection
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        public void DeepClone_IEnumerable_Clones()
        {
            var enumerable = Enumerable.Repeat(Helper.GetFake(), 2);
            var clone = enumerable.DeepClone();

            bool hasSameRefs = enumerable.SequenceEqual(clone);

            Assert.IsFalse(hasSameRefs);
        }

        [TestMethod]
        public void DeepClone_ArrayOfReferenceTypes_Clones()
        {
            var arr = Enumerable.Repeat(Helper.GetFake(), 2).ToArray();
            var arrClone = arr.DeepClone();

            arr[0].A = "original";

            Assert.IsFalse(arr[0].A == arrClone[0].A);
        }

        [TestMethod]
        public void DeepClone_SimpleObject_Clones()
        {
            var original = Helper.GetFake();
            var clone = original.DeepClone();

            original.A = "original";

            Assert.IsFalse(original.A == clone.A);
        }

        [TestMethod]
        public void DeepClone_Dictionary_Clones()
        {
            var dict = new Dictionary<string, FakeTest>
            {
                ["Jeff"] = Helper.GetFake(),
                ["bob"] = Helper.GetFake()
            };

            var clone = dict.DeepClone();

            dict["Jeff"].A = "original";

            Assert.IsFalse(dict["Jeff"].A == clone["Jeff"].A);

        }

        [TestMethod]
        public void DeepCopy_ValidClomplexObject_Clones()
        {
            var original = Helper.GetFakeMultiType();
            var clone = original.DeepClone();

            foreach(var prop in original.GetType().GetTypeInfo().GetProperties())
            {
                var originalValue = prop.GetValue(original);
                var cloneValue = prop.GetValue(clone);

                if(originalValue == null) continue;

                if(originalValue is IDictionary<string, object>)
                {
                    var orig = (IDictionary<string, object>)originalValue;
                    var clon = (IDictionary<string, object>)cloneValue;

                    orig["one"] = null;

                    Assert.IsFalse(orig["one"] == clon["one"]);
                }
                else if(originalValue is List<FakeTest>)
                {
                    var orig = (List<FakeTest>)originalValue;
                    var clon = (List<FakeTest>)cloneValue;

                    orig[0].A = "Original";

                    Assert.IsFalse(orig[0].A.Equals(clon[0].A));
                }
                else if(originalValue is string[])
                {
                    var orig = (string[])originalValue;
                    var clon = (string[])cloneValue;

                    Assert.IsFalse(cloneValue == originalValue);
                    Assert.IsTrue(orig.SequenceEqual(clon));

                    orig[0] = "Original";

                    Assert.IsFalse(orig[0] == clon[0]);
                }
                else
                {
                    Assert.IsTrue(cloneValue.Equals(originalValue));    // Deep comparison
                    Assert.IsFalse(cloneValue == originalValue);        // Reference comparison

                    originalValue = null; //If a shallow copy was performed, the cloneValue will also be null.

                    Assert.IsFalse(cloneValue.Equals(originalValue));   //Make sure references were not simply copied.
                }
            }
        }
    }
}