using System;
using System.Collections.Generic;
using System.Text;

namespace AF.Classes.Agent
{
    public static class Agent
    {
        public static IAgent<Msg> Start<State, Msg>(State initialState, Func<State, Msg, State> process)
            => new StatefulAgent<State, Msg>(initialState, process);
        public static IAgent<Msg> Start<Msg>(Action<Msg> action)
            => new StatelessAgent<Msg>(action);
    }
}
