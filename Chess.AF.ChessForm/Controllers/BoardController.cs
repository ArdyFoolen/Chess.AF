using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.ChessForm.Controllers
{
    public class BoardController : IBoardController
    {
        private Game game;
        private Dictionary<int, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)> positionDict;

        public BoardController()
        {
            game = new Game();
            game.Load();
        }

        public Option<(PiecesEnum Piece, SquareEnum Square, bool IsSelected)> this[int index]
        {
            get
            {
                SetPositionDict();
                if (positionDict.ContainsKey(index))
                    return Some(positionDict[index]);
                return None;
            }
        }

        private void SetPositionDict()
            => game.Map(SetPositionDict);

        private Position SetPositionDict(Position position)
        {
            positionDict = position.ToDictionary();
            return position;
        }
    }
}
