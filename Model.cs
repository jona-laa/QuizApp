using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuizApp
{
    public class QuizAppContext : DbContext
    {        
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Alternative> Alternatives { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=QuizApp.db");
    }

    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Question> Questions = new List<Question>();

        public Quiz(string title)
        {
            this.Title = title;
        }

        public void AddQuestion(Question question)
        {
            Questions.Add(question);
        }
    }

    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public int QuizId { get; set; }
        public List<Alternative> Alternatives = new List<Alternative>();

        public void AddAlternative(Alternative alternative)
        {
            Alternatives.Add(alternative);
        }
    }

    public class Alternative
    {
        public int Id { get; set; }
        public string AlternativeText { get; set; }
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }        
    }
}