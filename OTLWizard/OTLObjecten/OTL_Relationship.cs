using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.OTLObjecten
{
    public class OTL_Relationship
    {

        [Name("assetid.identificator")]
        public string assetID { get; set; }
        [Name("typeuri")]
        public string relationshipURI { get; set; }
        [Name("doelassetid.identificator")]
        public string doelID { get; set; }
        [Name("bronassetid.identificator")]
        public string bronID { get; set; }
        [Ignore]
        public bool isDirectional { get; set; }
        [Ignore]
        public string DisplayName { get; set; }

        public OTL_Relationship()
        {
            // for serialization
        }
    }
}
