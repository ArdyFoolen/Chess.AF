using System;
using System.Collections.Generic;
using System.Text;

namespace AF.Classes.Agent
{
    public interface IAgent<Msg>
    {
        void Tell(Msg message);
    }
}
