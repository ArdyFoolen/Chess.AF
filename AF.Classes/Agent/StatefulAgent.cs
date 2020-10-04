using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace AF.Classes.Agent
{
    public class StatefulAgent<State, Msg> : IAgent<Msg>
    {
        private State state;
        private readonly ActionBlock<Msg> actionBlock;

        public StatefulAgent(State initialState, Func<State, Msg, State> process)
        {
            state = initialState;

            actionBlock = new ActionBlock<Msg>(msg =>
            {
                var newState = process(state, msg);
                state = newState;
            });
        }

        public void Tell(Msg message)
            => actionBlock.Post(message);
    }
}
