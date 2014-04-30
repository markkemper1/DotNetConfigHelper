using System.Collections.Generic;

namespace DotNetConfigHelper.Providers
{
    public class DictionaryProvider : BaseSettingsProvider, ISettingsProvider
    {
        private readonly IDictionary<string, string> settings;

        public DictionaryProvider(IDictionary<string,string> settings )
            : base("{([^}]*)}")
        {
            this.settings = settings;
        }

        public IDictionary<string, string> Load()
        {
            return new Dictionary<string, string>(settings);
        }
      
    }
}
