using System;
using System.IO;
using System.Linq;

namespace DotNetConfigHelper
{
    /// <summary>
    ///     Represents an item of config. Allows access to the key and value. 
    ///     Has a bunch of utility methods for converting the string to something useful and also allows extension methods to be cleanly added / chainged
    /// </summary>
    public class ConfigItem
    {
        private readonly string key;
        private string value;

        public ConfigItem(string key, string value)
        {
            if (key == null) throw new ArgumentNullException("key");
            this.key = key;
            this.value = value;
        }

        public string Key
        {
            get { return key; }
        }

        public string Value
        {
            get { return value; }
        }

        public string[] ToArray(char[] splitChars = null, bool distinct = true)
        {
            return value.Split(new[] {',', ';', ' ', '\t', '\n'}).Distinct().ToArray();
        }

        public TEnum ToEnum<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof (TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            if (!Enum.IsDefined(typeof (TEnum), this.value))
            {
                var values = Enum.GetValues(typeof (TEnum));
                throw new ArgumentException("The value for key: \"" + this.key + "\" must be one of the following: " +
                                            String.Join(",", values));
            }

            return (TEnum) Enum.Parse(typeof (TEnum), this.value);
        }

        public ConfigItem MustNotBeEmpty()
        {
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("AppSetting:  \"" + key + "\" is missing!");
            return this;
        }

        public Uri ToUri(string scheme = null)
        {
            Uri uri;
            if (!Uri.TryCreate(value, UriKind.Absolute, out uri))
                throw new ArgumentException("Not a valid Uri. AppSetting:  \"" + key + "\" value: \"" + value + "\"");

            if (scheme != null && String.Compare(scheme, uri.Scheme, StringComparison.InvariantCultureIgnoreCase) != 0)
                throw new ArgumentException("This Uri:  \"" + key + "\" value: \"" + value + "\" must have a scheme of: " + scheme);
            return uri;
        }

        public bool ToBool(bool? defaultValue = null)
        {
            bool result;
            if (!bool.TryParse(value, out result) && defaultValue == null)
                throw new ArgumentException("Not a valid Bool (true or false only). AppSetting:  \"" + key +
                                            "\" value: \"" + value + "\"");


            return bool.TryParse(value, out result) ? result : defaultValue ?? false;
        }

        public int ToInt(int? defaultValue= null)
        {
            Int32 result;
            if (!Int32.TryParse(value, out result) && defaultValue == null)
                throw new ArgumentException("Not a valid Int. AppSetting:  \"" + key +
                                            "\" value: \"" + value + "\"");
            return Int32.TryParse(value, out result) ? result : (defaultValue ?? 0);
        }

        public long ToLong(long? defaultValue = null)
        {
            long result;
            if (!long.TryParse(value, out result) && defaultValue == null)
                throw new ArgumentException("Not a valid long. AppSetting:  \"" + key +
                                            "\" value: \"" + value + "\"");
            return long.TryParse(value, out result) ? result : (defaultValue ?? 0);
        }

        public decimal ToDecimal(long? defaultValue = null)
        {
            decimal result;
            if (!decimal.TryParse(value, out result) && defaultValue == null)
                throw new ArgumentException("Not a valid decimal. AppSetting:  \"" + key +
                                            "\" value: \"" + value + "\"");

            return decimal.TryParse(value, out result) ? result : (defaultValue ?? 0);
        }

        public float ToFloat(long? defaultValue = null)
        {
            float result;
            if (!float.TryParse(value, out result) && defaultValue == null)
                throw new ArgumentException("Not a valid decimal. AppSetting:  \"" + key +
                                            "\" value: \"" + value + "\"");
            return float.TryParse(value, out result) ? result : (defaultValue ?? 0);
        }

        public double ToDouble(double? defaultValue = null)
        {
            double result;
            if (!double.TryParse(value, out result) && defaultValue == null)
                throw new ArgumentException("Not a valid decimal. AppSetting:  \"" + key +
                                            "\" value: \"" + value + "\"");
            return double.TryParse(value, out result) ? result : (defaultValue ?? 0);
        }

        public ConfigItem FileMustExist()
        {
            if (!File.Exists(value)) throw new ArgumentException("File Not Found!. AppSetting:  \"" + key + "\" value: \"" + value + "\"");
            return this;
        }

        public ConfigItem DirectoryMustExist(bool isFilename = false)
        {
            var dir = isFilename ? Path.GetDirectoryName(value) : value;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return this;
        }

        public DirectoryInfo ToDirectoryInfo()
        {
            return new DirectoryInfo(this.value);
        }

        public FileInfo ToFileInfo()
        {
            return new FileInfo(this.value);
        }
    }
}
