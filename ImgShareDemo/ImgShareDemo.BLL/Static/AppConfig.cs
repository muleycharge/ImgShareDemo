namespace ImgShareDemo.BLL.Static
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;

    public class AppConfig
    {
        public static readonly AppConfig LinkedInClientId = new AppConfig("LinkedInClientId");
        public static readonly AppConfig LinkedInClientSecret = new AppConfig("LinkedInClientSecret");
        public static readonly AppConfig LinkedInBaseUrl = new AppConfig("LinkedBaseUrl");

        /// <summary>
        /// Run a App start to make sure that the configuration file is correct
        /// </summary>
        /// <param name="missingConfiguration"></param>
        /// <returns></returns>
        public static bool Validate(out string[] missingConfiguration)
        {
            missingConfiguration = typeof(AppConfig).GetFields(BindingFlags.Public | BindingFlags.Static)
              .Where(f => f.FieldType == typeof(AppConfig))
              .Select(f => f.GetValue(null) as AppConfig)
              .Where(f => String.IsNullOrEmpty(f.Value))
              .Select(f => f.Key).ToArray();

            return missingConfiguration.Length == 0;
        }

        public string Key { get; set; }
        public string Value { get; set; }

        private AppConfig(string key)
        {
            Key = key;
            Value = ConfigurationManager.AppSettings[key];
        }
    }
}
