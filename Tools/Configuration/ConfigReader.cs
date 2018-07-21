using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;
using Tools.Exceptions;
using Tools.Extensions.IO;
using Tools.Extensions.Validation;

namespace Tools.Configuration
{
    public class ConfigReader
    {
        private static IConfiguration Configuration { get; set; }

        static ConfigReader()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            AssertConfigFileExists();
        }

        public static string GetConnectionString(string connStringName)
        {
            string connString = ConfigurationManager.ConnectionStrings[connStringName]?.ConnectionString;

            if(Configuration != null && string.IsNullOrWhiteSpace(connString))
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
    }
}