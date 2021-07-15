using Chess.AF.Domain;
using Chess.AF.Enums;
using Chess.AF.ImportExport;
using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.UnitTests
{
    public class LoosePieceVisitorTests
    {

        [TestCaseSource(typeof(TestSourceHelper), "LoosePieceTestCases")]
        public void Iterator_IsValid((string FenString, SquareEnum[] Squares) expected)
        {
            Fen.Of(expected.FenString).CreateBoard()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { AssertIterator(p, expected.Squares); return true; });
        }

        [TestCaseSource(typeof(TestSourceHelper), "LoosePieceWithFilterTestCases")]
        public void Iterator_IsValid((string FenString, FilterFlags Flags, SquareEnum[] Squares) expected)
        {
            Fen.Of(expected.FenString).CreateBoard()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { AssertIterator(p, expected.Flags, expected.Squares); return true; });
        }

        private void AssertIterator(IBoard board, SquareEnum[] squares)
            => AssertIterator(board, FilterFlags.Both, squares);

        private void AssertIterator(IBoard board, FilterFlags flags, SquareEnum[] squares)
        {
            var visitor = BoardMap.GetLoosePiecesVisitor(flags);
            board.Accept(visitor);
            Assert.IsTrue(visitor.Iterator.Count() == squares.Count() && visitor.Iterator.Intersect(squares).Count() == visitor.Iterator.Count());
        }

    }
}
