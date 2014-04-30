using System;
using System.Collections.Generic;
using System.Configuration;
using DotNetConfigHelper.Providers;
using NUnit.Framework;

namespace DotNetConfigHelper.Test
{
    [TestFixture]
    public class ConfigProviderTest 
    {
        private ConfigProvider config = new ConfigProvider(new DictionaryProvider(new Dictionary<string, string>{
                {"a", "a"},
                {"b", "b"},
                {"c", "{a}+{b}"},
                {"d", "{c}"},
                {"e", "{d}-{c}"},
               {"xEnvA", "%envA%"},
               {"xEnvB", "%envB%"},
               {"xEnvC", "%envC%"},
            }), new EnvConfigFileProvider(), new AppSettingsProvider());

        [SetUp]
        public void Setup()
        {

        }

        [TestCase("a", "a")]
        [TestCase("b", "b")]
        [TestCase("c", "a+b")]
        [TestCase("d", "a+b")]
        [TestCase("e", "a+b-a+b")]
        [TestCase("envA", "envA")]
        [TestCase("xEnvB", "envB")]
        [TestCase("xEnvC", "envA+envB")]
        public void should_resolve(string key, string value)
        {
            Assert.That(config[key], Is.EqualTo(value));
        }

        [Test] 
        public void should_throw_for_undefined_token()
        {
            Assert.Throws<ArgumentException>(()=> config.ReplaceTokens("{x}"));
        }

        [Test]
        public void should_be_able_to_replace_appSettings()
        {
            AppSettingsReplacer.Install(config);

            Assert.That(ConfigurationManager.AppSettings["e"], Is.EqualTo("a+b-a+b"));
        }

        [Test]
        public void should_replace_connection_string_from_config()
        {
            AppSettingsReplacer.Install(config);

            Assert.That(ConfigurationManager.ConnectionStrings["default"].ConnectionString, Is.EqualTo("Server=localhost;Database=test_name;User ID=test_user;Password=test_password;"));
            Assert.That(ConfigurationManager.ConnectionStrings["default"].ProviderName, Is.EqualTo("System.Data.SqlClient"));
        }

    }
}
