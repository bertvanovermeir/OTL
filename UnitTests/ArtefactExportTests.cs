using OTLWizard.ApplicationData;
using OTLWizard.OTLObjecten;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class ArtefactExportTests
    {
        [Fact]
        public void ArtefactExportTest_3_types_CSV()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);

            var dbpath = "./../../Geometrie_Artefact_0.db";
            var artefactImporter = new ArtefactImporter(dbpath);

            // act
            subsetImporter.Import();
            artefactImporter.Import(subsetImporter.GetOTLObjectTypes().Select(x => x.otlName));
            var art_types = artefactImporter.GetOTLArtefactTypes();
            ArtefactExporterCSV csv = new ArtefactExporterCSV();
            csv.Export("./../../ArtefactImportTest_3_types.csv", art_types);

            
            // assert            

        }
    }
}
