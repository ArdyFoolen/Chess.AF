using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using static Chess.AF.PositionBridge.PositionAbstraction;
using static Chess.AF.PositionBridge.PositionImpl.PositionFactory;

namespace Chess.AF.PositionBridge
{
    public class PositionMediator : IPositionMediatorAbstraction, IPositionMediatorImpl
    {
        #region static methods

        public static Option<IPositionAbstraction> Of(Option<Fen> fen)
            => fen.Bind(WhenValid);

        private static Option<IPositionAbstraction> WhenValid(Fen fen)
            => Some((new PositionMediator(fen)).PositionAbstraction);



        #endregion

        #region private properties

        public IPositionImpl PositionImpl { get; private set; }
        public IPositionAbstraction PositionAbstraction { get; private set; }


        #endregion

        #region ctors

        private PositionMediator(Fen fen)
        {
            PositionImpl = Create(fen, this);
            PositionAbstraction = new PositionAbstraction(this, fen.IsWhiteToMove, fen.WhiteRokade, fen.BlackRokade, fen.EnPassant,
                fen.PlyCount, fen.MoveNumber);
        }

        private PositionMediator(IPositionAbstraction positionAbstraction, IPositionImpl positionImpl)
        {
            PositionAbstraction = positionAbstraction;
            PositionImpl = positionImpl;
        }

        #endregion

        #region IPositionMediatorImpl

        public bool IsWhiteToMove { get { return PositionAbstraction.IsWhiteToMove; } }
        public RokadeEnum Rokade { get { return PositionAbstraction.Rokade; } }
        public Option<SquareEnum> EpSquare { get { return PositionAbstraction.EpSquare; } }

        #endregion

        #region IPositionMediatorAbstraction

        public bool IsTake { get { return PositionImpl.IsTake; } }
        public bool IsInCheck { get { return PositionImpl.IsInCheck; } }

        public SquareEnum KingSquare { get { return PositionImpl.KingSquare; } }
        public void SetBits(Move move)
            => PositionImpl.SetBits(move);

        public PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece)
            => PositionImpl.GetIteratorFor(piece);

        public PiecesIterator<T> GetIteratorForAll<T>()
            where T : Enum
            => PositionImpl.GetIteratorForAll<T>();

        public RokadeEnum PossibleRokade()
            => PositionImpl.PossibleRokade();

        public IPositionAbstraction CreateCopy()
        {
            var abstraction = PositionAbstraction.CreateCopy();
            var impl = PositionImpl.CreateCopy();

            var mediator = new PositionMediator(abstraction, impl);

            abstraction.SetMediator(mediator);
            impl.SetMediator(mediator);

            return mediator.PositionAbstraction;
        }

        #endregion
    }
}
