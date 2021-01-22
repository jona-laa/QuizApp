using System;
using static System.Console;

namespace Utils
{
    static class Utilities
    {
        /// <summary>
        /// Makes sure Message strings are not empty
        /// </summary>
        public static bool IsValidString(string str)
        {
            return (string.IsNullOrEmpty(str) | String.IsNullOrWhiteSpace(str))  ? false : true;
        }

        /// <summary>
        /// Change text color
        /// </summary>
        public static void TextColor(ConsoleColor color)
        {
            ForegroundColor = color;
        }
    }


}