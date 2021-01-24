using System;
using System.Threading;
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
            ConsoleKeyInfo userInputKey;
            string userInputString;

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

            userInputKey = Console.ReadKey();
            
            switch(userInputKey.Key)
            {
                // 1. QUIZZES
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    Clear();
                    WriteLine("QUIZZES\n");
                    WriteLine("# MENU");
                    WriteLine("1. Play Quiz");
                    WriteLine("2. Delete Quiz\n");
                    WriteLine("C. Cancel");
                    
                    // Ask for menu chouce
                    userInputKey = ReadKey();

                    // switch to handle input
                    switch(userInputKey.Key)
                    {
                        case ConsoleKey.D1:
                            Clear();

                            WriteLine("# PLAY QUIZ");
                            WriteLine("Choose Quiz Id from the list below\n");
                            WriteLine("## QUIZZES");

                            QuizHandler.GetQuizzes();

                            // Ask for quiz ID
                            userInputString = ReadLine();
                            break;

                        case ConsoleKey.D2:
                            Clear();
                            
                            WriteLine("# DELETE QUIZ");
                            WriteLine("Choose Quiz Id from the list below\n");
                            WriteLine("## QUIZZES");

                            QuizHandler.GetQuizzes();

                            WriteLine("\nC. Cancel\n");

                            // Ask for quiz ID
                            userInputString = ReadLine();
                            // QuizHandler.DeleteQuiz();
                            if(userInputString.ToUpper() == "C")
                            {                
                                RunQuizApp();
                            } 
                            else {
                                try {
                                    using (var db = new QuizAppContext()) {
                                        var quizToRemove = db.Quizzes
                                        .Where(q => q.Id == Int16.Parse(userInputString))
                                        .First();

                                        var questionsToRemove = db.Questions
                                            .Where(q => q.QuizId == quizToRemove.Id);
                                        
                                        db.Remove(quizToRemove);
                                        
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
                                catch {
                                    WriteLine("Something went wrong");
                                }
                            }
                            break;
                        
                        case ConsoleKey.C:
                            RunQuizApp();
                            break;

                        default:
                            break;
                    }


                    break;



                // 2. CREATE NEW QUIZ
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                // QuizHandler.CreateQuiz();
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
                            newQuestion.Alternatives.Add(new Alternative() {
                                AlternativeText = alternativeText,
                                IsCorrect = isCorrect
                            });
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
                    break;

                case ConsoleKey.Q:
                    Clear();
                    WriteLine("Quitting application...");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }
    }
}
      
