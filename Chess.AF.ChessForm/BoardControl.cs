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
        private IGameController boardController;

        public bool IsReverse { get; private set; } = false;


        public BoardControl(IGameController boardController)
        {
            InitializeComponent();

            this.boardController = boardController;
            this.Controls.AddRange(GetSquares().ToArray());
        }

        private IEnumerable<SquareControl> GetSquares()
        {
            foreach (int id in Enumerable.Range(0, 64))
                yield return CreateSquareControl(id);
        }

        private SquareControl CreateSquareControl(int id)
        {
            var control = new SquareControl(id, boardController);
            //control.Location = GetLocation(id);
            control.Relocate(this.IsReverse);
            control.Margin = new Padding(0);
            control.Size = new Size(SquareWidth, SquareWidth);
            return control;
        }

        //private Point GetLocation(int id)
        //    => new Point(GetX(id) * SquareWidth, GetY(id) * SquareWidth);

        //private int GetX(int id)
        //    => GetRelativeLocation(id) % 8;

        //private int GetY(int id)
        //    => GetRelativeLocation(id) / 8;

        //private int GetRelativeLocation(int id)
        //    => IsReverse ? 63 - id : id;

        public void ReverseBoardView()
        {
            this.IsReverse = !this.IsReverse;
            foreach (SquareControl control in this.Controls)
                //control.Location = GetLocation(control.Id);
                control.Relocate(this.IsReverse);
            this.Invalidate(true);
        }
    }
}
