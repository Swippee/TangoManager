namespace TangoManagerAPI.Entities.Models
{
    public class QuizCardEntity
    {
        public QuizCardEntity(int cardId, int quizId, bool isCorrect)
        {
            CardId = cardId;
            QuizId = quizId;
            IsCorrect = isCorrect;
        }

        public QuizCardEntity()
        {

        }

        public int CardId { get; set; }
        public int QuizId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
