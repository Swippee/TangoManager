using System;

namespace TangoManagerAPI.Entities.Models
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
        public DateTime? DateDernierQuiz { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CarteEntity entity && entity.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        #endregion
    }
}
