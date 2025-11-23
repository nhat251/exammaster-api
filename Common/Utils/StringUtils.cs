using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class StringUtils
    {
        public static string? TrimAndLower(string input)
        {
            if (input == null) return null;
            return input.Trim().ToLowerInvariant();
        }
    }
}
