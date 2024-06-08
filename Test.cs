using Games.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games
{
    class Test : Interface1
    {
        public int StartGame()
        {
            return database.GetHighestQuizScore();
        }
    }
}
