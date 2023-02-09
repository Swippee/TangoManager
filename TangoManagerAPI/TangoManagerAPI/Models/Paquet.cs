namespace TangoManagerAPI.Models
{
    public class Paquet
    {
        #region Properties
        public int Id { get; set; }
        public string Nom { get; set; }

        public string? Description { get; set; }
        public decimal Score { get; set; }

        public DateTime DateCreation { get; set; }
        public DateTime DateDernierQuiz { get; set; }

        public List<Carte> cartes { get; set; } = new List<Carte>();

        #endregion
    }
}
