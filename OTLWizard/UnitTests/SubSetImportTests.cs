using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OTLWizard.ApplicationData;
using Xunit;


namespace OTLWizard.UnitTests
{
    public class SubSetImportTests
    {
        [Fact]
        public void SubsetImportTest_1_type()
        {
            // arrange
            var dbpath = Directory.GetCurrentDirectory() + "\\UnitTests\\test_1_type.db";
            var subsetImporter = new SubsetImporter(dbpath, "", null);
            
            // act
            subsetImporter.ImportSubset();
            
            // assert
            var objectTypes = subsetImporter.GetOTL_ObjectTypes();
            Assert.Equal(1, objectTypes.Count);
            var objectType = objectTypes.FirstOrDefault();
            Assert.Equal("https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#DNBLaagspanning", objectType.uri);
            
        }
    }
}