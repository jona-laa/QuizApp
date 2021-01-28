namespace QuizApp.Models
{
    public class Alternative
    {
        public int Id { get; set; }
        public string AlternativeText { get; set; }
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }

        public Alternative(string alternativeText, bool isCorrect)
        {
            this.AlternativeText = alternativeText;
            this.IsCorrect = isCorrect;
        }        
    }
}
