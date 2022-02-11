using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.OTLObjecten
{
    public class OTL_ArtefactType
    {
        [DisplayName("Naam OTL Object")]
        public string objectnaam { get; set; }
        [DisplayName("Verwacht Geometrietype")]
        public string geometrie { get; set; }
        [DisplayName("Meten of Overerven")]
        public string overerving { get; set; }
        [DisplayName("Meetcriterium")]
        public string meetcriterium { get; set; }
        [DisplayName("Overervingsgrens (in m)")]
        public string overervingsgrens { get; set; }
        [DisplayName("Meten volgens Steekkaart(en)")]
        public string steekkaarten { get; set; }
        [DisplayName("Overerven van")]
        public string overervenvan { get; set; }
        [DisplayName("Overerven via Relatie")]
        public string viarelatie { get; set; }
        [DisplayName("Uitzonderingen")]
        public string uitzonderingen { get; set; }
        [DisplayName("Opmerkingen")]
        public string opmerkingen { get; set; }


        public OTL_ArtefactType(string objectnaam, string geometrie, string overerving, string meetcriterium, string uitzonderingen, string overervingsgrens, string steekkaarten, string overervenvan, string viarelatie)
        {
            this.objectnaam = objectnaam;
            this.geometrie = geometrie;
            this.overerving = overerving;
            this.meetcriterium = meetcriterium;
            this.uitzonderingen = uitzonderingen;
            this.overervingsgrens = overervingsgrens;
            this.steekkaarten = steekkaarten;
            this.overervenvan = overervenvan;
            this.viarelatie = viarelatie;
        }

        public void addRemark(string text)
        {
            opmerkingen = text;
        }
    }
}
