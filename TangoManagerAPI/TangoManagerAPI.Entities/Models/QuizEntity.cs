using System;
using System.Collections.Generic;
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
            CreationDate = DateTime.UtcNow;
            CurrentState = QuizActiveState.StateName;
            PacketName = packetName;
            CurrentCardId = currentCardId;
        }

        public int? Id { get; set; }
        public string PacketName { get; set; }
        public int CurrentCardId { get; set; }
        public string CurrentState { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastModification { get; set; }
        public decimal TotalScore { get; set; }

        public ICollection<QuizCardEntity> QuizCardsCollection { get; set; } = new List<QuizCardEntity>();

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
