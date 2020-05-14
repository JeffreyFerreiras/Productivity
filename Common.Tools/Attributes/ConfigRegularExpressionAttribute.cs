using System.ComponentModel.DataAnnotations;

namespace Common.Tools.Attributes
{
    public class ConfigRegularExpressionAttribute : RegularExpressionAttribute
    {
        public ConfigRegularExpressionAttribute(string configKeyPattern)
            : base(Configuration.ConfigReader.GetAppSetting(configKeyPattern))
        {
            /*
             * This class is used for keeping the regex in the config file,
             * making it possible to change username and password requirements without changing code.
             */
        }
    }
}