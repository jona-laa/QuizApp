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

            TextColor(ConsoleColor.DarkRed);
            WriteLine("Q. Quit");
            ResetColor();

            userInput = Console.ReadKey();
            
            switch(userInput.Key)
            {
                // 1. QUIZZES
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    Clear();
                    WriteLine("Quizzes List");
                    
                    // Get Existing Quizzes
                    try {
                        using (var db = new QuizAppContext()) {
                            var quizzes = db.Quizzes
                            .OrderBy(q => q.Id);
                            
                            foreach(var q in quizzes) {
                                WriteLine(q.Title);
                            }
                        }
                    } 
                    catch {
                        WriteLine("There are no quizzes at this moment");
                    }
                    ReadLine();
                    break;



                // 2. CREATE NEW QUIZ
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                using (var db = new QuizAppContext()) {
                    string quizTitle;

                    // Ask for Quiz Name
                    do
                    {
                        Clear();
                        WriteLine("Create New Quiz\n");
                        WriteLine("Quiz Name:");
                        quizTitle = ReadLine();
                    } while(!IsValidString(quizTitle));
                    
                    // Save Quiz to DB
                    db.Add(new Quiz(quizTitle));
                    db.SaveChanges();


                    
                    // CREATE NEW QUESTION
                    int questionNum = 0;
                    string questionText;
                    do
                    {
                        // Ask for valid Question Text
                        do
                        {
                            Clear();
                            WriteLine(quizTitle + "\n");
                            WriteLine($"Question {questionNum + 1}");
                            questionText = ReadLine();
                        } while(!IsValidString(questionText));
                        
                        // Find Quiz 
                        var newQuiz = db.Quizzes
                            .Where(q => q.Title == quizTitle)
                            .First();

                        // Save Question to DB
                        newQuiz.Questions.Add(new Question(questionText));
                        db.SaveChanges();
                        questionNum++;



                        // CREATE FOUR ALTERNATIVES
                        int i = 0;
                        string alternativeText;
                        string isCorrectInput;
                        bool isCorrect;
                        while(i<4)
                        {
                            do
                            {
                                Clear();
                                WriteLine($"Quiz: {quizTitle}\n");
                                WriteLine($"Question: {questionText}\n");
                                WriteLine($"Alternative {i + 1}");
                                alternativeText = ReadLine();
                            } while(!IsValidString(alternativeText));

                            do
                            {
                                Clear();
                                WriteLine($"Quiz: {newQuiz.Title}\n");
                                WriteLine($"Question: {questionText}\n");
                                WriteLine($"Alternative: {alternativeText}\n");
                                WriteLine("Is it the correct answer?(true/false)");
                                isCorrectInput = ReadLine();
                            } while(!Boolean.TryParse(isCorrectInput, out isCorrect));

                            // PROGRAM BREAKS HERE
                            var newQuestion = db.Questions
                                .Where(q => q.QuestionText == questionText)
                                .First();

                            newQuestion.Alternatives.Add(new Alternative() {
                                AlternativeText = alternativeText,
                                IsCorrect = isCorrect
                            });

                            i++;
                        }

                        WriteLine($"Question Summary: \n Question: {questionText}\n");

                        var createdQuizId = db.Quizzes
                            .Where(q => q.Title == quizTitle)
                            .First().Id;

                        var quizQuestions = db.Questions
                                .Where(q => q.QuizId == createdQuizId)
                                .OrderBy(q => q.Id);

                        WriteLine($"Created Quiz ID: {createdQuizId}");

                        WriteLine("Quiz Questions");
                        foreach(var q in quizQuestions)
                        {
                            WriteLine(q.QuestionText);
                        }

                        // var newQuiz = db.Quizzes
                        //     .Where(q => q.Title == quizTitle)
                        //     .First();

                        // foreach(var alt in question.Alternatives)
                        // {          
                        //     WriteLine($"Alternative: {alt.AlternativeText}\nIs correct?: {alt.IsCorrect}\n");
                        // }


                        // Give option to add new question or save quiz
                        WriteLine("1. New Question");
                        WriteLine("2. Save Quiz");
                        userInput = ReadKey();
                    } while(userInput.Key != ConsoleKey.D2);

                    // WriteLine(newQuiz.Questions.Count);
                    ReadLine();
                }
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
      
