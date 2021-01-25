using System;
using System.Threading;
using static System.Console;
using static Utils.Utilities;



namespace QuizApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // RUN APPLICATION
            while(true)
            {
                RunQuizApp();
            }
        }

        public static void RunQuizApp()
        {
            ConsoleKeyInfo userInputKey;

            Clear();
            WriteColoredLine("C O N S O L E   Q U I Z   A P P", ConsoleColor.DarkGreen);
            WriteLine("\n1. Quizzes");
            WriteLine("2. Create New Quiz\n");
            WriteColoredLine("Q. Quit", ConsoleColor.DarkRed);

            userInputKey = Console.ReadKey();
            
            // QUIZZES, CREATE, OR QUIT
            switch(userInputKey.Key)
            {
                // MENU - 1. QUIZZES
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:

                    // Ask for valid input
                    do
                    {
                        Clear();
                        WriteColoredLine("QUIZZES\n", ConsoleColor.DarkGreen);
                        WriteLine("1. Play Quiz");
                        WriteLine("2. Delete Quiz\n");
                        WriteColoredLine("C. Cancel", ConsoleColor.DarkRed);
                        userInputKey = ReadKey();
                    } while(!IsValidChoice(userInputKey.KeyChar, 2, "C"));

                    // PLAY, DELETE OR CANCEL
                    switch(userInputKey.Key)
                    {
                        // QUIZZES - 1. PLAY QUIZ
                        case ConsoleKey.D1:
                            QuizHandler.PlayQuiz();                            
                            break;

                        // QUIZZES - 2. DELETE QUIZ
                        case ConsoleKey.D2:
                            QuizHandler.DeleteQuiz();  
                            break;
                        
                        case ConsoleKey.C:
                            RunQuizApp();
                            break;

                        default:
                            break;
                    }


                    break;



                // MENU - 2. CREATE NEW QUIZ
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    QuizHandler.CreateQuiz();
                    break;

                // MENU - Q. QUIT
                case ConsoleKey.Q:
                    Clear();
                    WriteLine("Quitting application...");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;

                default:
                    RunQuizApp();
                    break;
            }
        }
    }
}
      
