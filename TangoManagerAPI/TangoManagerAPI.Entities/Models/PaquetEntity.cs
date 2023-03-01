using System;
using System.Collections.Generic;

namespace TangoManagerAPI.Entities.Models
{
    public class PaquetEntity
    {
        #region Properties
        public string Nom { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime DateCreation { get; set; }
        public DateTime? DateDernierQuiz { get; set; }

        public ICollection<CarteEntity> Cartes { get; set; } = new List<CarteEntity>();

        #endregion
    }

}
