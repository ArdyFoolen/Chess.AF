﻿using Chess.AF.Controllers;
using Chess.AF.Views;
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
using static Chess.AF.ChessForm.Helpers.ImageHelper;
using static AF.Functional.F;
using Chess.AF.Enums;
using Chess.AF.ChessForm.Helpers;
using Chess.AF.ImportExport;
using Chess.AF.Domain;
using Chess.AF.ChessForm.Factories;
using Chess.AF.ChessForm.Controls;

namespace Chess.AF.ChessForm.Forms
{
    public partial class ChessFrm : Form, IBoardView
    {
        private Label lblResult = new Label();
        private Label lblCount = new Label();
        private Label lblMoveNumber = new Label();
        private Label lblPlyCount = new Label();

        private LoadFen loadFen = new LoadFen();
        private PgnDialog pgnDialog;
        private SetupPosition setupPosition;
        private BoardControl boardControl;
        private IGameController gameController;
        private IPgnController pgnController;
        private PgnControl pgnControl;
        private ToolstripNumericUpDown numUpDown;

        public ChessFrm(IGameController gameController, IPgnController pgnController, ISetupPositionController setupPositionController)
        {
            InitializeComponent();

            this.loadFen.Hide();
            this.Size = new Size(this.Size.Width, FormHeight);

            this.gameController = gameController;
            this.gameController.Register(this);
            this.pgnController = pgnController;

            var squareControlFactory = new SquareControlFactory(this.gameController);
            this.boardControl = new BoardControl(squareControlFactory);
            this.boardControl.BackColor = Color.SaddleBrown;
            this.boardControl.Size = new Size(BoardWidth, BoardWidth);
            this.boardControl.Margin = new Padding(0);
            this.boardControl.Location = new Point(0, 33);
            this.boardControl.BorderStyle = BorderStyle.FixedSingle;

            this.numUpDown = new ToolstripNumericUpDown(pgnController);
            toolStrip1.Items.Insert(2, this.numUpDown);
            this.numUpDown.Name = "numUpDown";
            this.numUpDown.Size = new Size(23, 22);
            this.numUpDown.Visible = false;
            this.numUpDown.ToolTipText = "Change Loaded Pgn Game";

            this.pgnDialog = new PgnDialog(gameController, pgnController);
            this.setupPosition = new SetupPosition(setupPositionController);

            var checkBoxControls = VisitorFactory.CreateCheckboxControls(gameController);

            this.BackColor = Color.Wheat;

            lblResult.Text = string.Empty;
            lblResult.Font = new Font(FontFamily.Families[0], 13.5f, FontStyle.Regular);
            lblResult.Location = new Point(570, 43);
            lblResult.Size = new Size(200, 23);

            lblMoveNumber.Text = string.Empty;
            lblMoveNumber.Font = new Font(FontFamily.Families[0], 13.5f, FontStyle.Regular);
            lblMoveNumber.Location = new Point(570, 76);
            lblMoveNumber.Size = new Size(200, 23);

            lblPlyCount.Text = string.Empty;
            lblPlyCount.Font = new Font(FontFamily.Families[0], 13.5f, FontStyle.Regular);
            lblPlyCount.Location = new Point(570, 109);
            lblPlyCount.Size = new Size(200, 23);

            lblCount.Text = string.Empty;
            lblCount.Font = new Font(FontFamily.Families[0], 13.5f, FontStyle.Regular);
            lblCount.Location = new Point(570, 142);
            lblCount.Size = new Size(200, 23);

            pgnControl = new PgnControl(pgnController)
            {
                Location = new Point(570, 246),
                Size = new Size(200, 400)
            };

            //MessageBox.Show($"X: {this.boardControl.Location.X} Y: {this.boardControl.Location.Y} W: {this.boardControl.Size.Width} H: {this.boardControl.Size.Height}");

            this.Controls.Add(this.boardControl);
            this.Controls.AddRange(checkBoxControls.ToArray());
            this.Controls.Add(lblResult);
            this.Controls.Add(lblMoveNumber);
            this.Controls.Add(lblPlyCount);
            this.Controls.Add(lblCount);
            this.Controls.Add(this.pgnControl);

            this.btnLoadFen.Click += BtnLoadFen_Click;
            this.btnLoadPgn.Click += BtnLoadPgn_Click;
            this.btnSetupPosition.Click += btnSetupPosition_Click;
            this.numUpDown.ValueChanged += NumUpDown_ValueChanged;
            this.btnFirstMove.Click += BtnFirstMove_Click;
            this.btnPreviousMove.Click += BtnPreviousMove_Click;
            this.btnNextMove.Click += BtnNextMove_Click;
            this.btnLastMove.Click += BtnLastMove_Click;
            this.btnReverseBoard.Click += btnReverseBoard_Click;
            this.btnResign.Click += btnResign_Click;
            this.btnDraw.Click += btnDraw_Click;

            this.btnLoadFen.Image = Fen();
            this.btnLoadPgn.Image = Pgn();
            this.btnSetupPosition.Image = SetupPosition();
            this.btnFirstMove.Image = FirstMove(23, 22);
            this.btnPreviousMove.Image = PreviousMove(23, 22);
            this.btnNextMove.Image = NextMove(23, 22);
            this.btnLastMove.Image = LastMove(23, 22);
            this.btnReverseBoard.Image = TurnBoard();
            this.btnResign.Image = ResignFlag();
            this.btnDraw.Image = Draw50();
            UpdateView();
        }

