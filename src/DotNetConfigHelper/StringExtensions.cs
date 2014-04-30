using System;

namespace DotNetConfigHelper
{
    internal static class StringExtensions
    {
        public static string Fmt(this string format, params object[] args)
        {
            return String.Format(format, args);
        }
    }
}