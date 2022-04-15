using Chess.AF.ChessForm.Helpers;
using Chess.AF.Controllers;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.AF.ChessForm.Controls
{
    internal static class VisitorFactory
    {
        internal static IEnumerable<Control> CreateCheckboxControls(IGameController gameController)
        {
            return LoosePiecesControl(gameController)
                .Concat(SquareNotAttackedControl(gameController));
        }

        private static IEnumerable<Control> LoosePiecesControl(IGameController gameController)
        {
            CheckBoxesControl chkControl = new CheckBoxesControl();
            chkControl.SetLabelText("Loose pieces");
            chkControl.AddCheckBox(ImageHelper.BlackWhiteQueenSmall(), (sender, e) => gameController.UseLoosePiecesIterator(IsCheckBoxChecked(sender)));
            chkControl.AddCheckBox(ImageHelper.WhiteQueenSmall(), (sender, e) => gameController.UseLoosePiecesIterator(IsCheckBoxChecked(sender), FilterFlags.White));
            chkControl.AddCheckBox(ImageHelper.BlackQueenSmall(), (sender, e) => gameController.UseLoosePiecesIterator(IsCheckBoxChecked(sender), FilterFlags.Black));
            chkControl.Location = new Point(570, 166);

            yield return chkControl;
        }

        private static IEnumerable<Control> SquareNotAttackedControl(IGameController gameController)
        {
            CheckBoxesControl chkControl = new CheckBoxesControl();
            chkControl.SetLabelText("Not attacked s");
            chkControl.AddCheckBox(ImageHelper.BlackWhiteQueenSmall(), (sender, e) => gameController.UseNotAttackedIterator(IsCheckBoxChecked(sender)));
            chkControl.AddCheckBox(ImageHelper.WhiteQueenSmall(), (sender, e) => gameController.UseNotAttackedIterator(IsCheckBoxChecked(sender), FilterFlags.White));
            chkControl.AddCheckBox(ImageHelper.BlackQueenSmall(), (sender, e) => gameController.UseNotAttackedIterator(IsCheckBoxChecked(sender), FilterFlags.Black));
            chkControl.Location = new Point(570, 196);

            yield return chkControl;
        }

        private static bool IsCheckBoxChecked(object sender)
        {
            CheckBox chk = sender as CheckBox;
            return chk.Checked;
        }
    }
}