        private void BtnLoadFen_Click(object sender, EventArgs e)
        {
            var result = this.loadFen.ShowDialog();
            if (DialogResult.OK.Equals(result))
                gameController.LoadFen(this.loadFen.Fen);
            if (DialogResult.Yes.Equals(result))
                gameController.LoadFen();
        }

        private void BtnLoadPgn_Click(object sender, EventArgs e)
            => this.pgnDialog.ShowDialog();

        private void btnSetupPosition_Click(object sender, EventArgs e)
        {
            var result = this.setupPosition.ShowDialog();
            if (DialogResult.OK.Equals(result))
                gameController.LoadFen(this.setupPosition.Board.ToFenString());
        }

        private void NumUpDown_ValueChanged(object sender, EventArgs e)
            => this.gameController.SetFromPgn(this.pgnController.PgnFileIndexChanged(numUpDown.Value - 1));

        private void BtnFirstMove_Click(object sender, EventArgs e)
            => this.gameController.GotoFirstMove();

        private void BtnPreviousMove_Click(object sender, EventArgs e)
            => this.gameController.GotoPreviousMove();

        private void BtnNextMove_Click(object sender, EventArgs e)
            => this.gameController.GotoNextMove();

        private void BtnLastMove_Click(object sender, EventArgs e)
            => this.gameController.GotoLastMove();

        private void btnReverseBoard_Click(object sender, EventArgs e)
            => this.boardControl.ReverseBoardView();

        private void btnResign_Click(object sender, EventArgs e)
            => gameController.Resign();

        private void btnDraw_Click(object sender, EventArgs e)
            => gameController.Draw();

        public void UpdateView()
        {
            lblResult.Text = whoToMove();
            lblMoveNumber.Text = $"Move: {gameController.MoveNumber.ToString("0;-#")}";
            lblPlyCount.Text = $"Ply count: {gameController.PlyCount.ToString("0;-#")}";
            lblCount.Text = $"Material: {gameController.MaterialCount.ToString("+0;-#")}";
        }

        private string whoToMove()
        {
            if (!tryToShowFinalInfo(out string finalResult))
                if (gameController.IsWhiteToMove)
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
            var result = gameController.Result;
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
            => gameController.IsMate ? " Checkmate" : string.Empty;

        private string showStalemate()
            => gameController.IsStaleMate ? " Stalemate" : string.Empty;

        private string checkInfo()
            => gameController.IsInCheck ? " Check" : string.Empty;
    }
}
