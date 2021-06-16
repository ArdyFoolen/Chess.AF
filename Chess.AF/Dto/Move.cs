using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.Dto
{
    [DataContract]
    public class Move
    {
        #region Properties

        [DataMember]
        public PieceEnum Piece { get; private set; }
        [DataMember]
        public SquareEnum From { get; private set; }
        [DataMember]
        public SquareEnum To { get; private set; }
        [DataMember]
        public PieceEnum Promote { get; private set; }
        [DataMember]
        public RokadeEnum Rokade { get; private set; }

        #endregion

        #region static methods

        public static Option<Move> Of(RokadeEnum rokade)
            => ValidateRokade(rokade);

        public static Option<Move> Of(PieceEnum piece, SquareEnum? from = null, SquareEnum? to = null, PieceEnum? promote = null)
            => ValidateMove(piece, from, to, promote);

        private static Option<Move> ValidateRokade(RokadeEnum rokade)
            => RokadeEnum.None.Equals(rokade) ? None : Some(new Move(PieceEnum.King, rokade));

        private static Option<Move> ValidateMove(PieceEnum piece, SquareEnum? from, SquareEnum? to, PieceEnum? promote)
            => !from.HasValue || !to.HasValue ? None : Some(new Move(piece, from, to, promote));

        #endregion

        #region ctors

        private Move(PieceEnum piece, RokadeEnum rokade)
        {
            Piece = piece;
            Rokade = rokade;
        }

        private Move(PieceEnum piece, SquareEnum? from = null, SquareEnum? to = null, PieceEnum? promote = null)
        {
            Piece = piece;
            Rokade = RokadeEnum.None;
            if (from.HasValue)
            {
                From = from.Value;
                Promote = Piece;
            }
            if (to.HasValue)
                To = to.Value;
            if (promote.HasValue)
                Promote = promote.Value;
        }

        #endregion
    }
}
