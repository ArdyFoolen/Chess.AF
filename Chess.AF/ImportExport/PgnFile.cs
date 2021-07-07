using AF.Functional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.ImportExport
{
    public class PgnFile
    {
        private string filePath { get; }
        private string pgnFileString { get; set; }
        private string[] pgnFiles;

        public PgnFile(string filePath)
        {
            this.filePath = filePath;
        }

        public void Read()
        {
            Using(new StreamReader(filePath), reader => pgnFileString = reader.ReadToEnd());
            var pgnFiles = pgnFileString.Split(new string[] { "[Event" }, StringSplitOptions.RemoveEmptyEntries);
            this.pgnFiles = pgnFiles.Map(p => $"[Event{p}").ToArray();
        }

        public void Write(Pgn pgn)
        {
            pgnFiles = new string[1]
            {
                pgn.PgnString
            };
            Using(new StreamWriter(filePath), writer => writer.Write(pgn.PgnString));
        }

        public string this[int index] { get => pgnFiles[index]; }

        public int Count()
            => pgnFiles.Length;
    }
}
