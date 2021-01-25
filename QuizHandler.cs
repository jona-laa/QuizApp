using System;
using System.Linq;
using static System.Console;
using static Utils.Utilities;
using System.Collections.Generic;
using System.Threading;
using static QuizApp.Program;



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
                        newQuestion.Alternatives.Add(new Alternative(alternativeText, isCorrect));
                        db.SaveChanges();

                        i++;
                    }



                    // QUESTION SUMMARY
                    Clear();
                    TextColor(ConsoleColor.DarkGreen);
                    WriteLine($"# Quiz Summary: \n");
                    ResetColor();
                    WriteLine($"## Quiz Name: \n{quizTitle}\n");

                    // Get Quiz Id and Quiz Questions
                    var createdQuizId = db.Quizzes
                        .Where(q => q.Title == quizTitle)
                        .First().Id;

                    var quizQuestions = db.Questions
                            .Where(q => q.QuizId == createdQuizId)
                            .OrderBy(q => q.Id);

                    WriteLine("## Quiz Questions:");
                    foreach(var q in quizQuestions)
                    {
                        WriteLine($"Question: {q.QuestionText}");
                        
                        // Get Question Alternatives
                        var questionAnswers = db.Alternatives
                            .Where(a => a.QuestionId == q.Id)
                            .OrderBy(a => a.Id);
                        
                        WriteLine("\n## Question Alternatives:");
                        foreach(var a in questionAnswers)
                        {
                            WriteLine($"Alternative: {a.AlternativeText}, Is Correct: {a.IsCorrect}");
                        }

                        WriteLine("");
                    }



                    // ADD NEW QUESTION OR SAVE QUIZ
                    WriteLine("1. New Question");
                    WriteLine("2. Save Quiz");
                    userInputKey = ReadKey();
                } while(userInputKey.Key != ConsoleKey.D2);

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
                        WriteLine("No Quizzes at this moment. Create one!");
                        ReadLine();
                        return null;
                    }
                }
            } 
            catch 
            {
                WriteLine("Error fetching Quizzes. Press any key.");
                ReadLine();
            }
            return null;
        }



        /// <summary>
        /// Delete Quiz
        /// </summary>
        public static void DeleteQuiz()
        {
            ConsoleKeyInfo userInputKey;
            List<Quiz> quizzes;

            do 
            {
                Clear();
                TextColor(ConsoleColor.DarkGreen);                    
                WriteLine("# DELETE QUIZ");
                WriteLine("Choose Quiz Id from the list below\n");
                ResetColor();
                
                WriteLine("## QUIZZES");
                quizzes = GetQuizzes();
                foreach(var q in quizzes) {
                    WriteLine($"[{quizzes.IndexOf(q)}] {q.Title}");
                }

                TextColor(ConsoleColor.DarkRed);    
                WriteLine("\nC. Cancel\n");
                ResetColor();

                // Ask for quiz ID
                // userInputString = ReadLine();
                userInputKey = ReadKey();
            } while(!IsValidChoice(userInputKey.KeyChar, quizzes.Count - 1, "C"));

            // CANCEL
            if(userInputKey.KeyChar.ToString().ToUpper() == "C")
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
                            .Where(q => q.Id == quizzes[Int16.Parse(userInputKey.KeyChar.ToString())].Id)
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
                        WriteLine("\n\nDeleting Quiz...");
                        Thread.Sleep(1000); 
                    }
                } 
                catch 
                {
                    WriteLine("Something went wrong");
                }
            }
        }



        public static void PlayQuiz()
        {
            ConsoleKeyInfo userInputKey;
            List<Quiz> quizzes;
            List<Alternative> questionAlternatives;

            do {
                Clear();
                TextColor(ConsoleColor.DarkGreen);
                WriteLine("Choose Quiz from the list below\n");
                ResetColor();

                quizzes = QuizHandler.GetQuizzes();
                foreach(var q in quizzes) {
                    WriteLine($"[{quizzes.IndexOf(q)}] {q.Title}");
                }

                TextColor(ConsoleColor.DarkRed);    
                WriteLine("\nC. Cancel\n");
                ResetColor();

                // Ask for Quiz Index
                userInputKey = ReadKey();
            } while(!IsValidChoice(userInputKey.KeyChar, quizzes.Count - 1, "C"));

            // CANCEL
            if(userInputKey.KeyChar.ToString().ToUpper() == "C")
            {                
                RunQuizApp();
            }
            // RUN QUIZ
            else
            {
                int quizScore = 0;
                try
                {
                    using (var db = new QuizAppContext()) 
                    {
                        // Get Quiz Id...
                        var quiz = db.Quizzes
                            .Where(q => q.Id == quizzes[Int16.Parse(userInputKey.KeyChar.ToString())].Id)
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
                                WriteLine($"## Question {quizQuestions.IndexOf(q) + 1} / {quizQuestions.Count}\n");
                                WriteLine($"{q.QuestionText}\n");
                                
                                // Get Question Alternatives
                                questionAlternatives = db.Alternatives
                                    .Where(a => a.QuestionId == q.Id)
                                    .OrderBy(a => a.Id).ToList();
                                
                                foreach(var a in questionAlternatives)
                                {
                                    WriteLine($"[{questionAlternatives.IndexOf(a)}] {a.AlternativeText}");
                                }

                                WriteLine("\nQ. Quit to Menu\n");

                                // Ask for Answer
                                WriteLine();
                                Write("Answer: ");
                                userInputKey = ReadKey();
                            } while(!IsValidChoice(userInputKey.KeyChar, 3, "Q"));

                            if (userInputKey.KeyChar.ToString().ToUpper() == "Q")
                            {
                                WriteLine("\nQuitting to Menu...");
                                Thread.Sleep(1000);
                                RunQuizApp();
                            }


                            // Give feedback and points
                            if (questionAlternatives[Int16.Parse(userInputKey.KeyChar.ToString())].IsCorrect) {
                                WriteLine($"\n{questionAlternatives[Int16.Parse(userInputKey.KeyChar.ToString())].AlternativeText} is the right Answer!\n");
                                quizScore++;
                            }
                            else 
                            {
                                WriteLine("\nWrong Answer!\n");
                            };

                            if (quizQuestions.IndexOf(q) + 1 != quizQuestions.Count)
                            {
                                WriteLine("[Enter] Next Question");
                            }
                            else
                            {
                                WriteLine("[Enter] See Quiz Summary");
                            }
                            
                            ReadLine();
                        }
                        
                        Clear();
                        WriteLine("QUIZ SUMMARY\n");
                        WriteLine($"Quiz Score: {quizScore}/{quizQuestions.Count()}\n");
                        WriteLine("[Enter] Quit to Menu");
                        ReadLine();
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