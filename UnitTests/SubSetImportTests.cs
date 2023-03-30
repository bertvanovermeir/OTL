using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class SubSetImportTests
    {
        [Fact]
        public void SubsetImportTest_1_type_GeenKeuzelijsten()
        {
            // arrange           
            var dbpath = "./../../subset_1_type_alle_attributen_bert.db";
            var subsetImporter = new SubsetImporter(dbpath);

            // act
            subsetImporter.Import();

            // assert
            var objectTypes = subsetImporter.GetOTLObjectTypes();
            Assert.Single(objectTypes);
            var objectType = objectTypes.FirstOrDefault();
            Assert.Equal("https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#DNBLaagspanning", objectType.uri);
            Assert.False(objectType.deprecated);
            Assert.Equal(45, objectType.GetParameters().Count);
        }

        [Fact]
        public void SubsetImportTest_1_type_Keuzelijsten()
        {
            Settings.Init();
            // arrange           
            var dbpath = "./../../subset_1_type_alle_attributen_bert.db";
            var subsetImporter = new SubsetImporter(dbpath, true);

            // act
            subsetImporter.Import();

            // assert
            var objectTypes = subsetImporter.GetOTLObjectTypes();
            Assert.Single(objectTypes);
            var objectType = objectTypes.FirstOrDefault();
            Assert.Equal("https://wegenenverkeer.data.vlaanderen.be/ns/onderdeel#DNBLaagspanning", objectType.uri);
            Assert.False(objectType.deprecated);
            Assert.Equal(45, objectType.GetParameters().Count);
            var parameterNames = objectType.GetParameters().Select(p => p.DotNotatie);
            Assert.Contains("adresVolgensDNB.provincie", parameterNames);
            var z = objectType.GetParameters().Where(w => w.DotNotatie == "adresVolgensDNB.provincie").Select(q => q).FirstOrDefault();
            Assert.Equal(6, z.DropdownValues.Count);
        }

        [Fact]
        public void SubsetImportTest_Alle_types()
        {
            // arrange
            var dbpath = "./../../subset_all_v2.0.2.db";
            var subsetImporter = new SubsetImporter(dbpath, true);

            // act
            subsetImporter.Import();

            // assert
            var objectTypes = subsetImporter.GetOTLObjectTypes();
            Assert.Equal(403, objectTypes.Count);
        }
    }
}