using System.Collections.Generic;

namespace QuizApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public int QuizId { get; set; }
        public List<Alternative> Alternatives { get; } = new List<Alternative>();

        public Question(string questionText)
        {
            this.QuestionText = questionText;
        }
    }   
}

