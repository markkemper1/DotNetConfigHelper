using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetConfigHelper.Providers;

namespace DotNetConfigHelper
{
    public class ConfigProvider
    {
        private readonly Dictionary<string, string> settings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        private readonly List<Tuple<ISettingsProvider, IDictionary<string, string>>> providerSettings;

        public static ConfigProvider Default { get; set; }

        public ConfigProvider()
            : this(new EnvConfigFileProvider(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)))
        {
        }

        public ConfigProvider(params ISettingsProvider[] providers)
        {
            providerSettings = providers.Select(p => new Tuple<ISettingsProvider, IDictionary<string, string>>(p, p.Load())).ToList();

            var rawSettings = new Dictionary<string, string>();

            //Add all settings to global dictionary
            foreach (var p in providerSettings)
            {
                foreach (var kv in p.Item2)
                    rawSettings[kv.Key] = kv.Value;
            }

            var problemKeys = new List<string>();
            settings = new Dictionary<string, string>(rawSettings);
            foreach (var key in rawSettings.Keys)
            {
                try
                {
                    settings[key] = ReplaceTokens(settings[key]);
                }
                catch (ArgumentException ex)
                {
                    problemKeys.Add(key);
                }
            }

            if (problemKeys.Count > 0)
                throw new InvalidOperationException("The following keys could not be resolved or are cirucare references: {0}".Fmt(String.Join(", ", problemKeys.ToArray())));
        }

        public string ReplaceTokens(string text)
        {
            int times = 0;

            var changed = new List<string>();
            string result = text;

            while (true)
            {
                if (String.IsNullOrWhiteSpace(result)) return result;

                changed.Clear();

                string initial = result;

                foreach (var p in providerSettings)
                {
                    result = p.Item1.ReplaceTokens(result, GetString);
                }

                bool allDone = true;
                foreach (var p in providerSettings)
                {
                    allDone = allDone && !p.Item1.HasToken(result);
                    changed.AddRange(p.Item1.GetTokens(result));
                }

                if (initial == result && allDone)
                {
                    return result;
                }


                times++;

                if (times > 50)
                    throw new ArgumentException("Failed to resolve settings, problem tokens: {0}".Fmt(String.Join(", ", changed.ToArray())));
            }

        }

        public string this[string key]
        {
            get { return GetString(key); }
        }

        public IEnumerable<KeyValuePair<string, string>> All
        {
            get { return new Dictionary<string, string>(settings); }
        }

        public string GetString(string key)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("key cannot be empty");

            return settings.ContainsKey(key) ? settings[key] : null;
        }

        internal string GetAnyKey(string key)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("key cannot be empty");

            return settings.ContainsKey(key) ? settings[key] : null;
        }

        public ConfigItem Required(string key, string error = null)
        {
            var result = Optional(key, null).MustNotBeEmpty();
            return result;
        }

        //private ConfigItem Get(string key, string defaultValue)
        //{
        //    var result = GetString(key);
        //    if (String.IsNullOrWhiteSpace(result))
        //    {
        //        Console.WriteLine("No setting detected for: \"{0}\". Using default value of \"{1}\"", key, defaultValue);
        //    }
        //    return new ConfigItem(key, result ?? defaultValue);
        //}

        public ConfigItem Optional(string key, string defaultValue)
        {
            return new ConfigItem(key, GetString(key) ?? defaultValue);

        }

        public static ConfigProvider CreateAndSetDefault()
        {
            Default = new ConfigProvider(new AppSettingsProvider(), new EnvConfigFileProvider());
            return Default;
        }

    }
}
