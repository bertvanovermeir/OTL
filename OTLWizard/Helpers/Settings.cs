using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.Helpers
{
    static class Settings
    {

        public static Dictionary<string, string> values;

        public static bool Init()
        {
            values = new Dictionary<string, string>();

            if (File.Exists("data\\settings.txt"))
            {
                string[] lines = File.ReadAllLines("data\\settings.txt", System.Text.Encoding.UTF8);
                foreach (string item in lines)
                {
                    if (item.Contains("="))
                    {
                        try
                        {
                            string key = item.Split('=')[0];
                            string value = item.Split('=')[1];
                            values.Add(key.ToLower(), value.ToLower());
                        } catch
                        {
                            // that is not a valid assignment parameter
                        }                 
                    }
                }
                return true;
            } else
            {
                return false;
            }
        }

        public static string Get(string key)
        {
            try
            {
                return values[key];
            } catch
            {
                return "";
            }           
        }
    }
}