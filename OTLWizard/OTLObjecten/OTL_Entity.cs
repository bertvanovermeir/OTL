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

        public Dictionary<string, string> Properties { get; set; }



        public OTL_Entity()
        {
            Properties = new Dictionary<string, string>();
        }
    }
}
