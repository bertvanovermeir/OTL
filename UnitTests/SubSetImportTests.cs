using System.Linq;
using Xunit;
using OTLWizard.ApplicationData;

namespace UnitTests
{
    public class SubSetImportTests
    {
        [Fact]
        public void SubsetImportTest_1_type()
        {
            // arrange           
            var dbpath = "C:\\resources\\subset_1_type_alle_attributen_bert.db";
            var subsetImporter = new SubsetImporter(dbpath);
            
            // act
            subsetImporter.Import();
            
            // assert
            var objectTypes = subsetImporter.GetOTLObjectTypes();
            Assert.Single(objectTypes);
            var objectType = objectTypes.FirstOrDefault();
            Assert.Equal("https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#DNBLaagspanning", objectType.uri);
            
        }

        [Fact]
        public void SubsetImportTest_Alle_types()
        {
            // arrange
            var dbpath = "C:\\resources\\subset_all_v2.0.2.db";
            var subsetImporter = new SubsetImporter(dbpath, "");

            // act
            subsetImporter.Import();

            // assert
            var objectTypes = subsetImporter.GetOTLObjectTypes();
            Assert.Equal(398, objectTypes.Count);
            var objectType = objectTypes.FirstOrDefault();
            Assert.Equal("https://wegenenverkeer.data.vlaanderen.be/ns/installatie#BlindePut", objectType.uri);
        }
    }
}