﻿using Productivity.RandomGenerator;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Productivity.UnitTests
{
    public struct SampleStruct
    {
        public int Count { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime Time { get; set; }

        public int[] Numbers => new int[] { 1, 2, 3, 4, 5, 6 };
    }

    [XmlRoot(ElementName = "catalog")]
    public class Catalog
    {
        [XmlElement(ElementName = "book")]
        public List<Book> BookList
        {
            get;
            set;
        }

        public Catalog()
        {
            BookList = new List<Book>();
        }
    }

    public class Book
    {
        //public string ID { get; set; }
        [XmlElement(ElementName = "author")]
        public string Author { get; set; }

        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "genre")]
        public string Genre { get; set; }

        [XmlElement(ElementName = "price")]
        public decimal Price { get; set; }

        [XmlElement(ElementName = "publish_date")]
        public DateTime PublishDate { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
    }

    public enum Suits
    {
        Spade,
        Diamond,
        Heart,
        Club
    }

    [Serializable]
    public class SimpleFake
    {
        public object A { get; set; }
        public object B { get; set; }
        public object C { get; set; }
        public object D { get; set; }
        public object E { get; set; }
    }

    [Serializable]
    public class ComplexFake
    {
        public string StringType { get; set; }
        public double DoubleType { get; set; }
        public DateTime DateType { get; set; }
        public Suits EnumType { get; set; }
        public string[] StringArrayType { get; set; }
        public List<SimpleFake> Fakes { get; set; }
        public IDictionary<string, object> DictionaryType { get; set; }
    }

    public class ComplexMan
    {
        public DateTime Dob { get; set; } = DateTime.Now;
        public string Name { get; set; } = "BOB";
        public decimal Age { get; set; } = 30;
        public Suits Suit { get; set; } = Suits.Spade;
    }

    public class SimpleMan
    {
        public string Dob { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Suit { get; set; }
    }

    public static class Helper
    {
        private readonly static Random SRandom = new Random();
        private static readonly object SSyncLock = new object();

        public static int[] GetRandomArray(int size)
        {
            return GetRandomArray(size, 0, 9999);
        }

        public static int[] GetRandomArray(int size, int minValue, int maxValue)
        {
            var arr = new int[size];

            for (int i = 0; i < arr.Length; i++)
            {
                lock (SSyncLock)
                {
                    arr[i] = SRandom.Next(minValue, maxValue);
                }
            }

            return arr;
        }

        public static string[] GeStringArray(int length = 10)
        {
            string[] stringArray = new string[length];

            for (int i = 0; i < length; i++)
            {
                stringArray[i] = RandomString.NextAlphabet();
            }

            return stringArray;
        }

        public static ComplexFake GetComplexFake()
        {
            return new ComplexFake
            {
                StringType = RandomString.NextAlphabet(),
                DoubleType = 1.1,
                DateType = DateTime.Now,
                EnumType = Suits.Club,
                StringArrayType = GeStringArray(),
                Fakes = GetSimpleFakes(),
                DictionaryType = new Dictionary<string, object>
                {
                    ["one"] = GetSimpleFake(),
                    ["two"] = GetSimpleFake()
                }
            };
        }

        public static List<SimpleFake> GetSimpleFakes(int len = 10)
        {
            var fakes = new List<SimpleFake>(len);

            for (int i = 0; i < len; i++)
            {
                fakes.Add(GetSimpleFake());
            }

            return fakes;
        }

        public static SimpleFake GetSimpleFake()
        {
            return new SimpleFake
            {
                A = RandomString.NextAlphabet(),
                B = SRandom.NextDouble(),
                C = DateTime.Now,
                D = Suits.Club
            };
        }
    }
}