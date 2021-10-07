using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ImportExport
{
    public interface IGameBuilder
    {
        IGame Game { get; }
        GameBuilder WithDefault();
        GameBuilder WithFen(string fen);
        GameBuilder With(PieceEnum piece);
        GameBuilder WithMoveFromFile(int file);
        GameBuilder WithMoveFromRow(int row);
        GameBuilder WithPromote(PieceEnum piece);
        GameBuilder With(RokadeEnum rokade);
        GameBuilder WithMoveTo(SquareEnum square);
        GameBuilder With(GameResult result);
    }
}
