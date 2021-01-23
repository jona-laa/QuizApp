using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuizApp
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