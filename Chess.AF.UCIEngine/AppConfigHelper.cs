using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.UCIEngine
{
    public static class AppConfigHelper
    {
        public static string LogCommandsFilePath
        {
            get
            {
                var filePath = ConfigurationManager.AppSettings.Get("LogCommandsFilePath");
                if (string.IsNullOrWhiteSpace(filePath))
                    return "ArenaCommands.log";
                return filePath;
            }
        }
    }
}
