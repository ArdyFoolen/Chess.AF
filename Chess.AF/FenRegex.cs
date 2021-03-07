using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chess.AF
{
    public class FenRegex
    {
        private static string regexString = @"^([rnbqkpRNBQKP1-8]{1,8}\/){7}([rnbqkpRNBQKP1-8]{1,8}){1}\s[wb]{1}\s[kqKQ-]{1,4}\s(-|([a-h]{1}[36]{1}))\s[0-9]+\s[0-9]+$";
        private static Regex regex = new Regex(regexString, RegexOptions.Compiled);
        public static bool IsValid(string value)
        {
            var match = regex.Match(value);
            return string.IsNullOrEmpty(value) ? false : regex.IsMatch(value);
        }
    }
}
