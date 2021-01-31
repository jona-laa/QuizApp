using System;
using System.Linq;
using static System.Console;
using static Utils.Utilities;
using System.Collections.Generic;
using System.Threading;
using static QuizApp.Program;
using QuizApp.Models;



namespace QuizApp
{
    public class QuizHandler
    {
        /// <summary>
        /// Logic for creating new quiz
        /// </summary>
        public static void CreateQuiz()
        {
            ConsoleKeyInfo userInputKey;
            
            try
            {
                using (var db = new QuizAppContext()) 
                {
                    string quizTitle;

                    // Ask for Quiz Name
                    do
                    {
                        Clear();
                        WriteColoredLine("Create New Quiz\n", ConsoleColor.DarkGreen);
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
                        
                        // Find Quiz to add Question to
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
                            // Ask for valid Alternative Text
                            do
                            {
                                Clear();
                                WriteLine($"Quiz: {quizTitle}\n");
                                WriteLine($"Question: {questionText}\n");
                                WriteLine($"Alternative {i + 1}");
                                alternativeText = ReadLine();
                            } while(!IsValidString(alternativeText));

                            // Ask for valid Boolean(Is Alternative true/false?)
                            do
                            {
                                Clear();
                                WriteLine($"Quiz: {newQuiz.Title}\n");
                                WriteLine($"Question: {questionText}\n");
                                WriteLine($"Alternative: {alternativeText}\n");
                                WriteLine("Is it the correct answer?(true/false)");
                                isCorrectInput = ReadLine();
                            } while(!Boolean.TryParse(isCorrectInput, out isCorrect));

                            // Find Question to add Alternatives to
                            var newQuestion = db.Questions
                                .Where(q => q.QuestionText == questionText)
                                .First();

                            // Save Alternatives to DB
                            newQuestion.Alternatives.Add(
                                new Alternative(alternativeText, isCorrect)
                                );
                            db.SaveChanges();

                            i++;
                        }



                        // QUESTION SUMMARY
                        Clear();
                        WriteColoredLine($"QUIZ SUMMARY \n", ConsoleColor.DarkGreen);
                        WriteColoredLine($"Quiz Name:", ConsoleColor.DarkGreen);
                        WriteLine($"{quizTitle}\n");

                        // Get Quiz Id and Quiz Questions
                        var createdQuizId = db.Quizzes
                            .Where(q => q.Title == quizTitle)
                            .First().Id;

                        var quizQuestions = db.Questions
                                .Where(q => q.QuizId == createdQuizId)
                                .OrderBy(q => q.Id);

                        WriteColoredLine("Quiz Questions:", ConsoleColor.DarkGreen);
                        foreach(var q in quizQuestions)
                        {
                            WriteColoredLine($"Question: {q.QuestionText}", ConsoleColor.DarkYellow);
                            
                            // Get Question Alternatives
                            var questionAnswers = db.Alternatives
                                .Where(a => a.QuestionId == q.Id)
                                .OrderBy(a => a.Id);
                            
                            WriteColoredLine("\nQuestion Alternatives:", ConsoleColor.DarkYellow);
                            foreach(var a in questionAnswers)
                            {
                                WriteLine($"Alternative: {a.AlternativeText}, Is Correct: {a.IsCorrect}");
                            }

                            WriteLine("");
                        }


                            // ADD NEW QUESTION OR SAVE QUIZ
                            WriteLine("1. New Question");
                            WriteLine("2. Save Quiz");
                        do
                        {
                            userInputKey = ReadKey();
                        } while (!IsValidChoice(userInputKey.KeyChar, 2, "C"));

                    } while(userInputKey.KeyChar.ToString().ToUpper() == "1");

                    Clear();
                    WriteLine("Saving Quiz...");
                    Thread.Sleep(1000);
                    RunQuizApp();
                }
                
            }
            catch (System.Exception)
            {
                WriteLine("Something went wrong. Try again.");
                ReadLine();
            }
        }



        /// <summary>
        /// Get All Quizzes
        /// </summary>
        public static List<Quiz> GetQuizzes()
        {
            try 
            {
                using (var db = new QuizAppContext()) 
                {
                    var quizzes = db.Quizzes
                    .OrderBy(q => q.Id).ToList();

                    if (quizzes.Count > 0) {
                        return quizzes;
                    } 
                    else {
                        WriteColoredLine("No Quizzes at this moment. Create one!", ConsoleColor.DarkRed);
                        WriteLine("\n[Any Key] Menu");
                        ReadLine();
                        return null;
                    }
                }
            } 
            catch 
            {
                WriteColoredLine("Error fetching Quizzes. Press any key.", ConsoleColor.DarkRed);
                ReadLine();
            }
            return null;
        }



