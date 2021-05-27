using Chess.AF.ChessForm.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Chess.AF.ChessForm.ChessConstants;
using static Chess.AF.ChessForm.ImageHelper;

namespace Chess.AF.ChessForm
{
    public partial class ChessFrm : Form
    {
        private BoardControl boardControl;
        public ChessFrm()
        {
            InitializeComponent();

            this.Size = new Size(this.Size.Width, FormHeight);

            this.boardControl = new BoardControl(new BoardController());
            this.boardControl.BackColor = Color.SaddleBrown;
            this.boardControl.Size = new Size(BoardWidth, BoardWidth);
            this.boardControl.Margin = new Padding(0);
            this.boardControl.Location = new Point(0, 33);
            this.boardControl.BorderStyle = BorderStyle.FixedSingle;

            this.BackColor = Color.Wheat;
            Button button = new Button();
            button.Image = BlackKing();
            button.Image = BlackKing();

            button.Location = new Point(600, 50);
            button.Size = new Size(500 / 6, 90);

            this.Controls.Add(this.boardControl);
            this.Controls.Add(button);

            this.btnLoadFen.Image = Fen();
        }

        private void BtnLoadFen_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Fen: {boardControl.ToFenString()}");
        }
    }
}
