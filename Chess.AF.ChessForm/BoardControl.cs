using Chess.AF.Controllers;
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

namespace Chess.AF.ChessForm
{
    public partial class BoardControl : UserControl
    {
        private IGameController gameController;

        public bool IsReverse { get; private set; } = false;


        public BoardControl(IGameController gameController)
        {
            InitializeComponent();

            this.gameController = gameController;
            this.Controls.AddRange(GetSquares().ToArray());
        }

        private IEnumerable<SquareControl> GetSquares()
        {
            foreach (int id in Enumerable.Range(0, 64))
                yield return CreateSquareControl(id);
        }

        private SquareControl CreateSquareControl(int id)
        {
            var control = new SquareControl(id, gameController);
            control.Relocate(this.IsReverse);
            control.Margin = new Padding(0);
            control.Size = new Size(SquareWidth, SquareWidth);
            return control;
        }

        public void ReverseBoardView()
        {
            this.IsReverse = !this.IsReverse;
            foreach (SquareControl control in this.Controls)
                control.Relocate(this.IsReverse);
            this.Invalidate(true);
        }
    }
}
