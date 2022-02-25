using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.ApplicationData
{
    public abstract class ArtefactExporter
    {
        public abstract bool Export(string path, List<OTL_ArtefactType> artefacts);
    }
}
