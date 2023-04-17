using Microsoft.Office.Interop.Excel;
using OTLWizard.Helpers;
using OTLWizard.Helpers;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace UnitTests
{
    public class SubsetExportTests
    {
        [Fact]
        public void SubsetExportTest_3_types_CSV()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);
            var path_save_to = "test_result.csv";

            // act
            subsetImporter.Import();
            var exporter = new SubsetExporterCSV();
            exporter.SetOTLSubset(subsetImporter.GetOTLObjectTypes());
            exporter.SetSelectedClassesByUser(null);
            bool success = exporter.Export(path: "./../../" + path_save_to, null, 1, help: false, dummydata: false, wkt: false, deprecated: false);

            // assert
            // 3 CSVs will be created check if they exist
            Assert.True(success);
            Assert.True(File.Exists("./../../" + "test_result_Netwerkpoort.csv"));
            Assert.True(File.Exists("./../../" + "test_result_Rack.csv"));
            Assert.True(File.Exists("./../../" + "test_result_Netwerkelement.csv"));
            // read lines in CSV and check contents
            var linesCSV = ReadCSV("./../../" + "test_result_Rack.csv", 1, ';');
            Assert.Single(linesCSV);
            Assert.Equal("assetId.identificator", linesCSV[0][0]);
            Assert.Equal("assetId.toegekendDoor", linesCSV[0][1]);
            Assert.Equal("typeURI", linesCSV[0][2]);
        }

        [Fact]
        public void SubsetExportTest_3_types_Help_CSV()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);
            var path_save_to = "test_result_help.csv";

            // act
            subsetImporter.Import();
            var exporter = new SubsetExporterCSV();
            exporter.SetOTLSubset(subsetImporter.GetOTLObjectTypes());
            exporter.SetSelectedClassesByUser(new[] { "Rack", "Netwerkpoort", "Netwerkelement" });
            bool success = exporter.Export(path: "./../../" + path_save_to, null, 1, help: true, dummydata: false, wkt: false, deprecated: false);

            // assert
            // 3 CSVs will be created check if they exist
            Assert.True(success);
            Assert.True(File.Exists("./../../" + "test_result_help_Netwerkpoort.csv"));
            Assert.True(File.Exists("./../../" + "test_result_help_Rack.csv"));
            Assert.True(File.Exists("./../../" + "test_result_help_Netwerkelement.csv"));
            // read lines in CSV and check contents
            var linesCSV = ReadCSV("./../../" + "test_result_help_Rack.csv", 1, ';');
            Assert.Equal(2, linesCSV.Count);
            Assert.Equal("Een groep van tekens om een AIM object te identificeren of te benoemen.", linesCSV[0][0]);
            Assert.Equal("Gegevens van de organisatie die de toekenning deed.", linesCSV[0][1]);
            Assert.Equal("De URI van het object volgens https://www.w3.org/2001/XMLSchema#anyURI .", linesCSV[0][2]);
            Assert.Equal("assetId.identificator", linesCSV[1][0]);
            Assert.Equal("assetId.toegekendDoor", linesCSV[1][1]);
            Assert.Equal("typeURI", linesCSV[1][2]);
        }

        /// <summary>
        /// input = CSV file, output = ExpandoObject List with "lines" amount of records
        /// </summary>
        /// <param name="path"></param>
        /// 7<param name="lines"></param>
        private List<string[]> ReadCSV(string path, int numLines, char separator)
        {
            var lines = new List<string[]>();
            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                    lines.Add(sr.ReadLine().Split(separator));
            }
            return lines;
        }




        [Fact]
        public void SubsetExportTest_3_types_NoChecklistOptions_XLS()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);
            var path_save_to = "test_result.xlsx";

            // act
            subsetImporter.Import();
            var exporter = new SubsetExporterXLS();
            exporter.SetOTLSubset(subsetImporter.GetOTLObjectTypes());
            bool success = exporter.Export(path: Directory.GetCurrentDirectory() + "\\" + path_save_to, null, 1, help: false, checklistoptions: false, dummydata: false, wkt: false, deprecated: false);

            // assert
            var excel = new Application { Visible = false, DisplayAlerts = false };
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
        public void SubsetExportTest_3_types_WithChecklistOptions_XLS()
        {
            // arrange           
            var otldbpath = "./../../subset_3_types_netwerk.db";
            var subsetImporter = new SubsetImporter(otldbpath);
            var path_save_to = "test_result.xlsx";

            // act
            subsetImporter.Import();
            var exporter = new SubsetExporterXLS();
            exporter.SetOTLSubset(subsetImporter.GetOTLObjectTypes());
            bool success = exporter.Export(path: Directory.GetCurrentDirectory() + "\\" + path_save_to, null, 1, help: false, checklistoptions: true, dummydata: false, wkt: false, deprecated: false);

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

        [Fact]
        public void SubsetExportTest_Aansluitmof_boolean()
        {
            // arrange           
            var otldbpath = "./../../subset_aansluitmof.db";
            var subsetImporter = new SubsetImporter(otldbpath);
            var path_save_to = "test_result.xlsx";

            // act
            subsetImporter.Import();
            var exporter = new SubsetExporterXLS();
            exporter.SetOTLSubset(subsetImporter.GetOTLObjectTypes());
            bool success = exporter.Export(path: Directory.GetCurrentDirectory() + "\\" + path_save_to, null, 1, help: false, checklistoptions: true, dummydata: false, wkt: false, deprecated: false);

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
            Assert.Contains("Aansluitmof", WSNames);
            Worksheet sheet = (Worksheet)workbook.Worksheets["Aansluitmof"];
            var range = sheet.Range[sheet.Cells[2, 4], sheet.Cells[2, 4]]; ;
            var cell = range.Cells;
            Assert.True(cell.Validation.InCellDropdown);
            Assert.Equal("- ;True;False;", cell.Validation.Formula1);
            // tear down
            workbook.Close();
            excel.Quit();
        }
    }
}