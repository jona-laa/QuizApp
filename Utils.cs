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



        /// <summary>
        /// Checks for valid integer, or C, when in Delete menu
        /// </summary>
        public static bool IsValidChoice(char key, int maxInput, string cancelBtn)
        {
            Int16 inputNumber;
            if (Int16.TryParse(key.ToString(), out inputNumber))
            {
                return (inputNumber >= 0 && inputNumber <= maxInput) ? true : false;
            }
            else if (key.ToString().ToUpper() == cancelBtn)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}