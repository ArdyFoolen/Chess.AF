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
    public partial class DlgPromote : Form
    {
        public DlgPromote()
        {
            InitializeComponent();

            this.Size = new Size(70, 70);
            PromoteControl promoteBlack = new PromoteControl(true);
            promoteBlack.Location = new Point(0, 0);
            promoteBlack.Size = new Size(70, 70);

            //PromoteControl promoteWhite = new PromoteControl(false);
            //promoteWhite.Location = new Point(600, 200);
            //promoteWhite.Size = new Size(70, 80);

            this.Controls.Add(promoteBlack);
            //this.Controls.Add(promoteWhite);
        }
    }
}
