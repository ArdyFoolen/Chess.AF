using Chess.AF.ChessForm.Factories;
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

namespace Chess.AF.ChessForm.Controls
{
    public partial class BoardControl : UserControl
    {
        private ISquareControlFactory squareControlFactory;

        public bool IsReverse { get; private set; } = false;

        public BoardControl(ISquareControlFactory squareControlFactory)
        {
            InitializeComponent();

            this.squareControlFactory = squareControlFactory;
            this.Controls.AddRange(GetSquares().ToArray());
        }

        private IEnumerable<Control> GetSquares()
        {
            foreach (int id in Enumerable.Range(0, 64))
                yield return CreateSquareControl(id);
        }

        private Control CreateSquareControl(int id)
        {
            var control = squareControlFactory.Create(id);
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
