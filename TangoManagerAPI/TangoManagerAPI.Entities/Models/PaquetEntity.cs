using System;

namespace TangoManagerAPI.Entities.Models
{
    public class PaquetEntity
    {
        public PaquetEntity()
        {

        }

        public static PaquetEntity Create(string packetName, string description)
        {
            if (string.IsNullOrEmpty(packetName))
                throw new ArgumentNullException(nameof(packetName), "Packet name cannot be null or empty!");

            return new PaquetEntity
            {
                DateCreation = DateTime.UtcNow,
                Nom = packetName,
                Description = description
            };
        }

        public string Nom { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public DateTime? DateDernierQuiz { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PaquetEntity packet && string.Equals(packet.Nom, Nom, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nom);
        }
    }

}
