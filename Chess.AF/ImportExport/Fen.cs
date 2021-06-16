using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static AF.Functional.F;

namespace Chess.AF.ImportExport
{
    public class Fen
    {
        public string FenString { get; }
        private Fen(string fenString) { this.FenString = fenString; }

        public static Option<Fen> Of(string fenString)
            => IsValid(fenString) ? Some(new Fen(fenString)) : None;

        public static bool IsValid(string fenString)
            => FenRegex.IsValid(fenString);

        public IEnumerable<char> ForEachPosition()
            => this.FenString.TakeWhile(c => !' '.Equals(c));
        public bool IsWhiteToMove { get { return "w".Equals(FenString.Split(' ')[1]); } }

        public RokadeEnum WhiteRokade { get { return GetRokadeFromString('K', 'Q'); } }

        public RokadeEnum BlackRokade { get { return GetRokadeFromString('k', 'q'); } }

        public Option<SquareEnum> EnPassant {
            get
            {
                if (Enum.TryParse<SquareEnum>(FenString.Split(' ')[3], out SquareEnum ep))
                    return Some(ep);
                return None;
            }
        }

        private RokadeEnum GetRokadeFromString(char king, char queen)
        {
            RokadeEnum r = 0;
            string rString = FenString.Split(' ')[2];
            r |= rString.Contains(king) ? RokadeEnum.KingSide : RokadeEnum.None;
            r |= rString.Contains(queen) ? RokadeEnum.QueenSide : RokadeEnum.None;
            return r;

        }

        public int PlyCount
        {
            get
            {
                if (int.TryParse(FenString.Split(' ')[4], out int plyCount))
                    return plyCount;
                return 0;
            }
        }

        public int MoveNumber
        {
            get
            {
                if (int.TryParse(FenString.Split(' ')[5], out int moveNumber))
                    return moveNumber;
                return 0;
            }
        }
    }
}
