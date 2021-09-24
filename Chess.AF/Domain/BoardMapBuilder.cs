using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public partial class BoardMap
    {
        internal class BoardMapBuilder
        {
            #region Properties

            private BoardMap boardMap;
            private PiecesEnum piece = PiecesEnum.WhiteKing;

            private bool IsWhiteToMove { get { return (int)piece >= 7;  } }

            #endregion

            #region ctor

            public BoardMapBuilder()
            {
                boardMap = new BoardMap();
            }

            #endregion

            #region IBoardBuilder

            public BoardMapBuilder Clear()
            {
                boardMap.Clear();
                return this;
            }

            internal BoardMapBuilder WithBoard(IBoard board)
            {
                boardMap.Abstraction = board;
                return this;
            }

            public BoardMapBuilder WithPiece(PiecesEnum piece)
            {
                this.piece = piece;
                return this;
            }

            public BoardMapBuilder Off(SquareEnum square)
            {
                boardMap.Maps[(int)piece].SetBitOff((int)square);
                boardMap.Maps[boardMap.GetIndexAllPiecesFor(IsWhiteToMove)].SetBitOff((int)square);
                return this;
            }

            public BoardMapBuilder On(SquareEnum square)
            {
                boardMap.Maps[(int)piece].SetBit((int)square);
                boardMap.Maps[boardMap.GetIndexAllPiecesFor(IsWhiteToMove)].SetBit((int)square);
                return this;
            }

            public BoardMapBuilder Toggle(SquareEnum square)
                => boardMap.Maps[(int)piece].IsBitOn((int)square) ? Off(square) : On(square);

            #endregion

            #region Validation

            internal bool ValidKingOnKingSquare(bool isWhiteToMove, SquareEnum kingSquare)
                => isBitOn(PieceEnum.King.ToPieces(isWhiteToMove), kingSquare);

            internal bool ValidRookOnRookSquare(bool isWhiteToMove, SquareEnum rookSquare)
                => isBitOn(PieceEnum.Rook.ToPieces(isWhiteToMove), rookSquare);

            private bool isBitOn(PiecesEnum piece, SquareEnum square)
                => boardMap.Maps[(int)piece].IsBitOn((int)square);

            #endregion

            #region Build

            public IBoardMap Build()
                => boardMap;

            #endregion
        }
    }

}
