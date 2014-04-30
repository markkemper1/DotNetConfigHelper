using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetConfigHelper.Providers
{
    public abstract class BaseSettingsProvider
    {
        private readonly RegexReplacer replacer;

        public BaseSettingsProvider(string regexPattern)
        {
            replacer = new RegexReplacer(regexPattern);
        }

        public string ReplaceTokens(string text, Func<string, string> getSettingValue)
        {
            return replacer.Replace(text, getSettingValue);
        }
        public bool HasToken(string text)
        {
            return replacer.Matches(text);
        }

        public string[] GetTokens(string text)
        {

            return replacer.GetTokens(text);
        }
    }
}
