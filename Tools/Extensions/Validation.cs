using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;


namespace Tools.Extensions
{
    /// <summary>
    /// Provides common validation functionality.
    /// </summary>

    public static class Validation
    {
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }

        public static bool ValidateProperties(this object o)
        {
            if (o == null) return false;

            foreach (var m in o.GetType().GetTypeInfo().GetProperties())
                if (!m.GetValue(o).IsValid()) return false;

            return true;
        }

        public static bool IsValid(this object o)
        {
            if (o.IsNumber()) return Convert.ToInt32(o) > 0;
            if (o == null) return false;
            if (o is string && string.IsNullOrWhiteSpace(o.ToString())) return false;
            if (o.HasProp("Count")) return (int)o.GetPropertyVal("Count") > 0;

            return true;
        }

        public static bool IsValidNumber(this string num)
        {
            if (!num.IsValid()) return false;

            foreach (var c in num)
            {
                if (c >= 48 && c <= 57) continue;

                return false;
            }

            return true;
        }

        public static bool OnlyLetters(this string letters)
        {
            foreach (var letter in letters)
            {
                if (letter > 96 && letter < 123) continue;
                if (letter > 64 && letter < 90) continue;

                return false;
            }
            return true;
        }

        public static bool HasCharType(this string pw, Predicate<char> predicate, int count = 1)
        {
            foreach (char letter in pw)
            {
                if (predicate(letter)) count--;
                if (count <= 0) return true;
            }

            return false;
        }

        public static bool HasUpper(this string pw, int count = 1)
        {
            Predicate<char> predicate = c => char.IsUpper(c);
            return pw.HasCharType(predicate, count);
        }

        public static bool HasLower(this string pw, int count = 1)
        {
            Predicate<char> predicate = c => char.IsLower(c);
            return pw.HasCharType(predicate, count);
        }

        public static bool HasNumber(this string pw, int count = 1)
        {
            Predicate<char> predicate = c => char.IsDigit(c);
            return pw.HasCharType(predicate, count);
        }

        public static bool HasProp(this object o, string propName)
        {
            PropertyInfo countProp = o.GetType().GetTypeInfo().GetProperty(propName);
            return countProp != null;
        }
    }
}