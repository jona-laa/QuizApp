using System.Collections.Generic;

namespace QuizApp.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; } = new List<Question>();

        public Quiz(string title)
        {
            this.Title = title;
        }
    }
}
