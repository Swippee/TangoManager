using System;
using System.Collections.Generic;

namespace TangoManagerAPI.Entities.Models
{
    public class PaquetEntity
    {
        #region Properties
       // public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public int? Score { get; set; }

        public string DateCreation { get; set; }
        public string? DateDernierQuiz { get; set; }

        public ICollection<CarteEntity> Cartes { get; set; } = new List<CarteEntity>();

        #endregion
    }

}
