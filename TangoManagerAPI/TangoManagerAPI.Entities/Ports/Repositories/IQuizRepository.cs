using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Ports.Repositories
{
    public interface IQuizRepository
    {
        Task<QuizAggregate> GetQuizByIdAsync(int id);
        Task SaveQuizAsync(QuizAggregate quizAggregate);
    }
}
