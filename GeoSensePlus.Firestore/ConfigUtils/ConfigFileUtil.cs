using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GeoSensePlus.Firestore.ConfigUtils
{
    static public class ConfigFileUtil
    {
        static string _configDirPath;
        static string _configFile;

        static public string GetConfigDirPath()
        {
            if (_configDirPath != null)
            {
                return _configDirPath;
            }
            else
            {
                LoadConfigDirPath();
                return _configDirPath;
            }
        }

        /// <summary>
        /// Load GeoSensePlus/* order:
        /// 1. $TARGET_PATH
        /// 2. user directory ($USERPROFILE for win, $HOME for linux)
        /// 3. /etc/
        /// </summary>
        static private void LoadConfigDirPath()
        {
            _configDirPath = $"/etc/{Variables.configDirectory}";

            string targetPath = Environment.GetEnvironmentVariable("TARGET_PATH");
            if (targetPath != null)
            {
                _configDirPath = $"{targetPath}/{Variables.configDirectory}";
            }
            else
            {
                // load from user's directory

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    _configDirPath = $"{Environment.GetEnvironmentVariable("USERPROFILE")}/{Variables.configDirectory}"; // runs as Windows gsv CLI
                }
                else
                {
                    _configDirPath = $"{Environment.GetEnvironmentVariable("HOME")}/{Variables.configDirectory}"; // runs as Linux gsv CLI
                }
            }
        }

        static public string GetConfigFile()
        {
            if (_configFile == null)
            {
                _configFile = $"{GetConfigDirPath()}/{Variables.configFile}";
            }
            return Path.GetFullPath(_configFile);   // normalize '/' or '\' according to OS
        }
    }
}
