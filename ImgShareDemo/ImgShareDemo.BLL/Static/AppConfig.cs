using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImgShareDemo.BLL.Static
{
    public class AppConfig
    {
        public static readonly AppConfig LinkedInClientId = new AppConfig("LinkedInClientId");
        public static readonly AppConfig LinkedInClientSecret = new AppConfig("LinkedInClientSecret");
        public static readonly AppConfig LinkedBaseUrl = new AppConfig("LinkedBaseUrl");

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
