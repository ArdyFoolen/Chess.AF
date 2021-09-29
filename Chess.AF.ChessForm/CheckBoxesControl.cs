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

        public void AddCheckBox(Image image, EventHandler clickEvent)
        {
            checkBoxes.Add(CreateCheckBox(image, clickEvent));
            this.Size = new Size(CheckboxesWidth, CheckBoxSize.Height);
        }

        public int Count { get => checkBoxes.Count(); }

        public Size CheckBoxSize
        {
            get => checkBoxes.Count() > 0 ? checkBoxes[0].Size : new Size(0, 0);
            set
            {
                foreach (var c in checkBoxes)
                    c.Size = value;
            }
        }

        private CheckBox CreateCheckBox(Image image, EventHandler clickEvent)
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

            this.Controls.Add(checkBox);
            this.ResumeLayout(false);
            this.PerformLayout();

            return checkBox;
        }

        private void CheckBox_Click(object sender, EventArgs e)
        {
            var chkBox = sender as CheckBox;
            checkBoxes.Where(w => !w.Name.Equals(chkBox.Name)).ToList().ForEach(c => c.Checked = false);
        }

        private Point GetNextLocation()
            => new Point(CheckboxesWidth, 5);

        private int CheckboxesWidth
        {
            get
            {
                var start = lblDescription.Visible ? 100 : 0;
                return start + CheckBoxSize.Width * Count;
            }
        }

        private string FormatName()
            => $"chkName-{Count}";
    }
}
