using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF
{
    public class Move
    {
        #region Properties

        public PieceEnum Piece { get; private set; }
        public SquareEnum From { get; private set; }
        public SquareEnum To { get; private set; }
        public PieceEnum Promote { get; private set; }
        public RokadeEnum Rokade { get; private set; }

        #endregion

        #region static methods

        public static Option<Move> Of(PieceEnum piece, SquareEnum? from = null, SquareEnum? to = null, PieceEnum? promote = null, RokadeEnum rokade = RokadeEnum.None)
            => ValidateMove(piece, from, to, promote, rokade);

        private static Option<Move> ValidateMove(PieceEnum piece, SquareEnum? from, SquareEnum? to, PieceEnum? promote, RokadeEnum rokade)
            => RokadeEnum.None.Equals(rokade) ? ValidateMove(piece, from, to, promote) : ValidateRokade(piece, from, to, promote, rokade);

        private static Option<Move> ValidateMove(PieceEnum piece, SquareEnum? from, SquareEnum? to, PieceEnum? promote)
            => !from.HasValue || !to.HasValue ? None : Some(new Move(piece, from, to, promote));

        private static Option<Move> ValidateRokade(PieceEnum piece, SquareEnum? from, SquareEnum? to, PieceEnum? promote, RokadeEnum rokade)
            => from.HasValue || to.HasValue || promote.HasValue ? None : Some(new Move(piece, rokade: rokade));

        #endregion


        #region ctors

        private Move(PieceEnum piece, SquareEnum? from = null, SquareEnum? to = null, PieceEnum? promote = null, RokadeEnum rokade = RokadeEnum.None)
        {
            Piece = piece;
            if (from.HasValue)
            {
                From = from.Value;
                Promote = Piece;
            }
            if (to.HasValue)
                To = to.Value;
            if (promote.HasValue)
                Promote = promote.Value;
            Rokade = rokade;
        }

        #endregion
    }
}
