using System.IO;
using Microsoft.Office.Interop.Excel;
using Xunit;
using OTLWizard.OTLObjecten;
using System.Collections.Generic;

namespace UnitTests
{
    public class TemplateExporterTests
    {
        [Fact]
        public void TemplateExportTest_3_types_NoChecklistOptions()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);
            var path_save_to = "test_result.xlsx";
            
            // act
            subsetImporter.Import();
            var exporter = new SubsetExporterXLS();
            exporter.SetOTLSubset(subsetImporter.GetOTLObjectTypes());
            bool success = exporter.Export(path: Directory.GetCurrentDirectory() + "\\" + path_save_to, help: false, checklistoptions:false);
 
            // assert
            var excel = new Application {Visible = false, DisplayAlerts = false};
            var workbook = excel.Workbooks.Open(Directory.GetCurrentDirectory() + "\\" + path_save_to);
            // add worksheet names to a list
            List<string> WSNames = new List<string>();
            foreach (Worksheet ws in workbook.Worksheets)
            {
                WSNames.Add(ws.Name);
            }
            // check sheet names
            Assert.True(success);
            Assert.DoesNotContain("Sheet1", WSNames);
            Assert.DoesNotContain("dropdownvalues", WSNames);
            Assert.Contains("Netwerkpoort", WSNames);
            Assert.Contains("Rack", WSNames);
            Assert.Contains("Netwerkelement", WSNames);
            workbook.Close();
            excel.Quit();
        }

        [Fact]
        public void TemplateExportTest_3_types_WithChecklistOptions()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);
            var path_save_to = "test_result.xlsx";

            // act
            subsetImporter.Import();
            var exporter = new SubsetExporterXLS();
            exporter.SetOTLSubset(subsetImporter.GetOTLObjectTypes());
            bool success = exporter.Export(path: Directory.GetCurrentDirectory() + "\\" + path_save_to, help: false, checklistoptions: true);

            // assert
            var excel = new Application { Visible = false, DisplayAlerts = false };
            var workbook = excel.Workbooks.Open(Directory.GetCurrentDirectory() + "\\" + path_save_to);
            List<string> WSNames = new List<string>();
            foreach (Worksheet ws in workbook.Worksheets)
            {
                WSNames.Add(ws.Name);
            }
            // check sheet names
            Assert.True(success);
            Assert.DoesNotContain("Sheet1", WSNames);
            Assert.Contains("dropdownvalues", WSNames);
            Assert.Contains("Netwerkpoort", WSNames);
            Assert.Contains("Rack", WSNames);
            Assert.Contains("Netwerkelement", WSNames);
            // tear down
            workbook.Close();
            excel.Quit();
        }
    }
}