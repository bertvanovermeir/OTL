using CsvHelper.Configuration.Attributes;
using OTLWizard.Helpers;
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
        public string AssetId { get; set; }
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
        [Name("isActief")]
        public bool Activated { get; set; }

        [Ignore]
        public SerializableDictionary<string, string> Properties { get; set; }

        public OTL_Relationship()
        {
            Properties = new SerializableDictionary<string, string>();
        }

        public void GenerateDisplayName()
        {
            if (this.isDirectional)
            {
                this.DisplayName = this.relationshipURI.Split('#')[1] + " | " + this.bronID + " --> " + this.doelID;

            }
            else
            {
                this.DisplayName = this.relationshipURI.Split('#')[1] + " | " + this.bronID + " <--> " + this.doelID;
            }
            // actief of niet
            if(Activated)
                this.DisplayName = this.DisplayName + " (Actief)";
            else
                this.DisplayName = this.DisplayName + " (Verwijderd)";

        }
    }
}
