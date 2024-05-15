using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LoanManagement.Utility
{
    internal class DbUtil
    {
        private static IConfiguration _iconfiguration;
        static DbUtil()
        {
            GetAppSettingsFile();
        }

        public static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();

        }

        public static string GetConnectionString()
        {
            return _iconfiguration.GetConnectionString("LocalConnectionString");
        }
    }
}
