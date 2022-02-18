using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Xunit;
using OTLWizard.Helpers;

namespace UnitTests
{
    public class TemplateExporterTests
    {
        [Fact]
        public void ArtefactImportTest_3_types()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);
            var path_save_to = "test_result.xlsx";
            
            // act
            subsetImporter.Import();
            var exporter = new TemplateExporter(subsetImporter.GetOTLObjectTypes());
            exporter.ExportXls(path: Directory.GetCurrentDirectory() + "\\" + path_save_to, help: false, checklistoptions:false);
 
            // assert
            Application excel;
            Workbook workbook;
            excel = new Application {Visible = false, DisplayAlerts = false};
            workbook = excel.Workbooks.Open(Directory.GetCurrentDirectory() + "\\" + path_save_to);
            var xlWorkSheet = (Worksheet) workbook.Worksheets["Netwerkpoort"];
            Assert.NotNull(xlWorkSheet);
            workbook.Close();
            excel.Quit();
        }
    }
}