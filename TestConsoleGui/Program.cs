using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleGui
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo("TestConsoleGuiIntermediate.exe")
            {
                //Arguments = "script.R",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            proc.Start();

            proc.StandardInput.WriteLine("[GUI] Hello");
            var output = proc.StandardOutput.ReadLine();
            Console.WriteLine($"{output} [GUI]");

            proc.Close();

            Console.ReadKey();
        }
    }
}
