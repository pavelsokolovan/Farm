using System.Configuration;

namespace Farm
{
    public class Settings
    {
        public static readonly string API_TOKEN = ConfigurationManager.AppSettings["token"];
    }
}