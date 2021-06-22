using Chess.AF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Controllers
{
    public class PgnController : IPgnController
    {
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
                view.UpdateView();
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
    }
}
