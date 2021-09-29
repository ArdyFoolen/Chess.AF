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
    public partial class CheckBoxControl : UserControl
    {
        public CheckBoxControl()
        {
            InitializeComponent();

            this.Size = new Size(300, 33);

            lblDescription.Text = "Loose pieces";
            lblDescription.Size = new Size(100, 23);
            lblDescription.Location = new Point(0, 5);
            lblDescription.Font = new Font(FontFamily.Families[0], 16, FontStyle.Regular);
            lblDescription.Paint += FontHelper.Label_Paint;

            chkBoth.Size = new Size(33, 33);
            chkBoth.Location = new Point(110, 5);
            chkBoth.Image = ImageHelper.BlackWhiteQueenSmall();
            chkBoth.Click += chkBoth_Click;

            chkWhite.Size = new Size(33, 33);
            chkWhite.Location = new Point(143, 5);
            chkWhite.Image = ImageHelper.WhiteQueenSmall();
            chkWhite.Click += chkWhite_Click;

            chkBlack.Size = new Size(33, 33);
            chkBlack.Location = new Point(176, 5);
            chkBlack.Image = ImageHelper.BlackQueenSmall();
            chkBlack.Click += chkBlack_Click;
        }

        private void chkBlack_Click(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            uncheckOthersAndUseIterator(chk, FilterFlags.Black);
        }

        private void chkWhite_Click(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            uncheckOthersAndUseIterator(chk, FilterFlags.White);
        }

        private void chkBoth_Click(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            uncheckOthersAndUseIterator(chk, FilterFlags.Both);
        }

        private void uncheckOthersAndUseIterator(CheckBox chk, FilterFlags flags)
        {
            if (!chkBoth.Name.Equals(chk.Name))
                chkBoth.Checked = false;
            if (!chkWhite.Name.Equals(chk.Name))
                chkWhite.Checked = false;
            if (!chkBlack.Name.Equals(chk.Name))
                chkBlack.Checked = false;
            ClickHandler?.Invoke(chk.Checked, flags);
        }

        public Action<bool, FilterFlags> ClickHandler { private get; set; }

        public Image BlackWhiteImage { set { chkBoth.Image = value; } }
        public Image WhiteImage { set { chkWhite.Image = value; } }
        public Image BlackImage { set { chkBlack.Image = value; } }
    }
}
