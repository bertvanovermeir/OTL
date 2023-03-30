using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OTLWizard.Helpers
{
    public static class Settings
    {

        public static Dictionary<string, string> values;

        public static bool Init()
        {
            values = new Dictionary<string, string>();

            var localPath = System.IO.Path.GetTempPath() + "otlsettingsv9\\";
            // create the folder if it does not exist
            Directory.CreateDirectory(localPath);

            if (File.Exists(localPath + "settings.txt"))
            {
                string[] lines = File.ReadAllLines(localPath + "settings.txt", System.Text.Encoding.UTF8);
                ProcessFileContents(lines);
                return true;
            }
            else if (File.Exists("data\\settings.txt"))
            {
                string[] lines = File.ReadAllLines("data\\settings.txt", System.Text.Encoding.UTF8);
                ProcessFileContents(lines);
                // backup overwrite
                try { File.WriteAllLines(localPath + "settings.txt", values.Select(x => x.Key + "=" + x.Value).ToArray()); } catch { };
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string Get(string key)
        {
            try
            {
                return values[key.ToLower()].ToLower();
            }
            catch
            {
                return "";
            }
        }

        public static string GetRaw(string key)
        {
            try
            {
                return values[key];
            }
            catch
            {
                return "";
            }
        }

        public static bool Update(string key, string value)
        {
            if (values.ContainsKey(key.ToLower()))
            {
                values[key.ToLower()] = value;

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool WriteSettings()
        {
            try
            {
                // save to file
                var localPath = System.IO.Path.GetTempPath() + "otlsettingsv9\\";
                // create the folder if it does not exist
                Directory.CreateDirectory(localPath);
                // write it
                File.WriteAllLines(localPath + "settings.txt", values.Select(x => x.Key + "=" + x.Value).ToArray());
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static string GetSettingsPath()
        {
            return System.IO.Path.GetTempPath() + "otlsettingsv9\\" + "settings.txt";
        }

        private static void ProcessFileContents(string[] lines)
        {
            foreach (string item in lines)
            {
                if (item.Contains("="))
                {
                    try
                    {
                        string key = item.Split('=')[0];
                        string value = item.Split('=')[1];
                        values.Add(key.ToLower(), value);
                    }
                    catch
                    {
                        // that is not a valid assignment parameter
                    }
                }
            }
        }
    }
}