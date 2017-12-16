using System;
using System.Reflection;

namespace Tools.Extensions.Validation
{
    /// <summary>
    /// Provides common validation functionality.
    /// </summary>

    public static class Validation
    {
        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

        public static bool IsNumber(this object value)
        {
            if(value is string) return value.ToString().IsValidNumber();

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

        public static bool IsValidNumber(this string num)
        {
            if(string.IsNullOrWhiteSpace(num)) return false;
            if(num[0] == '-') num = num.Substring(1);

            foreach(var c in num)
            {
                if(c >= 48 && c <= 57) continue;

                return false;
            }

            return true;
        }

        public static bool ValidateProperties<T>(this T value)
        {
            if(value == null) return false;

            foreach(var m in value.GetType().GetTypeInfo().GetProperties())
                if(!m.GetValue(value).IsValid()) return false;

            return true;
        }

        public static bool IsValid<T>(this T value)
        {
            if(value == null) return false;
            if(value.IsNumber()) return Convert.ToInt32(value) > -1;
            if(value is string) return !string.IsNullOrWhiteSpace(value.ToString());
            if(value.HasProp("Count")) return (int)value.GetType().GetProperty("Count").GetValue(value) > 0;

            return true;
        }
        
        public static bool HasOnlyLetters(this string letters)
        {
            foreach(var letter in letters)
            {
                if(letter > 96 && letter < 123) continue;
                if(letter > 64 && letter < 90) continue;

                return false;
            }

            return true;
        }

        public static bool HasCharType(this string pw, Predicate<char> predicate, int count = 1)
        {
            foreach(char letter in pw)
            {
                if(predicate(letter)) count--;
                if(count <= 0) return true;
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

        public static bool HasProp<T>(this T value, string propName)
        {
            PropertyInfo property = value.GetType().GetTypeInfo().GetProperty(propName);
            return property != null;
        }
    }
}