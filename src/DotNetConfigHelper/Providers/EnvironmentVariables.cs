using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetConfigHelper.Providers
{
    public class EnvironmentVariables : BaseSettingsProvider, ISettingsProvider
    {
        public EnvironmentVariables()
            : base("%([^%]*)%")
        {
        }

        public IDictionary<string, string> Load()
        {
            var results = new Dictionary<string, string>();

            foreach (DictionaryEntry e in Environment.GetEnvironmentVariables())
            {
                results[(string) e.Key] = (string) e.Value;
            }
            return results;
        }
      
    }
}
