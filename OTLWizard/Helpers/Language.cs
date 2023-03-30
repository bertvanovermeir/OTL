using System.Collections.Generic;
using System.IO;

namespace OTLWizard.Helpers
{
    public static class Language
    {
        private static string language = "";
        private static Dictionary<string, string> languages = new Dictionary<string, string>();

        public static void Init()
        {
            languages.Clear();
            language = Settings.Get("language");
            if (language.Equals(""))
                language = "NL";

            if (File.Exists("data\\lang.txt"))
            {
                string[] lines = File.ReadAllLines("data\\lang.txt", System.Text.Encoding.UTF8);
                foreach (string item in lines)
                {
                    if (item.Contains("=") && item.Contains("<" + language.ToUpper() + ">"))
                    {
                        try
                        {
                            string key = item.Split('=')[0].Split('>')[1];
                            string value = item.Split('=')[1].Replace("<br>", "\n");
                            languages.Add(key.ToLower(), value);
                        }
                        catch
                        {
                            // that is not a valid assignment parameter
                        }
                    }
                }
            }
        }

        public static string Get(string key)
        {
            try
            {
                return languages[key.ToLower()];
            }
            catch
            {
                return "<" + key + ">";
            }
        }

    }
}
