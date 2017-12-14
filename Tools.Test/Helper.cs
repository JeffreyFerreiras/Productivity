using System;

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
        public string A { get; set; }
        public double B { get; set; }
        public DateTime C { get; set; }
        public Suits D { get; set; }
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
    }
}