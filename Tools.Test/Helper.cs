using System;
using System.Collections.Generic;

namespace Tools.Test
{
    public enum Suits
    {
        Spade,
        Diamond,
        Heart,
        Club
    }

    public class FakeTest
    {
        public object A { get; set; }
        public object B { get; set; }
        public object C { get; set; }
        public object D { get; set; }
        public object E { get; set; }
    }

    public class FakeMultiType
    {
        public string StringType { get; set; }
        public double DoubleType { get; set; }
        public DateTime DateType { get; set; }
        public Suits EnumType { get; set; }
        public string[] StringArrayType { get; set; }
        public List<FakeTest> Fakes { get; set; }
        public IDictionary<string, object> DictionaryType { get; set; }
    }

    public class Helper
    {
        public static string[] GeStringArray(int length = 10)
        {
            string[] stringArray = new string[length];

            for(int i = 0; i < length; i++)
            {
                stringArray[i] = RandomString.NextAlphabet();
            }

            return stringArray;
        }

        public static FakeMultiType GetFakeMultiType()
        {
            return new FakeMultiType
            {
                StringType = RandomString.NextAlphabet(),
                DoubleType = 1.1,
                DateType = DateTime.Now,
                EnumType = Suits.Club,
                StringArrayType = GeStringArray(),
                Fakes = GetFakes(),
                DictionaryType =    new Dictionary<string, object>
                {
                    ["one"] = GetFake(),
                    ["two"] = GetFake()
                }
            };
        }
        public static List<FakeTest> GetFakes(int len = 10)
        {
            var fakes = new List<FakeTest>(len);

            for(int i = 0; i < len; i++)
            {
                fakes.Add(GetFake());
            }

            return fakes;
        }

        public static FakeTest GetFake()
        {
            return new FakeTest
            {
                A = RandomString.NextAlphabet(),
                B = 1.55,
                C = DateTime.Now,
                D = Suits.Club
            };
        }
    }
}