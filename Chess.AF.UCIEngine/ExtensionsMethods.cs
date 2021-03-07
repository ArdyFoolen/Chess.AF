using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = System.ValueTuple;
using static AF.Functional.F;
using System.Reflection.Emit;
using AF.Classes.Agent;

namespace Chess.AF.UCIEngine
{
    public static class ExtensionsMethods
    {
        public static string[] SplitPositionsCommand(this string command)
            => command.Split(new string[] { "position", "moves" }, StringSplitOptions.RemoveEmptyEntries);

        public static string GetFenFromCommandParameter(this string cmdParm)
            => cmdParm.Trim().Split(new string[] { "fen" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

        public static bool IsDebugOn(this string[] args)
            => args.Contains("-debug", new CompareCultureInvariant());

        //public static Exceptional<string[]> ValidatePositionsCommand(this string[] cmdParams)
        //{
        //    if (cmdParams.Length != 2)
        //        return new Exception("Expected a start position and moves parameter");
        //    if (!cmdParams[0].StartsWith("startpos") && !cmdParams[0].StartsWith("fen"))
        //        return new Exception("Not a valid position");
        //    return cmdParams;
        //}

        //public static Exceptional<string[]> SetupPosition(this Exceptional<string[]> cmdParms, IAgent<Action<Game>> GameAgent)
        //    => cmdParms.Bind<string[], string[]>(
        //        parms => { parms[0].LoadPosition(GameAgent); return parms; }
        //        );


        //public static void LoadPosition(this string position, IAgent<Action<Game>> GameAgent)
        //{
        //    if (position.StartsWith("startpos"))
        //        GameAgent.Tell(g => g.Load());
        //    else
        //        GameAgent.Tell(g => g.Load(position.Split(new string[] { "fen" }, StringSplitOptions.RemoveEmptyEntries)[1]));
        //}
    }
}
