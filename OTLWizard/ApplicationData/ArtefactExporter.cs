using OTLWizard.Helpers;
using System.Collections.Generic;

namespace OTLWizard.Helpers
{
    public abstract class ArtefactExporter
    {
        public abstract bool Export(string path, List<OTL_ArtefactType> artefacts);
    }
}