        /// <summary>
        /// Delete Quiz
        /// </summary>
        public static void DeleteQuiz()
        {
            char userInputKey;
            List<Quiz> quizzes;

            do 
            {
                Clear();
                WriteColoredLine("DELETE QUIZ\n", ConsoleColor.DarkGreen);
                
                quizzes = GetQuizzes();

                if (quizzes != null)
                {
                    if (quizzes.Count != 0)
                    {
                        foreach(var q in quizzes) {
                            WriteLine($"[{quizzes.IndexOf(q) + 1}] {q.Title}");
                        }
                    }
                    else
                    {
                        RunQuizApp();
                    }
                }
                else 
                {
                    RunQuizApp();
                }

                WriteColoredLine("\nC. Cancel\n", ConsoleColor.DarkRed);

                // Ask for quiz ID
                userInputKey = ReadKey().KeyChar;
            } while(!IsValidChoice(userInputKey, quizzes.Count, "C"));

            // CANCEL
            if(userInputKey.ToString().ToUpper() == "C")
            {                
                RunQuizApp();
            }
            // DELETE QUIZ 
            else 
            {
                try 
                {
                    using (var db = new QuizAppContext()) 
                    {
                        // Get Quiz to Remove...
                        var quizToRemove = db.Quizzes
                            .Where(q => q.Id == quizzes[Int16.Parse(userInputKey.ToString()) - 1].Id)
                            .First();

                        // ...and Quiz Questions to Remove
                        var questionsToRemove = db.Questions
                            .Where(q => q.QuizId == quizToRemove.Id);
                        
                        db.Remove(quizToRemove);
                        
                        // Remove every Question together with its Alternatives
                        foreach(var question in questionsToRemove)
                        {
                            var alternativesToRemove = db.Alternatives
                                .Where(a => a.QuestionId == question.Id);
                            
                            db.Remove(question);

                            foreach(var alternative in alternativesToRemove)
                            {
                                db.Remove(alternative);
                            }
                        }
                        
                        db.SaveChanges();
                        
                        Clear();
                        WriteLine("Deleting Quiz...");
                        Thread.Sleep(1000); 
                        
                        RunQuizApp();
                    }
                } 
                catch 
                {
                    WriteColoredLine("Something went wrong", ConsoleColor.DarkRed);
                }
            }
        }



        /// <summary>
        /// Play Quiz
        /// </summary>
        public static void PlayQuiz()
        {
            char userInputKey;
            List<Quiz> quizzes;
            List<Alternative> questionAlternatives;
            int quizScore = 0;

            do {
                Clear();
                WriteColoredLine("PLAY QUIZ\n", ConsoleColor.DarkGreen);

                quizzes = QuizHandler.GetQuizzes();
   
                if (quizzes != null)
                {
                    if (quizzes.Count != 0)
                    {
                        foreach(var q in quizzes) {
                            WriteLine($"[{quizzes.IndexOf(q) + 1}] {q.Title}");
                        }
                    }
                    else
                    {
                        RunQuizApp();
                    }
                }
                else 
                {
                    RunQuizApp();
                }

                WriteColoredLine("\nC. Cancel\n", ConsoleColor.DarkRed);

                // Ask for Quiz Index
                userInputKey = ReadKey().KeyChar;
            } while(!IsValidChoice(userInputKey, quizzes.Count, "C"));

            // CANCEL
            if(userInputKey.ToString().ToUpper() == "C")
            {                
                RunQuizApp();
            }
            // RUN QUIZ
            else
            {
                try
                {
                    using (var db = new QuizAppContext()) 
                    {
                        // Get Quiz Id...
                        var quiz = db.Quizzes
                            .Where(q => q.Id == quizzes[Int16.Parse(userInputKey.ToString()) - 1].Id)
                            .First();

                        // ...and Quiz Questions
                        var quizQuestions = db.Questions
                            .Where(q => q.QuizId == quiz.Id)
                            .OrderBy(q => q.Id).ToList();

                        foreach(var q in quizQuestions)
                        {
                            do
                            {
                                Clear();
                                WriteColoredLine($"QUESTION {quizQuestions.IndexOf(q) + 1} / {quizQuestions.Count}\n", ConsoleColor.DarkGreen);
                                WriteColoredLine($"{q.QuestionText}\n", ConsoleColor.DarkYellow);
                                
                                // Get Question Alternatives
                                questionAlternatives = db.Alternatives
                                    .Where(a => a.QuestionId == q.Id)
                                    .OrderBy(a => a.Id).ToList();
                                
                                foreach(var a in questionAlternatives)
                                {
                                    WriteLine($"[{questionAlternatives.IndexOf(a) + 1}] {a.AlternativeText}");
                                }

                                WriteColoredLine("\nQ. Quit to Menu", ConsoleColor.DarkRed);

                                // Ask for Answer
                                WriteLine();
                                Write("Answer: ");
                                userInputKey = ReadKey().KeyChar;
                            } while(!IsValidChoice(userInputKey, 4, "Q"));

                            if (userInputKey.ToString().ToUpper() == "Q")
                            {
                                WriteLine("\nQuitting to Menu...");
                                Thread.Sleep(1000);
                                RunQuizApp();
                            }


                            // Give feedback and points
                            if (questionAlternatives[Int16.Parse(userInputKey.ToString()) - 1].IsCorrect) {
                                WriteColoredLine($"\n\n {questionAlternatives[Int16.Parse(userInputKey.ToString()) -1].AlternativeText} is the Right Answer!\n", ConsoleColor.DarkGreen);
                                quizScore++;
                            }
                            else 
                            {
                                WriteColoredLine($"\n\n {questionAlternatives[Int16.Parse(userInputKey.ToString()) -1].AlternativeText} is the Wrong Answer!\n", ConsoleColor.DarkRed);
                            };

                            if (quizQuestions.IndexOf(q) + 1 != quizQuestions.Count)
                            {
                                WriteLine("[Enter] Next Question");
                            }
                            else
                            {
                                WriteColoredLine("[Enter] See Quiz Summary", ConsoleColor.Green);
                            }
                            
                            ReadLine();
                        }
                        
                        Clear();
                        WriteColoredLine("QUIZ SUMMARY\n", ConsoleColor.DarkGreen);
                        WriteLine($"Quiz Score: {quizScore}/{quizQuestions.Count()}\n");
                        WriteColoredLine("[Enter] Quit to Menu", ConsoleColor.Green);
                        ReadLine();

                        RunQuizApp();
                    }
                }
                catch
                {
                    WriteLine("Something went wrong");
                }
            } 
        }
    }
}