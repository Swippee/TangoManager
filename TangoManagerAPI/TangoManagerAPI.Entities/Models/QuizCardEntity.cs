using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TangoManagerAPI.Entities.Ports;

namespace TangoManagerAPI.Entities.Models
{
    [Serializable]
    public class QuizCardEntity : ICloneable<QuizCardEntity>
    {
        public QuizCardEntity(int cardId, int quizId, bool isCorrect)
        {
            CardId = cardId;
            QuizId = quizId;
            IsCorrect = isCorrect;
        }

        public QuizCardEntity()
        {

        }

        public int CardId { get; set; }
        public int QuizId { get; set; }
        public bool IsCorrect { get; set; }
        //        public QuizCardEntity Clone()
        //        {
        //            using var ms = new MemoryStream();
        //            var formatter = new BinaryFormatter();
        //#pragma warning disable SYSLIB0011
        //            formatter.Serialize(ms, this);
        //#pragma warning restore SYSLIB0011
        //            ms.Position = 0;
        //            ms.Seek(0, SeekOrigin.Begin);
        //#pragma warning disable SYSLIB0011
        //            return (QuizCardEntity)formatter.Deserialize(ms);
        //#pragma warning restore SYSLIB0011
        //        }
        public QuizCardEntity Clone()
        {
            var quizCard = (QuizCardEntity)MemberwiseClone();
            return quizCard;
        }
    }
}
