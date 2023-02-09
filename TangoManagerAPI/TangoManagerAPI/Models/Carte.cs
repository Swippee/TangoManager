namespace TangoManagerAPI.Models
{
    public class Carte
    {
        #region Properties
        public int Id { get; set; }
        public int PaquetId { get; set; }
        public string Question { get; set; }
        public string Reponse { get; set; }
        public decimal Score { get; set; }

        public DateTime DateCreation { get; set; }
        public DateTime DateDernierQuiz { get; set; }
        #endregion
    }
}
