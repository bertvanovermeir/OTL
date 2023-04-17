using OTLWizard.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace OTLWizard.Helpers
{
    public abstract class SubsetExporter
    {
        public List<OTL_ObjectType> OTL_ObjectTypes;
        public string[] classes;

        public bool SetOTLSubset(List<OTL_ObjectType> OTL_ObjectTypes)
        {
            if (OTL_ObjectTypes == null)
            {
                return false;
            }
            else if (OTL_ObjectTypes.Count == 0)
            {
                return false;
            }
            else
            {
                this.OTL_ObjectTypes = OTL_ObjectTypes;
                return true;
            }
        }

        public bool SetSelectedClassesByUser(string[] classes)
        {
            if (classes == null)
            {
                this.classes = OTL_ObjectTypes.Select(x => x.otlName).ToArray();
                return true;
            }
            else if (classes.Length == 0)
            {
                return false;
            }
            else
            {
                this.classes = classes;
                return true;
            }
        }

        public abstract bool Export(string path, List<OTL_ArtefactType> oTL_ArtefactTypes, int amountExamples, bool help, bool checklistoptions = false, bool dummydata = false, bool wkt = false, bool deprecated = false);


    }
}
