using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using static Chess.AF.Domain.Board;

namespace Chess.AF.Domain
{
    public partial class BoardMap
    {
        internal class BoardValidator : IBoardValidator
        {
            #region Properties

            private Board board;
            private BoardMap boardMap;

            #endregion

            #region IBoardValidator

            public void SetBoard(IBoard board)
                => this.board = board as Board;

            public void SetBoardMap(IBoardMap boardMap)
                => this.boardMap = boardMap as BoardMap;

            public Validation<Board> Validate()
                => ValidateAll()(board);

            private Validator<Board> ValidateAll()
                => HarvestErrorsTr(ValidateKings(boardMap), ValidateRokade(board, boardMap), ValidateEpSquare(board, boardMap), ValidatePawns(boardMap));

            #endregion

            #region ValidateKings

            private static Validator<Board> ValidateKings(BoardMap boardMap)
               => HarvestErrorsTr(ShouldBe1King(boardMap, PiecesEnum.BlackKing), ShouldBe1King(boardMap, PiecesEnum.WhiteKing));

            private static Validator<Board> ShouldBe1King(BoardMap boardMap, PiecesEnum piece)
              => b
              => GetIteratorFor(boardMap, piece).ToArray().Length == 1 ? Valid(b) : Error($"{piece} should be one King");

            #endregion

            #region ValidateRokade

            private static Validator<Board> ValidateRokade(Board board, BoardMap boardMap)
               => HarvestErrorsTr(
                   ShouldBeKingOnKingSquareForRokade(boardMap, PiecesEnum.BlackKing, SquareEnum.e8, board.BlackRokade),
                   ShouldBeKingOnKingSquareForRokade(boardMap, PiecesEnum.WhiteKing, SquareEnum.e1, board.WhiteRokade),
                   ShouldBeRookOnRookSquareForRokade(boardMap, PiecesEnum.BlackRook, SquareEnum.h8, board.BlackRokade, r => r.IsKingsideRokade()),
                   ShouldBeRookOnRookSquareForRokade(boardMap, PiecesEnum.WhiteRook, SquareEnum.h1, board.WhiteRokade, r => r.IsKingsideRokade()),
                   ShouldBeRookOnRookSquareForRokade(boardMap, PiecesEnum.BlackRook, SquareEnum.a8, board.BlackRokade, r => r.IsQueensideRokade()),
                   ShouldBeRookOnRookSquareForRokade(boardMap, PiecesEnum.WhiteRook, SquareEnum.a1, board.WhiteRokade, r => r.IsQueensideRokade()));

            private static Validator<Board> ShouldBeKingOnKingSquareForRokade(BoardMap boardMap, PiecesEnum king, SquareEnum kingSquare, RokadeEnum rokade)
              => b
              => !IsRokadePossible(rokade) || kingSquare.Equals(GetSquare(boardMap, king)) ? Valid(b) : Error($"{king} should be on square {kingSquare} for rokade {rokade}");

            private static Validator<Board> ShouldBeRookOnRookSquareForRokade(BoardMap boardMap, PiecesEnum rook, SquareEnum rookSquare, RokadeEnum rokade, Func<RokadeEnum, bool> IsSideRokade)
              => b
              => !IsRokadePossible(rokade) || !IsSideRokade(rokade) || IsPieceOnSquare(boardMap, rook, rookSquare)
              ? Valid(b)
              : Error($"{rook} should be on square {rookSquare} for rokade {rokade}");

            #endregion

            #region ValidateEpSquare

            private static Validator<Board> ValidateEpSquare(Board board, BoardMap boardMap)
                => HarvestErrorsTr(
                    ShouldBeEpSquareOnRow(board.IsWhiteToMove, board.EpSquare),
                    ShouldBePawnForEpSquareOnNextRow(boardMap, board.IsWhiteToMove, board.EpSquare),
                    ShouldBeEmptyForEpSquareOnPreviousRow(boardMap, board.IsWhiteToMove, board.EpSquare));

            private static Validator<Board> ShouldBeEpSquareOnRow(bool isWhiteToMove, Option<SquareEnum> epSquare)
              => b
              => epSquare.Match(
                  None: () => Valid(b),
                  Some: s => isWhiteToMove && s.Row() == 2 || !isWhiteToMove && s.Row() == 5 ? Valid(b) : Error(getEpRowError(isWhiteToMove, s))
                  );

