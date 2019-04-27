using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions.Reflection;

namespace Tools.Test.Extensions.Reflection
{
    [TestFixture]
    public class ReflectionTests
    {
        [Test]
        public void DeepClone_IEnumerable_ReturnsDeepClone()
        {
            var enumerable = Enumerable.Repeat(Helper.GetSimpleFake(), 2);
            var clone = enumerable.DeepClone();

            bool hasSameRefs = enumerable.SequenceEqual(clone);

            Assert.IsFalse(hasSameRefs);
        }

        [Test]
        public void DeepClone_ArrayOfReferenceTypes_ReturnsDeepClone()
        {
            var arr = Enumerable.Repeat(Helper.GetSimpleFake(), 2).ToArray();
            var arrClone = arr.DeepClone();

            arr[0].A = "original";

            Assert.IsFalse(arr[0].A == arrClone[0].A);
        }

        [Test]
        public void DeepClone_SimpleObject_ReturnsDeepClone()
        {
            var original = Helper.GetSimpleFake();
            var clone = original.DeepClone();

            original.A = "original";

            Assert.IsFalse(original.A == clone.A);
        }

        [Test]
        public void DeepClone_Dictionary_ReturnsDictionaryClone()
        {
            var original = new Dictionary<string, SimpleFake>
            {
                ["Jeff"] = Helper.GetSimpleFake(),
                ["bob"] = Helper.GetSimpleFake()
            };

            var clone = original.DeepClone();

            original["Jeff"].A = "original";

            Assert.IsFalse(original["Jeff"].A == clone["Jeff"].A);
        }
    }
}