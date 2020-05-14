using Common.Tools.Exceptions;
using Common.Tools.Extensions.IO;
using Common.Tools.Extensions.Validation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Common.Tools.Configuration
{
    public class ConfigReader
    {
        private static IConfiguration Configuration { get; set; }

        static ConfigReader()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                Configuration = builder.Build();
            }
            catch
            {
                //Ignore
            }

            AssertConfigFileExists();
        }

        public static string GetConnectionString(string connStringName)
        {
            string connString = ConfigurationManager.ConnectionStrings[connStringName]?.ConnectionString;

            if (Configuration != null && string.IsNullOrWhiteSpace(connString))
            {
                connString = Configuration.GetConnectionString(connStringName); //Try finding in appsettings.json
            }

            Guard.AssertOperation(connString.IsValid(), $"Connection string \"{connStringName}\" is not set in the app or web config file.");

            return connString;
        }

        private static void AssertConfigFileExists()
        {
            bool hasConfigFile = "*.config".InCurrentDirectory();
            bool hasJsonFile = "appsettings.json".InCurrentDirectory();

            Guard.Assert<FileNotFoundException>(hasConfigFile || hasJsonFile, "Unable to locate config files");
        }

        public static string GetAppSetting(string key)
        {
            string result = ConfigurationManager.AppSettings[key];

            if (Configuration != null && string.IsNullOrWhiteSpace(result))
            {
                try
                {
                    result = GreedyFindSectionValue(key);
                }
                catch
                {
                    //Ignored
                }
            }

            return result;
        }

        private static string GreedyFindSectionValue(string key, IEnumerable<IConfigurationSection> sections = null)
        {
            if (sections == null)
            {
                return GreedyFindSectionValue(key, Configuration.GetChildren());
            }

            IConfigurationSection foundSection = sections.SingleOrDefault(x => x.Key == key);

            if (foundSection == null)
            {
                foreach (var section in sections) //search sub sections
                {
                    var subSections = section.GetChildren();

                    string result = GreedyFindSectionValue(key, subSections);

                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return foundSection?.Value;
        }
    }
}