using System;
using System.Collections.Generic;
using System.Configuration;

namespace DotNetConfigHelper.Providers
{
    public class AppSettingsProvider  : BaseSettingsProvider, ISettingsProvider
    {
        public AppSettingsProvider()
            : base("{([^}]*)}")
        {
            
        }

        public IDictionary<string, string> Load()
        {
            var results = new Dictionary<string, string>();

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                var rawValue = ConfigurationManager.AppSettings[key];
                results[key] = rawValue;
            }
            return results;
        }


    }
}
