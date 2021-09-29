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
            this.boardControl.Location = new Point(0, 33);
            this.boardControl.BorderStyle = BorderStyle.FixedSingle;

            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.Add(this.boardControl);
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
