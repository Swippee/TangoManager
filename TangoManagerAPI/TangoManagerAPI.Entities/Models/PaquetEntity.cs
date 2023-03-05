using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TangoManagerAPI.Entities.Ports;

namespace TangoManagerAPI.Entities.Models
{
    [Serializable]
    public class PaquetEntity : ICloneable<PaquetEntity>
    {
        public PaquetEntity()
        {

        }

        public PaquetEntity (string packetName, string description)
        {
            if (string.IsNullOrEmpty(packetName))
                throw new ArgumentNullException(nameof(packetName), "Packet name cannot be null or empty!");

            DateCreation = DateTime.UtcNow;
            Nom = packetName;
            Description = description;
        }

        public string Nom { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public DateTime? DateDernierQuiz { get; set; }

        public ICollection<CarteEntity> CardsCollection { get; set; } = new List<CarteEntity>();

        public PaquetEntity Clone()
        {
            using var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            formatter.Serialize(ms, this);
#pragma warning restore SYSLIB0011
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011
            return (PaquetEntity)formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011
        }

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
