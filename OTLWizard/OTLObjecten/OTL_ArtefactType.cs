using CsvHelper.Configuration.Attributes;
using System.ComponentModel;

namespace OTLWizard.Helpers
{
    public class OTL_ArtefactType
    {
        [Name("Naam OTL Object")]
        [DisplayName("Naam OTL Object")]
        public string objectnaam { get; set; }
        [Name("Verwacht Geometrietype")]
        [DisplayName("Verwacht Geometrietype")]
        public string geometrie { get; set; }
        [Name("Meten of Overerven")]
        [DisplayName("Meten of Overerven")]
        public string overerving { get; set; }
        [Name("Meetcriterium")]
        [DisplayName("Meetcriterium")]
        public string meetcriterium { get; set; }
        [Name("Overervingsgrens (in m)")]
        [DisplayName("Overervingsgrens (in m)")]
        public string overervingsgrens { get; set; }
        [Name("Meten volgens Steekkaart(en)")]
        [DisplayName("Meten volgens Steekkaart(en)")]
        public string steekkaarten { get; set; }
        [Name("Overerven van")]
        [DisplayName("Overerven van")]
        public string overervenvan { get; set; }
        [Name("Overerven via Relatie")]
        [DisplayName("Overerven via Relatie")]
        public string viarelatie { get; set; }
        [Name("Uitzonderingen")]
        [DisplayName("Uitzonderingen")]
        public string uitzonderingen { get; set; }
        [Name("Overervingsklasse in Subset")]
        [DisplayName("Overervingsklasse in Subset")]
        public string opmerkingen { get; set; }

        public string URL; // not for user export

        public OTL_ArtefactType(string objectnaam, string geometrie, string overerving, string meetcriterium, string uitzonderingen, string overervingsgrens, string steekkaarten, string overervenvan, string viarelatie, string URL)
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
            this.URL = URL;
        }
    }
}
