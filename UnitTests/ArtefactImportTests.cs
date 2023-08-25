using OTLWizard.Helpers;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class ArtefactImportTests
    {
        [Fact]
        public void ArtefactImportTest_3_types()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);

            var dbpath = "./../../Geometrie_Artefact_0.db";
            var artefactImporter = new ArtefactImporter(dbpath);

            // act
            subsetImporter.Import(true, true);
            artefactImporter.Import(subsetImporter.GetOTLObjectTypes().Select(x => x.otlName));

            // assert
            var art_types = artefactImporter.GetOTLArtefactTypes();
            var poort = art_types.FirstOrDefault(a => a.URL == "https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#Netwerkpoort");
            var netwerkelement1 = art_types.FirstOrDefault(a => a.URL == "https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#Netwerkelement" && a.overervenvan == "Rack");
            var netwerkelement2 = art_types.FirstOrDefault(a => a.URL == "https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#Netwerkelement" && a.overervenvan == "IndoorKast");
            var rack = art_types.FirstOrDefault(a => a.URL == "https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#Rack");
            var kast = art_types.FirstOrDefault(a => a.URL == "https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#IndoorKast");

            Assert.Equal("ja", poort.opmerkingen);
            Assert.Equal("ja", netwerkelement1.opmerkingen);
            Assert.Equal("neen", netwerkelement2.opmerkingen);
            Assert.Equal("neen", rack.opmerkingen);
            Assert.Null(kast);
        }
    }
}