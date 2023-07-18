using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeGeneratorToolKit.Utilities
{
    public static class PatternUtitlity
    {

        public static string RemoveListType(this string input)
        {
            return input.Replace("List<", "").Replace(">>", ">");
        }
    }
}
