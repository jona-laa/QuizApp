using System;
using static System.Console;

namespace Utils
{
    /// <summary>
    /// The Utilities Class
    /// Contains methods for formatting text and validating user input
    /// </summary>
    static class Utilities
    {
        /// <summary>
        /// Makes sure Message strings are not empty
        /// </summary>
        /// <param name="str">User input to validate</param>
        public static bool IsValidString(string str)
        {
            return (string.IsNullOrEmpty(str) | String.IsNullOrWhiteSpace(str))  ? false : true;
        }



        /// <summary>
        /// Change text color
        /// </summary>
        /// <param name="text">String to print to console</param>
        /// <param name="color">Color of text</param>
        public static void WriteColoredLine(string text, ConsoleColor color)
        {
            ForegroundColor = color;
            WriteLine(text);
            ResetColor();
        }



        /// <summary>
        /// Checks for valid input.
        /// Used to force user to provide correct input in menus
        /// </summary>
        /// <param name="key">Character from ReadKey.KeyChar</param>
        /// <param name="maxInput">Max value for user input</param>
        /// <param name="cancelBtn">Excepted string value for e.g cancel or quit input</param>
        public static bool IsValidChoice(char key, int maxInput, string cancelBtn)
        {
            Int16 inputNumber;
            if (Int16.TryParse(key.ToString(), out inputNumber))
            {
                return (inputNumber >= 1 && inputNumber <= maxInput) ? true : false;
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