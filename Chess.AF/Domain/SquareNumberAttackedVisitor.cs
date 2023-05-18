using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public interface ISquareNumberAttackedVisitor : IBoardMapVisitor
    {
        IEnumerable<AttackSquare> Iterator { get; }
        FilterFlags Flags { get; set; }
    }

    public partial class BoardMap
    {
        public static ISquareNumberAttackedVisitor GetSquareNumberAttackedVisitor()
            => new SquareNumberAttackedVisitor();
        public static ISquareNumberAttackedVisitor GetSquareNumberAttackedVisitor(FilterFlags flags = FilterFlags.Both)
            => new SquareNumberAttackedVisitor(flags);

        private class SquareNumberAttackedVisitor : BoardMapVisitor, ISquareNumberAttackedVisitor
        {
            public FilterFlags Flags { get; set; }
            public IEnumerable<AttackSquare> Iterator { get; private set; }

            public SquareNumberAttackedVisitor() : this(FilterFlags.Both) { }
            public SquareNumberAttackedVisitor(FilterFlags flags = FilterFlags.Both)
            {
                this.Flags = flags;
            }

            public override void Visit(BoardMap map)
                => Iterator = Enum<SquareEnum>
                .AsEnumerable()
                .Select(s => new AttackSquare() { Square = s, Count = 0 })
                .Select(a => Count(map, a));

            private AttackSquare Count(BoardMap map, AttackSquare attackSquare)
            {
                attackSquare.Count = map.CountAttacks(attackSquare.Square, ShouldFilterWhite(), ShouldFilterBlack());
                return attackSquare;
            }

            private bool ShouldFilterWhite()
                => (Flags & FilterFlags.White) == FilterFlags.White;

            private bool ShouldFilterBlack()
                => (Flags & FilterFlags.Black) == FilterFlags.Black;
        }
    }
}
