using OTLWizard.OTLObjecten;
using System.Collections.Generic;

namespace OTLWizard.ApplicationData
{
    public abstract class ArtefactExporter
    {
        public abstract bool Export(string path, List<OTL_ArtefactType> artefacts);
    }
}
