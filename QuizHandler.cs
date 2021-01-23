// using System;
// using System.Linq;
// using static System.Console;
// using static Utils.Utilities;

// namespace QuizApp
// {
//     public class QuizHandler
//     {
//         /// <summary>
//         /// Logic for creating new quiz
//         /// </summary>
//         public static void CreateQuiz()
//         {
//             ConsoleKeyInfo userInput;
            
//             using (var db = new QuizAppContext()) {
//                     string quizTitle;

//                     // Ask for Quiz Name
//                     do
//                     {
//                         Clear();
//                         WriteLine("Create New Quiz\n");
//                         WriteLine("Quiz Name:");
//                         quizTitle = ReadLine();
//                     } while(!IsValidString(quizTitle));
                    
//                     // Save Quiz to DB
//                     db.Add(new Quiz(quizTitle));
//                     db.SaveChanges();


                    
//                     // CREATE NEW QUESTION
//                     int questionNum = 0;
//                     string questionText;
//                     do
//                     {
//                         // Ask for valid Question Text
//                         do
//                         {
//                             Clear();
//                             WriteLine(quizTitle + "\n");
//                             WriteLine($"Question {questionNum + 1}");
//                             questionText = ReadLine();
//                         } while(!IsValidString(questionText));
                        
//                         // Find Quiz to add Question to
//                         var newQuiz = db.Quizzes
//                             .Where(q => q.Title == quizTitle)
//                             .First();

//                         // Save Question to DB
//                         newQuiz.Questions.Add(new Question(questionText));
//                         db.SaveChanges();
//                         questionNum++;



//                         // CREATE FOUR ALTERNATIVES
//                         int i = 0;
//                         string alternativeText;
//                         string isCorrectInput;
//                         bool isCorrect;
//                         while(i<4)
//                         {
//                             // Ask for valid Alternative Text
//                             do
//                             {
//                                 Clear();
//                                 WriteLine($"Quiz: {quizTitle}\n");
//                                 WriteLine($"Question: {questionText}\n");
//                                 WriteLine($"Alternative {i + 1}");
//                                 alternativeText = ReadLine();
//                             } while(!IsValidString(alternativeText));

//                             // Ask for valid Boolean(Is Alternative true/false?)
//                             do
//                             {
//                                 Clear();
//                                 WriteLine($"Quiz: {newQuiz.Title}\n");
//                                 WriteLine($"Question: {questionText}\n");
//                                 WriteLine($"Alternative: {alternativeText}\n");
//                                 WriteLine("Is it the correct answer?(true/false)");
//                                 isCorrectInput = ReadLine();
//                             } while(!Boolean.TryParse(isCorrectInput, out isCorrect));

//                             // Find Question to add Alternatives to
//                             var newQuestion = db.Questions
//                                 .Where(q => q.QuestionText == questionText)
//                                 .First();

//                             // Save Alternatives to DB
//                             newQuestion.Alternatives.Add(new Alternative() {
//                                 AlternativeText = alternativeText.ToString(),
//                                 IsCorrect = isCorrect
//                             });
//                             db.SaveChanges();

//                             i++;
//                         }



//                         // QUESTION SUMMARY
//                         Clear();
//                         TextColor(ConsoleColor.DarkGreen);
//                         WriteLine($"# Quiz Summary: \n");
//                         ResetColor();
//                         WriteLine($"## Quiz Name: \n{quizTitle}\n");

//                         // Get Quiz Id and Quiz Questions
//                         var createdQuizId = db.Quizzes
//                             .Where(q => q.Title == quizTitle)
//                             .First().Id;

//                         var quizQuestions = db.Questions
//                                 .Where(q => q.QuizId == createdQuizId)
//                                 .OrderBy(q => q.Id);

//                         WriteLine("## Quiz Questions:");
//                         foreach(var q in quizQuestions)
//                         {
//                             WriteLine($"Question: {q.QuestionText}");
                            
//                             // Get Question Alternatives
//                             var questionAnswers = db.Alternatives
//                                 .Where(a => a.QuestionId == q.Id)
//                                 .OrderBy(a => a.Id);
                            
//                             WriteLine("\n## Question Alternatives:");
//                             foreach(var a in questionAnswers)
//                             {
//                                 WriteLine($"Alternative: {a.AlternativeText}, Is Correct: {a.IsCorrect}");
//                             }

//                             WriteLine("");
//                         }



//                         // Give option to add new question or save quiz
//                         WriteLine("1. New Question");
//                         WriteLine("2. Save Quiz");
//                         userInput = ReadKey();
//                     } while(userInput.Key != ConsoleKey.D2);

//                     ReadLine();
//                 }
//         }
//     }
// }