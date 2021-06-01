using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm
{
    public static class ResourceHelper
    {
        private static ResourceManager rm = new ResourceManager("Chess.AF.ChessForm.Resources.ToolTips", Assembly.GetExecutingAssembly());

        public static string TooltipFen1
        {
            get => rm.GetString("ttFen1");
        }
        public static string TooltipFen2
        {
            get => rm.GetString("ttFen2");
        }
        public static string TooltipFen3
        {
            get => rm.GetString("ttFen3");
        }
        public static string TooltipFen4
        {
            get => rm.GetString("ttFen4");
        }
        public static string TooltipFen5
        {
            get => rm.GetString("ttFen5");
        }
        public static string TooltipFen6
        {
            get => rm.GetString("ttFen6");
        }
        public static string TooltipFenHistory
        {
            get => $"1: {TooltipFen1}\n2: {TooltipFen2}\n3: {TooltipFen3}\n4: {TooltipFen4}\n5: {TooltipFen5}\n6: {TooltipFen6}";
        }
    }
}
