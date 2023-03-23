using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.OTLObjecten
{
    public class OTL_Entity
    {
        public string Name { get; set; }
        public string TypeUri { get; set; }
        public string AssetId { get; set; }
        public string DisplayName { get; set; }
        public SerializableDictionary<string,string> GlobalWKT { get; set; }
        public SerializableDictionary<string, string> Properties { get; set; }


        public OTL_Entity()
        {
            Properties = new SerializableDictionary<string, string>();
            GlobalWKT = new SerializableDictionary<string, string>();
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
