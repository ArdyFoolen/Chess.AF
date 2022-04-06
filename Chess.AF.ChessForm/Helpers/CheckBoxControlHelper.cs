using Chess.AF.ChessForm.Controls;
using Chess.AF.Controllers;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Helpers
{
    internal class CheckBoxControlHelper
    {
        public static void CreateWhitePieces(CheckBoxesControl control, ISetupPositionController controller)
        {
            control.AddCheckBox(ImageHelper.WhiteKing(), (sender, e) => controller.WithPiece(PiecesEnum.WhiteKing), IsCurrentPiece(controller, PiecesEnum.WhiteKing));
            control.AddCheckBox(ImageHelper.WhiteQueen(), (sender, e) => controller.WithPiece(PiecesEnum.WhiteQueen), IsCurrentPiece(controller, PiecesEnum.WhiteQueen));
            control.AddCheckBox(ImageHelper.WhiteRook(), (sender, e) => controller.WithPiece(PiecesEnum.WhiteRook), IsCurrentPiece(controller, PiecesEnum.WhiteRook));
            control.AddCheckBox(ImageHelper.WhiteBishop(), (sender, e) => controller.WithPiece(PiecesEnum.WhiteBishop), IsCurrentPiece(controller, PiecesEnum.WhiteBishop));
            control.AddCheckBox(ImageHelper.WhiteKnight(), (sender, e) => controller.WithPiece(PiecesEnum.WhiteKnight), IsCurrentPiece(controller, PiecesEnum.WhiteKnight));
            control.AddCheckBox(ImageHelper.WhitePawn(), (sender, e) => controller.WithPiece(PiecesEnum.WhitePawn), IsCurrentPiece(controller, PiecesEnum.WhitePawn));
        }

        public static void CreateBlackPieces(CheckBoxesControl control, ISetupPositionController controller)
        {
            control.AddCheckBox(ImageHelper.BlackKing(), (sender, e) => controller.WithPiece(PiecesEnum.BlackKing), IsCurrentPiece(controller, PiecesEnum.BlackKing));
            control.AddCheckBox(ImageHelper.BlackQueen(), (sender, e) => controller.WithPiece(PiecesEnum.BlackQueen), IsCurrentPiece(controller, PiecesEnum.BlackQueen));
            control.AddCheckBox(ImageHelper.BlackRook(), (sender, e) => controller.WithPiece(PiecesEnum.BlackRook), IsCurrentPiece(controller, PiecesEnum.BlackRook));
            control.AddCheckBox(ImageHelper.BlackBishop(), (sender, e) => controller.WithPiece(PiecesEnum.BlackBishop), IsCurrentPiece(controller, PiecesEnum.BlackBishop));
            control.AddCheckBox(ImageHelper.BlackKnight(), (sender, e) => controller.WithPiece(PiecesEnum.BlackKnight), IsCurrentPiece(controller, PiecesEnum.BlackKnight));
            control.AddCheckBox(ImageHelper.BlackPawn(), (sender, e) => controller.WithPiece(PiecesEnum.BlackPawn), IsCurrentPiece(controller, PiecesEnum.BlackPawn));
        }

        private static bool IsCurrentPiece(ISetupPositionController controller, PiecesEnum piece)
            => piece.Is(controller.CurrentPiece);
    }
}
