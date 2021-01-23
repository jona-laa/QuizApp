using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuizApp
{
    public class Alternative
    {
        public int Id { get; set; }
        public string AlternativeText { get; set; }
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }        
    }
}