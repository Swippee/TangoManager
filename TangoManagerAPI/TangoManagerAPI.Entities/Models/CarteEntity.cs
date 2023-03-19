using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TangoManagerAPI.Entities.Ports;

namespace TangoManagerAPI.Entities.Models
{
    [Serializable]
    public class CarteEntity : ICloneable<CarteEntity>
    {

        public CarteEntity()
        {

        }

        public CarteEntity(string packetName, string question, string answer, decimal score)
        {
            if (string.IsNullOrEmpty(packetName))
                throw new ArgumentNullException(nameof(packetName), "PacketName cannot be null or empty!");

            if (string.IsNullOrEmpty(question))
                throw new ArgumentNullException(nameof(question), "Question cannot be null or empty!");

            if (string.IsNullOrEmpty(answer))
                throw new ArgumentNullException(nameof(answer), "Answer cannot be null or empty!");

            PacketName = packetName;
            Question = question;
            Answer = answer;
            Score = score;
            LastModification = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string PacketName { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public decimal Score { get; set; }

        public DateTime LastModification { get; set; }
        public DateTime? LastQuiz { get; set; }

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
            return obj is CarteEntity entity && entity.Id == Id && string.Equals(entity.Question, Question, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Question);
        }
    }
}
