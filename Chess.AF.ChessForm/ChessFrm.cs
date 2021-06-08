using Chess.AF.ChessForm.Controllers;
using Chess.AF.ChessForm.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Chess.AF.ChessForm.ChessConstants;
using static Chess.AF.ChessForm.ImageHelper;
using static AF.Functional.F;

namespace Chess.AF.ChessForm
{
    public partial class ChessFrm : Form, IBoardView
    {
        private BoardControl boardControl;
        private IBoardController boardController;
        public ChessFrm()
        {
            InitializeComponent();

            this.loadFen.Hide();
            this.Size = new Size(this.Size.Width, FormHeight);

            this.boardController = new BoardController();
            this.boardController.Register(this);

            this.boardControl = new BoardControl(this.boardController);
            this.boardControl.BackColor = Color.SaddleBrown;
            this.boardControl.Size = new Size(BoardWidth, BoardWidth);
            this.boardControl.Margin = new Padding(0);
            this.boardControl.Location = new Point(0, 33);
            this.boardControl.BorderStyle = BorderStyle.FixedSingle;

            this.BackColor = Color.Wheat;

            lblResult.Text = string.Empty;
            lblResult.Font = new Font(FontFamily.Families[0], 16, FontStyle.Regular);
            lblResult.Location = new Point(570, 43);
            lblResult.Size = new Size(200, 23);

            //MessageBox.Show($"X: {this.boardControl.Location.X} Y: {this.boardControl.Location.Y} W: {this.boardControl.Size.Width} H: {this.boardControl.Size.Height}");

            this.Controls.Add(this.boardControl);
            this.Controls.Add(lblResult);

            this.btnLoadFen.Image = Fen();
            this.btnLoadPgn.Image = Pgn();
            this.btnFirstMove.Image = FirstMove(23, 22);
            this.btnPreviousMove.Image = PreviousMove(23, 22);
            this.btnNextMove.Image = NextMove(23, 22);
            this.btnLastMove.Image = LastMove(23, 22);
            this.btnReverseBoard.Image = TurnBoard();
            this.btnResign.Image = ResignFlag();
            this.btnDraw.Image = Draw50();
            UpdateView();
        }

        Label lblResult = new Label();
        LoadFen loadFen = new LoadFen();

        private void BtnLoadFen_Click(object sender, EventArgs e)
        {
            var result = this.loadFen.ShowDialog();
            if (DialogResult.OK.Equals(result))
                boardController.LoadFen(this.loadFen.Fen);
            if (DialogResult.Yes.Equals(result))
                boardController.LoadFen();
        }

        private void BtnLoadPgn_Click(object sender, EventArgs e)
        {
            this.openFileDialog1 = new OpenFileDialog();
            this.openFileDialog1.Filter = "pgn files (*.pgn)|*.pgn|All files (*.*)|*.*";
            this.openFileDialog1.FilterIndex = 0;
            this.openFileDialog1.Title = "Browse Portable Game Notation files";
            var result = this.openFileDialog1.ShowDialog();
            if (DialogResult.OK.Equals(result))
            {
                string pgnString = string.Empty;
                Using(new StreamReader(this.openFileDialog1.FileName), reader => pgnString = reader.ReadToEnd());
                this.boardController.SetFromPgn(Pgn.Import(pgnString));
            }
        }

        private void BtnFirstMove_Click(object sender, EventArgs e)
            => this.boardController.GotoFirstMove();

        private void BtnPreviousMove_Click(object sender, EventArgs e)
            => this.boardController.GotoPreviousMove();

        private void BtnNextMove_Click(object sender, EventArgs e)
            => this.boardController.GotoNextMove();

        private void BtnLastMove_Click(object sender, EventArgs e)
            => this.boardController.GotoLastMove();

        private void btnReverseBoard_Click(object sender, EventArgs e)
            => this.boardControl.ReverseBoardView();

        private void btnResign_Click(object sender, EventArgs e)
            => boardController.Resign();

        private void btnDraw_Click(object sender, EventArgs e)
            => boardController.Draw();

        public void UpdateView()
            => lblResult.Text = whoToMove();

        private string whoToMove()
        {
            if (!tryToShowFinalInfo(out string finalResult))
                if (boardController.IsWhiteToMove)
                    return $"White to move{checkInfo()}";
                else
                    return $"Black to move{checkInfo()}";
            else
                return finalResult;
        }

        private bool tryToShowFinalInfo(out string finalResult)
        {
            finalResult = string.Empty;
            bool final = false;
            var result = boardController.Result;
            if (!GameResult.Ongoing.Equals(result) && !GameResult.Invalid.Equals(result))
            {
                final = true;
                if (GameResult.Draw.Equals(result))
                    finalResult = $"Draw{showStalemate()}";
                if (GameResult.WhiteWins.Equals(result))
                    finalResult = $"White Wins{showMate()}";
                if (GameResult.BlackWins.Equals(result))
                    finalResult = $"Black Wins{showMate()}";
            }
            return final;
        }

        private string showMate()
            => boardController.IsMate ? " Checkmate" : string.Empty;

        private string showStalemate()
            => boardController.IsStaleMate ? " Stalemate" : string.Empty;

        private string checkInfo()
            => boardController.IsInCheck ? " Check" : string.Empty;
    }
}
