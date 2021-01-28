using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

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
}