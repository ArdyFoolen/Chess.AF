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
            return LoosePiecesControl(gameController);
        }

        private static IEnumerable<Control> LoosePiecesControl(IGameController gameController)
        {
            CheckBoxesControl chkLooseControl = new CheckBoxesControl();
            chkLooseControl.SetLabelText("Loose pieces");
            chkLooseControl.AddCheckBox(ImageHelper.BlackWhiteQueenSmall(), (sender, e) => gameController.UseLoosePiecesIterator(IsCheckBoxChecked(sender)));
            chkLooseControl.AddCheckBox(ImageHelper.WhiteQueenSmall(), (sender, e) => gameController.UseLoosePiecesIterator(IsCheckBoxChecked(sender), FilterFlags.White));
            chkLooseControl.AddCheckBox(ImageHelper.BlackQueenSmall(), (sender, e) => gameController.UseLoosePiecesIterator(IsCheckBoxChecked(sender), FilterFlags.Black));
            chkLooseControl.Location = new Point(570, 166);

            yield return chkLooseControl;
        }

        private static bool IsCheckBoxChecked(object sender)
        {
            CheckBox chk = sender as CheckBox;
            return chk.Checked;
        }
    }
}
