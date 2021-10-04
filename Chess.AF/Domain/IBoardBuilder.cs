using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;

namespace Chess.AF.Domain
{
    public interface IBoardBuilder
    {
        PiecesEnum CurrentPiece { get; }
        Option<SquareEnum> CurrentEpSquare { get; }
        bool IsWhiteToMove { get; }
        RokadeEnum WhiteRokade { get; }
        RokadeEnum BlackRokade { get; }
        Option<PieceOnSquare<PiecesEnum>> GetPieceOn(SquareEnum square);

        IBoardBuilder With(bool whiteToMove);
        IBoardBuilder WithWhite(RokadeEnum rokade);
        IBoardBuilder WithBlack(RokadeEnum rokade);
        IBoardBuilder WithEnPassant(SquareEnum epSquare);
        IBoardBuilder WithoutEnPassant();

        IBoardBuilder Clear();
        IBoardBuilder With(PiecesEnum piece);
        IBoardBuilder Off(SquareEnum square);
        IBoardBuilder On(SquareEnum square);
        IBoardBuilder Toggle(SquareEnum square);
    }
}
