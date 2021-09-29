using Chess.AF.ChessForm.Factories;
using Chess.AF.Controllers;
using Chess.AF.Domain;
using Chess.AF.Enums;
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
    public partial class SetupPosition : Form
    {
        private BoardControl boardControl;
        private ISetupPositionController setupPositionController;

        private CheckBoxesControl chkBlackControl;
        private CheckBoxesControl chkWhiteControl;

        public IBoard Board { get => setupPositionController.Board; }

        public SetupPosition(ISetupPositionController setupPositionController)
        {
            InitializeComponent();

            this.setupPositionController = setupPositionController;

            var squareControlFactory = new BaseSquareControlFactory(setupPositionController);
            this.boardControl = new BoardControl(squareControlFactory);
            this.boardControl.BackColor = Color.SaddleBrown;
            this.boardControl.Size = new Size(BoardWidth, BoardWidth);
            this.boardControl.Margin = new Padding(0);
            this.boardControl.Location = new Point(0, SquareWidth);
            this.boardControl.BorderStyle = BorderStyle.FixedSingle;

            this.chkBlackControl = new CheckBoxesControl();
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackKing(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.BlackKing));
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackQueen(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.BlackQueen));
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackRook(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.BlackRook));
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackBishop(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.BlackBishop));
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackKnight(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.BlackKnight));
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackPawn(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.BlackPawn));
            this.chkBlackControl.Location = new Point(0, 0);
            this.chkBlackControl.CheckBoxSize = new Size(SquareWidth, SquareWidth);

            this.chkWhiteControl = new CheckBoxesControl();
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteKing(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.WhiteKing));
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteQueen(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.WhiteQueen));
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteRook(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.WhiteRook));
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteBishop(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.WhiteBishop));
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteKnight(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.WhiteKnight));
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhitePawn(), (sender, e) => this.setupPositionController.WithPiece(PiecesEnum.WhitePawn));
            this.chkWhiteControl.Location = new Point(0, BoardWidth + SquareWidth);
            this.chkWhiteControl.CheckBoxSize = new Size(SquareWidth, SquareWidth);

            this.chkBlackControl.LinkTo(this.chkWhiteControl);
            this.chkWhiteControl.LinkTo(this.chkBlackControl);
            this.chkWhiteControl.SetCheckedFor(0);

            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.Add(this.boardControl);
            this.Controls.Add(this.chkBlackControl);
            this.Controls.Add(this.chkWhiteControl);
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            if (setupPositionController.TryBuild())
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
