using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF
{
    public partial class Position
    {
        public class MapIterator
        {
            private Position Position { get; }
            private MapIterator(Position position)
            {
                this.Position = position;
            }

            public static Option<MapIterator> Of(Option<Position> position)
                => position.Bind(WhenValid);

            private static Option<MapIterator> WhenValid(Position position)
                => Some(new MapIterator(position));

            public IEnumerable<ulong> Maps()
            {
                if (this.Position.IsWhiteToMove)
                    return MapsForWhitePieces();
                return MapsForBlackPieces();
            }

            private IEnumerable<ulong> MapsForBlackPieces()
                => IterateMaps(typeof(BlackPiecesEnum));

            private IEnumerable<ulong> MapsForWhitePieces()
                => IterateMaps(typeof(WhitePiecesEnum));

            private IEnumerable<ulong> IterateMaps(Type enumType)
            {
                Array values = Enum.GetValues(enumType);
                foreach (int value in values)
                    yield return Position.Maps[value];
            }
        }
    }
}
