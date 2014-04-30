using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetConfigHelper.Providers
{
    public class RegexReplacer
    {
        private Regex regex;
        public RegexReplacer(string pattern)
        {
            regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool Matches(string text)
        {
            return regex.IsMatch(text);
        }

        public string Replace(string text, Func<string,string> getSetting)
        {
            var matches = regex.Matches(text);

            foreach (Match m in matches)
            {
                var token = m.Groups[0].Value;
                var resolved = getSetting(m.Groups[1].Value);
                if (resolved == null)
                    continue;
                text = text.Replace(token, resolved);
            }
            return text;
        }

        public string[] GetTokens(string text)
        {
            var matches = regex.Matches(text);

            var tokens = new List<string>();
            foreach (Match m in matches)
            {
                tokens.Add(m.Groups[0].Value);
            }

            return tokens.ToArray();
        }
    }
}
