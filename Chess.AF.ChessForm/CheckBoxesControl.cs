using Chess.AF.ChessForm.Helpers;
using Chess.AF.Controllers;
using Chess.AF.Domain;
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
    public partial class CheckBoxesControl : UserControl
    {
        private List<CheckBox> checkBoxes = new List<CheckBox>();
        private CheckBoxesControl linkedControl;

        public CheckBoxesControl()
        {
            InitializeComponent();

            this.Size = new Size(300, 33);

            lblDescription.Size = new Size(100, 23);
            lblDescription.Location = new Point(0, 5);
            lblDescription.Font = new Font(FontFamily.Families[0], 16, FontStyle.Regular);
            lblDescription.Paint += FontHelper.Label_Paint;
            lblDescription.Visible = false;
        }

        public bool RadioStyle { get; set; } = false;

        public void LinkTo(CheckBoxesControl control)
            => linkedControl = control;

        public void SetLabelText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                lblDescription.Visible = false;
            else
            {
                lblDescription.Text = text;
                lblDescription.Visible = true;
            }
        }

        public void AddCheckBox(Image image, EventHandler clickEvent, bool isChecked = false)
        {
            checkBoxes.Add(CreateCheckBox(image, clickEvent, isChecked));
            this.Size = new Size(CheckboxesWidth, CheckBoxSize.Height);
        }

        public int Count { get => checkBoxes.Count(); }

        public Size CheckBoxSize
        {
            get => checkBoxes.Count() > 0 ? checkBoxes[0].Size : new Size(0, 0);
            set
            {
                this.SuspendLayout();
                checkBoxes = checkBoxes.Select((c, i) => { c.Size = value; c.Location = GetLocationForIndex(i); return c; }).ToList();
                this.Size = new Size(CheckboxesWidth, CheckBoxSize.Height);
                this.ResumeLayout(false);
            }
        }

        public void SetCheckedFor(int index)
            => checkBoxes[index].Checked = true;

        private CheckBox CreateCheckBox(Image image, EventHandler clickEvent, bool isChecked = false)
        {
            CheckBox checkBox = new CheckBox();
            this.SuspendLayout();
            // 
            // chkBoth
            // 
            checkBox.Appearance = Appearance.Button;
            checkBox.AutoSize = true;
            checkBox.Location = GetNextLocation();
            checkBox.Name = FormatName();
            checkBox.Size = new Size(33, 33);
            checkBox.TabIndex = Count + 1;
            checkBox.UseVisualStyleBackColor = true;

            checkBox.Image = image;
            checkBox.Click += CheckBox_Click;
            checkBox.Click += clickEvent;
            checkBox.Checked = isChecked;

            this.Controls.Add(checkBox);
            this.ResumeLayout(false);
            this.PerformLayout();

            return checkBox;
        }

        private void CheckBox_Click(object sender, EventArgs e)
        {
            var chkBox = sender as CheckBox;
            if (RadioStyle && !chkBox.Checked)
                chkBox.Checked = true;
            else
            {
                checkBoxes.Where(w => !w.Name.Equals(chkBox.Name)).ToList().ForEach(c => c.Checked = false);
                linkedControl?.checkBoxes.Where(w => !w.Name.Equals(chkBox.Name)).ToList().ForEach(c => c.Checked = false);
            }
        }

        private Point GetLocationForIndex(int index)
            => new Point(WidthForIndex(index), 5);

        private Point GetNextLocation()
            => new Point(CheckboxesWidth, 5);

        private int CheckboxesWidth { get => WidthForIndex(Count); }

        private int WidthForIndex(int index)
        {
            var start = lblDescription.Visible ? 100 : 0;
            return start + CheckBoxSize.Width * index;
        }

        private string FormatName()
            => $"chkName-{Guid.NewGuid()}";
    }
}
