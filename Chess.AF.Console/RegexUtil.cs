﻿using AF.Functional;
using static AF.Functional.F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chess.AF.Enums;

namespace Chess.AF.Console
{
    internal static class RegexUtil
    {
        private static readonly string regexExpression = @"((?<KingRokade>^o-o$)|(?<QueenRokade>^o-o-o$)|^(?<Piece>[NBRQK]?)(?<From>[a-h]{1}[1-8]{1})(?<Take>[-x]?)(?<To>[a-h]{1}[1-8]{1})(?<Promote>[NBRQ]?)$)";
        private static readonly Regex regex = new Regex(regexExpression, RegexOptions.Compiled);

        internal static Option<Move> ToMove(this string parameter)
        {
            Match match = regex.Match(parameter);
            if (match.Success)
            {
                string value;
                PieceEnum piece = PieceEnum.Pawn;
                SquareEnum from = SquareEnum.a1;
                SquareEnum to = SquareEnum.a1;
                PieceEnum promote = PieceEnum.Pawn;
                if (IsKingsideRokade(match.Groups))
                    return Move.Of(RokadeEnum.KingSide);
                else if (IsQueensideRokade(match.Groups))
                    return Move.Of(RokadeEnum.QueenSide);
                else
                {
                    if (IsPawnMove(match.Groups))
                    {
                        piece = PieceEnum.Pawn;
                        if (TryGetGroupValue(match.Groups, "Promote", out value))
                            value.TryPieceParse().Map(p => promote = p);
                        else
                            promote = piece;
                    }
                    else
                    {
                        if (!TryGetGroupValue(match.Groups, "Piece", out value))
                            return None;
                        value.TryPieceParse().Map(p => promote = piece = p);
                        if (TryGetGroupValue(match.Groups, "Promote", out value))
                            return None;
                    }
                    if (!TryGetGroupValue(match.Groups, "From", out value))
                        return None;
                    from = value.ParseEnum<SquareEnum>();
                    if (!TryGetGroupValue(match.Groups, "To", out value))
                        return None;
                    to = value.ParseEnum<SquareEnum>();
                }

                return Move.Of(piece, from, to, promote);
            }
            else
                return None;
        }

        private static bool TryGetGroupValue(GroupCollection Group, string Name, out string Value)
        {
            Value = Group[Name].Value;
            return !string.IsNullOrWhiteSpace(Value);
        }

        private static bool IsPawnMove(GroupCollection Group)
            => !TryGetGroupValue(Group, "Piece", out string Value);

        private static bool IsKingsideRokade(GroupCollection Group)
            => TryGetGroupValue(Group, "KingRokade", out string Value);

        private static bool IsQueensideRokade(GroupCollection Group)
            => TryGetGroupValue(Group, "QueenRokade", out string Value);
    }
}
