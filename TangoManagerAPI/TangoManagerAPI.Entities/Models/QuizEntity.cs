using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TangoManagerAPI.Entities.Ports;

namespace TangoManagerAPI.Entities.Models
{
    [Serializable]
    public class QuizEntity : ICloneable<QuizEntity>
    {
        public QuizEntity()
        {

        }

        public QuizEntity (int currentCardId, string packetName)
        {
            if (string.IsNullOrEmpty(packetName))
                throw new ArgumentNullException(nameof(packetName), "Packet name cannot be null or empty!");

            CreationDate = DateTime.UtcNow;
            CurrentCardId = currentCardId;
            CurrentState = QuizActiveState.StateName;
            PacketName = packetName;
        }

        public int Id { get; set; }
        public string PacketName { get; set; }
        public int CurrentCardId { get; set; }
        public string CurrentState { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public decimal TotalScore { get; set; }
        public QuizEntity Clone()
        {
            using var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
            formatter.Serialize(ms, this);
#pragma warning restore SYSLIB0011
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011
            return (QuizEntity)formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011
        }
    }
}
