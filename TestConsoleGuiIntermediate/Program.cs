using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace TestConsoleGuiIntermediate
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo("Chess.AF.Console.UCIEngine.exe")
            {
                //Arguments = "script.R",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            proc.Start();

            var message = ReadLine();

            proc.StandardInput.WriteLine($"[Intermediate] {message}");
            var output = proc.StandardOutput.ReadLine();
            WriteLine($"{output} [Intermediate]");

            proc.Close();
        }
    }
}
