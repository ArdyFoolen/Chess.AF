﻿using System;
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
        private IEnumerable<SquareControl> squares;
        public BoardControl()
        {
            InitializeComponent();

            this.squares = GetSquares();

            this.Controls.AddRange(this.squares.ToArray());
        }

        private IEnumerable<SquareControl> GetSquares()
        {
            foreach (int id in Enumerable.Range(0, 64))
                yield return CreateSquareControl(id);
        }

        private SquareControl CreateSquareControl(int id)
        {
            var control = new SquareControl(id);
            control.BackColor = GetSquareBackColor(id);
            control.Location = new Point(id % 8 * SquareWidth, id / 8 * SquareWidth);
            control.Margin = new Padding(0);
            control.Size = new Size(SquareWidth, SquareWidth);
            return control;
        }

        private static Color GetSquareBackColor(int id)
        {
            return (id % 2 == 0 && (id / 8) % 2 == 0 ||
                    id % 2 == 1 && (id / 8) % 2 == 1)
                   ? Color.White : Color.Brown;
        }
    }
}
