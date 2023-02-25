using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Exceptions;

namespace TangoManagerAPI.Entities.Models
{
    public  class QuizFinishedState : IQuizState
    {
        public void Answer(string answer)
        {
            throw new QuizAlreadyFinishedException("Quiz has already been finished!");
        }
    }
}
