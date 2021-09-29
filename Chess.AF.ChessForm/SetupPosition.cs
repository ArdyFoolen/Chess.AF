using Chess.AF.ChessForm.Factories;
using Chess.AF.Controllers;
using Chess.AF.Domain;
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
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackKing(), null);
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackQueen(), null);
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackRook(), null);
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackBishop(), null);
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackKnight(), null);
            this.chkBlackControl.AddCheckBox(ImageHelper.BlackPawn(), null);
            this.chkBlackControl.Location = new Point(0, 0);
            this.chkBlackControl.CheckBoxSize = new Size(SquareWidth, SquareWidth);

            this.chkWhiteControl = new CheckBoxesControl();
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteKing(), null);
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteQueen(), null);
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteRook(), null);
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteBishop(), null);
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhiteKnight(), null);
            this.chkWhiteControl.AddCheckBox(ImageHelper.WhitePawn(), null);
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
