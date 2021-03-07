using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using AF.Functional;
using static AF.Functional.F;
using AF.Classes.Agent;
using Unit = System.ValueTuple;

namespace Chess.AF.UCIEngine
{
    // Link UCI protocol: https://gist.github.com/aliostad/f4470274f39d29b788c1b09519e67372

    class Program
    {
        static void Main(string[] args)
        {
            if (args.IsDebugOn())
                Debugger.Launch();

            var engine = new Engine();
            engine.Execute();
        }
    }
}
