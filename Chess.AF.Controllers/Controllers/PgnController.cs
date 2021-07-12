using AF.Functional;
using Chess.AF.ImportExport;
using Chess.AF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.Controllers
{
    public class PgnController : IPgnController
    {
        private PgnFile pgnFile;
        private List<IPgnView> views = new List<IPgnView>();
        public Dictionary<string, string> TagPairDictionary { get; private set; } = new Dictionary<string, string>();

        public void Register(IPgnView view)
        {
            if (!views.Contains(view))
                views.Add(view);
        }

        public void UnRegister(IPgnView view)
        {
            if (views.Contains(view))
                views.Remove(view);
        }

        private void NotifyViews()
        {
            foreach (var view in views)
                view.UpdateFromPgn();
        }

        public void SetTagPairDictionary(Dictionary<string, string> tagPairDictionary)
        {
            this.TagPairDictionary = tagPairDictionary;
            NotifyViews();
        }
        public void Clear()
        {
            this.TagPairDictionary.Clear();
            NotifyViews();
        }

        public Option<Pgn> Read(string pgnFilePath)
        {
            Option<Pgn> pgn = None;
            this.pgnFile = new PgnFile(pgnFilePath);
            pgnFile.Read();
            if (pgnFile.Count() > 0)
            {
                pgn = Pgn.Import(pgnFile[0]);
                SetTagPairDictionary(pgn);
            }
            else
                NotifyViews();
            return pgn;
        }

        public void Write(Option<Pgn> pgn, string pgnFilePath)
            => pgn.Map(p => write(p, pgnFilePath));

        public void WriteAndAdd(Option<Pgn> pgn, string pgnFilePath)
            => pgn.Map(p => WriteAndAdd(p, pgnFilePath));

        public int Count()
            => pgnFile?.Count() ?? 0;

        public Option<Pgn> PgnFileIndexChanged(int index)
        {
            if (index >= pgnFile.Count())
                return None;

            var pgn = Pgn.Import(pgnFile[index]);
            SetTagPairDictionary(pgn);

            return pgn;
        }

        private Unit write(Pgn pgn, string pgnFilePath)
        {
            this.pgnFile = new PgnFile(pgnFilePath);
            this.pgnFile.Write(pgn);
            SetTagPairDictionary(pgn);

            return Unit();
        }

        private Unit WriteAndAdd(Pgn pgn, string pgnFilePath)
        {
            if (this.pgnFile == null)
                this.pgnFile = new PgnFile(pgnFilePath);
            this.pgnFile.WriteAndAdd(pgn);
            SetTagPairDictionary(pgn);

            return Unit();
        }

        private void SetTagPairDictionary(Option<Pgn> pgn)
            => pgn.Map(p => SetTagPairDictionary(p));

        private Unit SetTagPairDictionary(Pgn pgn)
        {
            SetTagPairDictionary(pgn.TagPairDictionary);
            return Unit();
        }
    }
}
