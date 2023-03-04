using System;

namespace TangoManagerAPI.Entities.Models
{
    public class QuizEntity
    {
        public QuizEntity()
        {

        }

        public static QuizEntity Create(int currentCardId, string packetName)
        {
            if (string.IsNullOrEmpty(packetName))
                throw new ArgumentNullException(nameof(packetName), "Packet name cannot be null or empty!");

            return new QuizEntity
            {
                CreationDate = DateTime.UtcNow,
                CurrentCardId = currentCardId,
                CurrentState = QuizActiveState.StateName,
                PacketName = packetName
            };
        }

        public int Id { get; set; }
        public string PacketName { get; set; }
        public int CurrentCardId { get; set; }
        public string CurrentState { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public decimal TotalScore { get; set; }
    }
}
