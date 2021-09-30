using Chess.AF.ChessForm.Factories;
using Chess.AF.ChessForm.Helpers;
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
            CheckBoxControlHelper.CreateBlackPieces(chkBlackControl, setupPositionController);
            this.chkBlackControl.Location = new Point(0, 0);
            this.chkBlackControl.CheckBoxSize = new Size(SquareWidth, SquareWidth);
            this.chkBlackControl.RadioStyle = true;

            this.chkWhiteControl = new CheckBoxesControl();
            CheckBoxControlHelper.CreateWhitePieces(chkWhiteControl, setupPositionController);
            this.chkWhiteControl.Location = new Point(0, BoardWidth + SquareWidth);
            this.chkWhiteControl.CheckBoxSize = new Size(SquareWidth, SquareWidth);
            this.chkWhiteControl.RadioStyle = true;

            this.chkBlackControl.LinkTo(this.chkWhiteControl);
            this.chkWhiteControl.LinkTo(this.chkBlackControl);

            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.Add(this.boardControl);
            this.Controls.Add(this.chkBlackControl);
            this.Controls.Add(this.chkWhiteControl);

            rdWhiteToMove.Checked = setupPositionController.IsWhiteToMove;
            rdBlackToMove.Checked = !setupPositionController.IsWhiteToMove;
            AddEpSquaresToCombo();
            AddRokadeToCombos();
        }

        private void AddRokadeToCombos()
        {
            AddRokadeToCombo(cmbWhiteRokade, setupPositionController.WhiteRokade);
            AddRokadeToCombo(cmbBlackRokade, setupPositionController.BlackRokade);
        }

        private void AddRokadeToCombo(ComboBox box, RokadeEnum selected = RokadeEnum.KingAndQueenSide)
        {
            box.DisplayMember = "Description";
            box.ValueMember = "Value";
            box.DataSource = Enum.GetValues(typeof(RokadeEnum))
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();

            box.SelectedValue = selected;
        }

        private void AddEpSquaresToCombo()
        {
            cmbEpSquare.Items.Add("None");
            cmbEpSquare.Items.AddRange(
                Enum.GetValues(typeof(SquareEnum))
                .Cast<SquareEnum>()
                .Where(w => w.Row() == 2 || w.Row() == 5)
                .Cast<object>()
                .ToArray());

            cmbEpSquare.SelectedItem = setupPositionController.CurrentEpSquare.Match<object>(
                None: () => "None",
                Some: s => s
                );
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

        private void btnClear_Click(object sender, EventArgs e)
            => setupPositionController.ClearBoard();

        private void cmbEpSquare_SelectedIndexChanged(object sender, EventArgs e)
        {
            SquareEnum? square = cmbEpSquare.SelectedItem as SquareEnum?;
            if (square.HasValue)
                setupPositionController.WithEpSquare(square.Value);
            else
                setupPositionController.WithoutEpSquare();
        }

        private void rdWhiteToMove_CheckedChanged(object sender, EventArgs e)
            => setupPositionController.WithWhiteToMove(rdWhiteToMove.Checked);

        private void cmbWhiteRokade_SelectedIndexChanged(object sender, EventArgs e)
            => setupPositionController.WithWhiteRokade((RokadeEnum)cmbWhiteRokade.SelectedValue);

        private void cmbBlackRokade_SelectedIndexChanged(object sender, EventArgs e)
            => setupPositionController.WithBlackRokade((RokadeEnum)cmbBlackRokade.SelectedValue);
    }
}
