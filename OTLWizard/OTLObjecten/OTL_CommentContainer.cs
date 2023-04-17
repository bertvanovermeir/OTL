using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.OTLObjecten
{
    public  class OTL_CommentContainer
    {
        [System.ComponentModel.DisplayName("Opmerking")]
        public string Comment { get; set; }
        [System.ComponentModel.DisplayName("Type")]
        public string Type { get; set; }
        [System.ComponentModel.DisplayName("AssetId.Identificator")]
        public string AssetId { get; set; }
        [System.ComponentModel.DisplayName("Attribuut")]
        public bool IsAttribute { get; set; }
        [System.ComponentModel.DisplayName("Attribuutnaam")]
        public string AttributeName { get; set; }
        [System.ComponentModel.DisplayName("Originele attribuutwaarde")]
        public string originalAttributeValue { get; set; }
        [System.ComponentModel.DisplayName("Nieuwe attribuutwaarde")]
        public string newAttributeValue { get; set; }

        public OTL_CommentContainer()
        {
            IsAttribute = false;
        }
    }
}
