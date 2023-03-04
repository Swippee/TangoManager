using System.Collections.Generic;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Ports.Repositories
{
    public interface IQuizRepository
    {
        Task<QuizEntity> GetQuizByIdAsync(int id);
        Task SaveQuizAsync(QuizEntity quizEntity);
        Task SaveQuizCard(QuizCardEntity quizCardEntity);
        Task<IEnumerable<QuizCardEntity>> GetQuizCardsByQuizIdAsync(int quizId);
    }
}
