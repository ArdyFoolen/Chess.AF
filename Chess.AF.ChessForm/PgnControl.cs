using Chess.AF.Controllers;
using Chess.AF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.ChessForm
{
    public partial class PgnControl : UserControl, IPgnView
    {
        private IPgnController pgnController;
        List<Label> TagLabels = new List<Label>();
        private Font font = new Font(FontFamily.Families[0], 12, FontStyle.Regular);

        public PgnControl(IPgnController pgnController)
        {
            InitializeComponent();

            this.pgnController = pgnController;
            this.pgnController.Register(this);

            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoSize = true;
            UpdateView();
        }

        public void UpdateView()
        {
            this.Controls.Clear();
            TagLabels.Clear();
            AddTagPairDictionaryControls(pgnController.TagPairDictionary);
            this.Controls.AddRange(TagLabels.ToArray());
            this.Size = new Size(200, this.TagLabels.Sum(t => t.Size.Height));
            this.Visible = TagLabels.Any(a => true);
        }

        private void AddTagPairDictionaryControls(Dictionary<string, string> tagPairDictionary)
        {
            int y = 0;
            foreach (var kv in tagPairDictionary)
                AddTagPair(kv, ref y);
        }

        private void AddTagPair(KeyValuePair<string, string> kv, ref int y)
        {
            if (string.IsNullOrWhiteSpace(kv.Value) || "?".Equals(kv.Value.Trim()))
                return;
            if (y != 0)
            {
                TagLabels.Add(HorzLine(y));
                y = recalcY();
            }
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            TagLabels.Add(new Label() { Text = myTI.ToTitleCase(kv.Key), Location = new Point(0, y), Font = font, AutoSize = true });
            y = recalcY();
            TagLabels.Add(new Label() { Text = kv.Value, Location = new Point(10, y), Font = font, AutoSize = true });
            y = recalcY();
        }

        private Label HorzLine(int y)
        {
            Label label = new Label();
            label.Text = string.Empty;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.AutoSize = false;
            label.Height = 1;
            label.Location = new Point(0, y);
            label.Size = new Size(this.Size.Width, label.Size.Height);
            return label;
        }

        private int recalcY()
            => TagLabels[TagLabels.Count() - 1].Location.Y + TagLabels[TagLabels.Count() - 1].Size.Height;
    }
}
