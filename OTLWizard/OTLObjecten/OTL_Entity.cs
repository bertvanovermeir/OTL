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

        //public Dictionary<string, string> Properties { get; set; }
        public SerializableDictionary<string, string> Properties { get; set; }


        public OTL_Entity()
        {
            Properties = new SerializableDictionary<string, string>();
        }

        public void GenerateDisplayName()
        {
            var tmp = AssetId;

            if (Boolean.Parse(Settings.Get("hidebase64cosmetic")))
            {
                // change the relationshipuris, if applicable, if not silently fail.
                // they look like this: 632f6526-7bdf-4d44-a289-a0a00a993793-b25kZXJkZWVsI0VpbmRzdHVr               
                try
                {
                    var col = tmp.Split('-');
                    var replacer = col[col.Length - 1];
                    tmp = tmp.Replace("-" + replacer, "");
                }
                catch
                {
                    // silent error
                }
            }

            this.DisplayName = tmp + " | " + this.Name;
        }
    }
}
