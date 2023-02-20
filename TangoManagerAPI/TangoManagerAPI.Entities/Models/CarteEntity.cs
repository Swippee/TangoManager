using System;

namespace TangoManagerAPI.Models
{
    public class CarteEntity
    {
        #region Properties
        public int Id { get; set; }
        public int PaquetId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Reponse { get; set; } = string.Empty;
        public decimal Score { get; set; }

        public DateTime DateCreation { get; set; }
        public DateTime DateDernierQuiz { get; set; }
        #endregion
    }
}
