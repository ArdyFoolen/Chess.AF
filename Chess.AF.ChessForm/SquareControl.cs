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
using static Chess.AF.ChessForm.ImageHelper;
using static AF.Functional.F;
using AF.Functional;
using static Chess.AF.ChessForm.ChessConstants;
using Chess.AF.ChessForm.Extensions;

namespace Chess.AF.ChessForm
{
    public partial class SquareControl : UserControl
    {
        private IBoardController boardController;

        public int Id { get; private set; }
        public SquareControl(int id, IBoardController boardController)
        {
            InitializeComponent();

            btnImage.Size = new Size(SquareWidth, SquareWidth);

            this.boardController = boardController;
            this.Id = id;

            boardController[Id].Map(
                s => btnImage.Image = GetPieceDict[(int)s.Piece]()
                );
        }

        private void btnImage_MouseLeave(object sender, EventArgs e)
        {
            btnImage.BackColor = this.BackColor;
        }

        private void btnImage_MouseEnter(object sender, EventArgs e)
        {
            btnImage.BackColor = this.BackColor.ChangeColorBrightness(1.5f);
        }
    }
}
