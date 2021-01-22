using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
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
            ConsoleKeyInfo userInput;

            Clear();
            TextColor(ConsoleColor.DarkGreen);
            // TITLE
            WriteLine("C O N S O L E   Q U I Z   A P P");
            ResetColor();
            
            WriteLine("\n# M E N U");
            WriteLine("1. Quizzes");
            WriteLine("2. Create New Quiz\n");

            WriteLine("Q. Quit");

            userInput = Console.ReadKey();
            
            switch(userInput.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    Clear();
                    WriteLine("Quizzes List");
                    ReadLine();
                    break;

                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    string quizTitle;
                    do
                    {
                        Clear();
                        WriteLine("Create New Quiz\n");
                        WriteLine("Quiz Name:");
                        quizTitle = ReadLine();
                    } while(!IsValidString(quizTitle));
                    Quiz quiz = new Quiz(quizTitle); 
                    WriteLine(quiz.Title);
                    // Save Quiz to DB
                    //  
                    //DbConnection();
                    // Get ID of quiz fr DB to give as FK to its questions
                    // 
                    // Create new Question with a text and quiz ID as arguments.
                    int questionNum = 0;
                    string questionText;
                    do
                    {
                        do
                        {
                            Clear();
                            WriteLine(quiz.Title + "\n");
                            WriteLine($"Question {questionNum + 1}");
                            questionText = ReadLine();
                        } while(!IsValidString(questionText));

                        Question question = new Question();
                        question.QuizId = 1; // Add real Quiz ID from DB
                        question.QuestionText = questionText;
                        quiz.AddQuestion(question);
                        questionNum++;
                        // Save Questions to DB
                        //
                        // Get ID of new question from DB to give as FK to its alternatives


                        // Create new Alternatives with text, question ID, and isCorrect as arguments
                        // Create 4 alternatives
                        int i = 0;
                        string alternativeText;
                        string isCorrectInput;
                        bool isCorrect;
                        while(i<4)
                        {
                            do
                            {
                                Clear();
                                WriteLine($"Quiz: {quiz.Title}\n");
                                WriteLine($"Question: {question.QuestionText}\n");
                                WriteLine($"Alternative {i + 1}");
                                alternativeText = ReadLine();
                            } while(!IsValidString(alternativeText));

                            do
                            {
                                Clear();
                                WriteLine($"Quiz: {quiz.Title}\n");
                                WriteLine($"Question: {question.QuestionText}\n");
                                WriteLine($"Alternative: {alternativeText}\n");
                                WriteLine("Is it the correct answer?(true/false)");
                                isCorrectInput = ReadLine();
                            } while(!Boolean.TryParse(isCorrectInput, out isCorrect));

                            question.AddAlternative(new Alternative() {
                                AlternativeText = alternativeText,
                                QuestionId = 1, // Add real Question ID from DB
                                IsCorrect = isCorrect
                            });
                            i++;
                        }
                        // Save Alternatives to DB
                        //
                        WriteLine($"Question Summary: {question.QuestionText}\n");
                        foreach(var alt in question.Alternatives)
                        {          
                            WriteLine($"Alternative: {alt.AlternativeText}\nIs correct?: {alt.IsCorrect}\n");
                        }


                        // Give option to add new question or save quiz
                        WriteLine("1. New Question");
                        WriteLine("2. Save Quiz");
                        userInput = ReadKey();
                    } while(userInput.Key != ConsoleKey.D2);

                    WriteLine(quiz.Questions.Count);
                    ReadLine();
                    break;

                case ConsoleKey.Q:
                    Clear();
                    WriteLine("Quitting application...");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;

                default:
                    WriteLine(userInput.Key.ToString()); // DEBUG can delete
                    ReadLine();
                    break;
            }
        }
    }
}
      