            private static Validator<Board> ShouldBePawnForEpSquareOnNextRow(BoardMap boardMap, bool isWhiteToMove, Option<SquareEnum> epSquare)
              => b
              => epSquare.Match(
                  None: () => Valid(b),
                  Some: s => isWhiteToMove && IsPieceOnSquare(boardMap, PiecesEnum.BlackPawn, WhiteEpPawnSquare(s)) ||
                            !isWhiteToMove && IsPieceOnSquare(boardMap, PiecesEnum.WhitePawn, BlackEpPawnSquare(s))
                                ? Valid(b) : Error(getEpPawnNotPresentError(isWhiteToMove, s))
                  );

            private static Validator<Board> ShouldBeEmptyForEpSquareOnPreviousRow(BoardMap boardMap, bool isWhiteToMove, Option<SquareEnum> epSquare)
              => b
              => epSquare.Match(
                  None: () => Valid(b),
                  Some: s => isWhiteToMove && !IsPieceOnSquare(boardMap, WhiteEpEmptySquare(s)) ||
                            !isWhiteToMove && !IsPieceOnSquare(boardMap, BlackEpEmptySquare(s))
                                ? Valid(b) : Error(getEpNotEmptyError(isWhiteToMove, s))
                  );

            private static string getEpNotEmptyError(bool isWhiteToMove, SquareEnum epSquare)
                => isWhiteToMove
                ? $"En-Passant square {epSquare}, White to move square {WhiteEpEmptySquare(epSquare)} not empty"
                : $"En-Passant square {epSquare}, Black to move square {BlackEpEmptySquare(epSquare)} not empty";

            private static string getEpPawnNotPresentError(bool isWhiteToMove, SquareEnum epSquare)
                => isWhiteToMove
                ? $"En-Passant square {epSquare}, White to move no pawn present on square {WhiteEpPawnSquare(epSquare)}"
                : $"En-Passant square {epSquare}, Black to move no pawn present on square {BlackEpPawnSquare(epSquare)}";

            private static string getEpRowError(bool isWhiteToMove, SquareEnum epSquare)
                => isWhiteToMove
                ? $"En-Passant square {epSquare}, White to move En-Passant square should be on row 6"
                : $"En-Passant square {epSquare}, Black to move En-Passant square should be on row 3";

            #endregion

            #region ValidatePawn

            private static Validator<Board> ValidatePawns(BoardMap boardMap)
                => HarvestErrorsTr(ShouldBeNoPawnOnRow1Or8(boardMap));

            private static Validator<Board> ShouldBeNoPawnOnRow1Or8(BoardMap boardMap)
              => b
              => !IsPawnOnRow1Or8(boardMap, PiecesEnum.BlackPawn) && !IsPawnOnRow1Or8(boardMap, PiecesEnum.WhitePawn)
              ? Valid(b)
              : Error($"No pawn should be on row 1 or 8");

            #endregion

            #region Private Methods

            private static SquareEnum GetSquare(BoardMap boardMap, PiecesEnum piece)
                => GetIteratorFor(boardMap, piece).First().Square;

            private static bool IsPieceOnSquare(BoardMap boardMap, PiecesEnum piece, SquareEnum square)
                => GetIteratorFor(boardMap, piece).Any(a => square.Equals(a.Square));

            private static bool IsPieceOnSquare(BoardMap boardMap, SquareEnum square)
                => boardMap.GetIteratorForAll<PiecesEnum>().Any(a => square.Equals(a.Square));

            private static bool IsPawnOnRow1Or8(BoardMap board, PiecesEnum piece)
                => GetIteratorFor(board, piece).Any(ps => ps.Square.Row() == 0 || ps.Square.Row() == 7);

            private static PiecesIterator<PiecesEnum> GetIteratorFor(BoardMap boardMap, PiecesEnum piece)
            {
                var map = boardMap.Maps[(int)piece];
                var pieceMap = new PieceMap<PiecesEnum>(piece, map);
                return new PiecesIterator<PiecesEnum>(pieceMap);
            }

            private static bool IsRokadePossible(RokadeEnum rokade)
                => !RokadeEnum.None.Equals(rokade);

            private static SquareEnum WhiteEpPawnSquare(SquareEnum epSquare)
                => (SquareEnum)((int)epSquare + 8);

            private static SquareEnum BlackEpPawnSquare(SquareEnum epSquare)
                => (SquareEnum)((int)epSquare - 8);

            private static SquareEnum WhiteEpEmptySquare(SquareEnum epSquare)
                => (SquareEnum)((int)epSquare - 8);

            private static SquareEnum BlackEpEmptySquare(SquareEnum epSquare)
                => (SquareEnum)((int)epSquare + 8);

            #endregion
        }
    }
}
