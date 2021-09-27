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
                boardMap.Maps[(int)piece] = boardMap.Maps[(int)piece].SetBitOff((int)square);
                int allPieces = boardMap.GetIndexAllPiecesFor(IsWhiteToMove);
                boardMap.Maps[allPieces] = boardMap.Maps[allPieces].SetBitOff((int)square);
                return this;
            }

            public BoardMapBuilder On(SquareEnum square)
            {
                int allPieces = boardMap.GetIndexAllPiecesFor(IsWhiteToMove);
                clearKingMaps(allPieces);
                if (!isPawnOnRow1Or8(square))
                {
                    boardMap.Maps[(int)piece] = boardMap.Maps[(int)piece].SetBit((int)square);
                    boardMap.Maps[allPieces] = boardMap.Maps[allPieces].SetBit((int)square);
                }
                return this;
            }

            public BoardMapBuilder Toggle(SquareEnum square)
                => boardMap.Maps[(int)piece].IsBitOn((int)square) ? Off(square) : On(square);

            #endregion

            #region Private

            private void clearKingMaps(int indexAllPieces)
            {
                if (isKing())
                {
                    boardMap.Maps[(int)piece] = 0ul;
                    boardMap.Maps[indexAllPieces] = 0ul;
                }
            }

            private bool isPawnOnRow1Or8(SquareEnum square)
                => isPawn() && (square.Row() == 0 || square.Row() == 7);

            private bool isKing()
                => PiecesEnum.BlackKing.Is(piece) || PiecesEnum.WhiteKing.Is(piece);

            private bool isPawn()
                => PiecesEnum.BlackPawn.Is(piece) || PiecesEnum.WhitePawn.Is(piece);

            #endregion

            #region Build

            public IBoardMap Build()
                => boardMap;

            #endregion
        }
    }

}
