using CsvHelper.Configuration.Attributes;
using OTLWizard.Helpers;
using System;
using System.Collections.Generic;

namespace OTLWizard.Helpers
{
    public class OTL_Entity
    {
        public string Name { get; set; }
        public string TypeUri { get; set; }
        public string AssetId { get; set; }
        public string DisplayName { get; set; }
        public SerializableDictionary<string, string> GlobalWKT { get; set; }
        public SerializableDictionary<string, string> Properties { get; set; }


        public OTL_Entity()
        {
            Properties = new SerializableDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            GlobalWKT = new SerializableDictionary<string, string>();
        }

        public SerializableDictionary<string, string> GetProperties()
        {
            return Properties;             
        }

        public void GenerateDisplayName()
        {
            DisplayName = OTLUtils.SimplifyBase64Notation(AssetId) + " | " + this.Name;
            if (Boolean.Parse(Settings.Get("showassetnamewherepossible")))
            {
                if (Properties.ContainsKey("naam"))
                {
                    var naam = Properties["naam"];
                    if (!naam.Equals(""))
                    {
                        DisplayName = naam + " | " + this.Name;
                    }
                }
            }
        }

        public string GetPreferredDisplayValue()
        {
            return DisplayName.Split('|')[0].Trim();
        }

    }
}
