using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoManagerAPI.Entities.Models
{
    public interface IQuizState
    {
        public void Answer(string answer);
    }
}
