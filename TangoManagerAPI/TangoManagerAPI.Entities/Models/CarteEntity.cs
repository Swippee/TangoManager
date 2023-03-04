using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TangoManagerAPI.Entities.Ports;

namespace TangoManagerAPI.Entities.Models
{
    [Serializable]
    public class CarteEntity : ICloneable<CarteEntity>
    {
        
        public int Id { get; set; }
        public string PaquetNom { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Reponse { get; set; } = string.Empty;
        public decimal Score { get; set; }

        public DateTime DateCreation { get; set; }
        public DateTime? DateDernierQuiz { get; set; }

        public CarteEntity Clone()
        {
            using var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            formatter.Serialize(ms, this);
#pragma warning restore SYSLIB0011
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011
            return (CarteEntity)formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011
        }

        public override bool Equals(object obj)
        {
            return obj is CarteEntity entity && entity.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
