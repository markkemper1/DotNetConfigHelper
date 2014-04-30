using System;
using System.Collections.Generic;

namespace DotNetConfigHelper.Providers
{
    public interface ISettingsProvider
    {
        /// <summary>
        ///     Replaces any tokens in the text with tere values.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string ReplaceTokens(string text, Func<string,string> getSettingValue);

        /// <summary>
        ///     Tests if the text contains any tokens that should be replaced.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        bool HasToken(string text);

        /// <summary>
        /// Gets a list of tokens in the text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string[] GetTokens(string text);

        /// <summary>
        ///     Get the settings.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> Load();
    }
}
