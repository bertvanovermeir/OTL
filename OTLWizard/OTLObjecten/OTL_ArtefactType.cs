using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.OTLObjecten
{
    public class OTL_ArtefactType
    {
        public string objectnaam;
        public string geometrie;
        public string overerving;
        public string uitzonderingen;
        public string overervingsgrens;
        public string steekkaarten;
        public string overervenvan;
        public string viarelatie;

        public OTL_ArtefactType(string objectnaam, string geometrie, string overerving, string uitzonderingen, string overervingsgrens, string steekkaarten, string overervenvan, string viarelatie)
        {
            this.objectnaam = objectnaam;
            this.geometrie = geometrie;
            this.overerving = overerving;
            this.uitzonderingen = uitzonderingen;
            this.overervingsgrens = overervingsgrens;
            this.steekkaarten = steekkaarten;
            this.overervenvan = overervenvan;
            this.viarelatie = viarelatie;
        }


    }
}
