using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public interface ISquareNotAttackedVisitor : IBoardMapVisitor
    {
        IEnumerable<SquareEnum> Iterator { get; }
        FilterFlags Flags { get; set; }
    }

    public partial class BoardMap
    {
        public static ISquareNotAttackedVisitor GetSquareNotAttackedVisitor()
            => new SquareNotAttackedVisitor();
        public static ISquareNotAttackedVisitor GetSquareNotAttackedVisitor(FilterFlags flags = FilterFlags.Both)
            => new SquareNotAttackedVisitor(flags);

        private class SquareNotAttackedVisitor : BoardMapVisitor, ISquareNotAttackedVisitor
        {
            public FilterFlags Flags { get; set; }
            public IEnumerable<SquareEnum> Iterator { get; private set; }

            public SquareNotAttackedVisitor() : this(FilterFlags.Both) { }
            public SquareNotAttackedVisitor(FilterFlags flags = FilterFlags.Both)
            {
                this.Flags = flags;
            }

            public override void Visit(BoardMap map)
                => Iterator = Enum<SquareEnum>.AsEnumerable().Where(s => Filter(map, s));

            private bool WhiteFilter(BoardMap map, SquareEnum square)
            {
                if (!ShouldFilterWhite())
                    return false;
                return (map.IsWhiteToMove && !map.IsSquareDefended(square)) ||
                    (!map.IsWhiteToMove && !map.IsSquareAttacked(square));
            }

            private bool BlackFilter(BoardMap map, SquareEnum square)
            {
                if (!ShouldFilterBlack())
                    return false;
                return (!map.IsWhiteToMove && !map.IsSquareDefended(square)) ||
                    (map.IsWhiteToMove && !map.IsSquareAttacked(square));
            }

            private bool Filter(BoardMap map, SquareEnum square)
            {
                if (ShouldFilterWhite() && ShouldFilterBlack())
                    return WhiteFilter(map, square) && BlackFilter(map, square);
                return WhiteFilter(map, square) || BlackFilter(map, square);
            }

            private bool ShouldFilterWhite()
                => (Flags & FilterFlags.White) == FilterFlags.White;

            private bool ShouldFilterBlack()
                => (Flags & FilterFlags.Black) == FilterFlags.Black;
        }
    }
}
