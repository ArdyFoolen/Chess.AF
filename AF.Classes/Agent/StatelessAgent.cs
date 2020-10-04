using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace AF.Classes.Agent
{
    public class StatelessAgent<Msg> : IAgent<Msg>
    {
        private readonly ActionBlock<Msg> actionBlock;

        public StatelessAgent(Action<Msg> process)
        {
            actionBlock = new ActionBlock<Msg>(msg =>
            {
                process(msg);
            });
        }

        public void Tell(Msg message)
            => actionBlock.Post(message);
    }
}
