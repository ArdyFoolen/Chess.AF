using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.AF.ChessForm
{
    public partial class ToolstripNumericUpDown : ToolStripControlHost
    {
        public ToolstripNumericUpDown() : base(new NumericUpDown()) { }

        public NumericUpDown NumericUpDownControl { get=> Control as NumericUpDown; }

        public decimal Minimum
        {
            get { return NumericUpDownControl.Minimum; }
            set { NumericUpDownControl.Minimum = value; }
        }

        public decimal Maximum
        {
            get { return NumericUpDownControl.Maximum; }
            set { NumericUpDownControl.Maximum = value; }
        }

        public int Value
        {
            get { return Convert.ToInt32(NumericUpDownControl.Value); }
            set { NumericUpDownControl.Value = value; }
        }

        //public override string Text
        //{
        //    get { return NumericUpDownControl.Text; }
        //    set { NumericUpDownControl.Text = value; }
        //}

        protected override void OnSubscribeControlEvents(Control c)
        {
            // Call the base so the base events are connected.
            base.OnSubscribeControlEvents(c);

            NumericUpDown numericUpDownControl = (NumericUpDown)c;
            numericUpDownControl.ValueChanged += new EventHandler(OValueChanged);
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(c);

            NumericUpDown numericUpDownControl = (NumericUpDown)c;
            numericUpDownControl.ValueChanged -= new EventHandler(OValueChanged);
        }

        public event EventHandler ValueChanged;
        private void OValueChanged(object sender, EventArgs e)
            => ValueChanged?.Invoke(this, e);
    }
}
