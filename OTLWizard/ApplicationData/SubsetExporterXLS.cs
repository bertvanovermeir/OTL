﻿using Microsoft.Office.Interop.Excel;
using OTLWizard.Helpers;
using OTLWizard.Helpers;
using System;
using System.Collections.Generic;

namespace OTLWizard.Helpers
{
    /// <summary>
    /// Deze klasse omschrijft de connectie met XLS. Ze maakt een XLS template aan.
    /// </summary>
    public class SubsetExporterXLS : SubsetExporter
    {
        private string path;
        private bool help;
        private bool dummydata;
        private bool checklistoptions;
        private bool wkt;
        private bool deprecated;
        private List<OTL_ArtefactType> oTL_ArtefactTypes;
        private int amountExamples;

        public SubsetExporterXLS()
        { }

        public override bool Export(string path, List<OTL_ArtefactType> oTL_ArtefactTypes, int amountExamples, bool help, bool checklistoptions, bool dummydata, bool wkt, bool deprecated)
        {
            this.dummydata = dummydata;
            this.path = path;
            this.deprecated = deprecated;
            this.help = help;
            this.wkt = wkt;
            this.checklistoptions = checklistoptions;
            this.oTL_ArtefactTypes = oTL_ArtefactTypes;
            this.amountExamples = amountExamples;
            bool temp = run();
            return temp;
        }

        private void processClass(Workbook workbook, OTL_ObjectType temp)
        {

            // set the starting value (if help enabled first row will be descriptions (wordwrap)
            int start = 1;
            if (help)
            {
                start = 3;
            }
            if (dummydata)
            {
                DummyDataHandler.initRandom(oTL_ArtefactTypes);
            }
            if (wkt)
            {
                temp.AddParameter(new OTL_Parameter(false,
                    "geometry", "geometry",
                    "De geometrische representatie van het OTL object beschreven in een WKT-string.",
                    "WKT", false));
            }
            Worksheet sheet = newWorkSheet(workbook, temp.otlName);
            // extract all necessary data from the otl table (parameters)
            for (int i = 0; i < temp.GetParameters().Count; i++)
            {
                OTL_Parameter p = temp.GetParameters()[i];
                // deprecated check
                var dotnotatie = p.DotNotatie;
                if (p.Deprecated && deprecated)
                {
                    dotnotatie = "[DEPRECATED]" + dotnotatie;
                }
                // main column name
                sheet.Cells[start, i + 1] = dotnotatie;
                // now set the help messages
                if (help)
                {
                    sheet.Cells[2, i + 1].Formula = "=HYPERLINK(\"" + p.DataTypeString + "\")";
                    sheet.Cells[1, i + 1] = p.Description;
                    sheet.Range[sheet.Cells[1, i + 1], sheet.Cells[1, i + 1]].EntireRow.WrapText = true;
                    sheet.Range["A2:X2"].EntireRow.Interior.Color = System.Drawing.Color.LightGray;
                }
                // set default value
                sheet.Cells[start + 1, i + 1] = p.DefaultValue;
                // overwrite if dummydata
                if (dummydata)
                {
                    for (int j = 1; j < amountExamples + 1; j++)
                    {
                        sheet.Cells[start + j, i + 1] = DummyDataHandler.GetDummyValue(p, temp);
                    }
                }
                // now fill in dropdowns if they are applicable
                if (p.DataType == Enums.DataType.List && checklistoptions)
                {
                    string xlslist = "";
                    foreach (string item in p.DropdownValues)
                    {
                        xlslist = xlslist + item + ";";
                    }
                    xlslist = xlslist.Trim();
                    if (xlslist.Length > 253)
                    {
                        // character limit reached, create a table dropdown space
                        xlslist = newDropDownList(workbook, p.DropdownValues);
                    }
                    try
                    {
                        sheet.Range[sheet.Cells[start + 1, i + 1], sheet.Cells[200, i + 1]].Cells.Validation.Delete();
                    }
                    catch
                    {

                    }
                    sheet.Range[sheet.Cells[start + 1, i + 1], sheet.Cells[200, i + 1]].Cells.Interior.Color = System.Drawing.Color.LightGreen;
                    sheet.Range[sheet.Cells[start + 1, i + 1], sheet.Cells[200, i + 1]].Cells.Validation.Add(XlDVType.xlValidateList, XlDVAlertStyle.xlValidAlertInformation, XlFormatConditionOperator.xlEqual, xlslist, Type.Missing);


                }
            }
            // finish
            sheet.Columns.AutoFit();
            sheet.Range["A1:X1"].EntireRow.Interior.Color = System.Drawing.Color.LightGray;
            // sheet.Range["A1:Z1"].BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, (Microsoft.Office.Interop.Excel.XlColorIndex)3, ColorTranslator.ToOle(Color.Black));

        }

        private string newDropDownList(Workbook workbook, List<string> values)
        {
            Worksheet sheet = (Worksheet)workbook.Worksheets["dropdownvalues"];
            // first empty row
            int newRow = sheet.Range["A200000"].End[Microsoft.Office.Interop.Excel.XlDirection.xlUp].Row;
            for (int i = 0; i < values.Count; i++)
            {
                sheet.Cells[i + newRow + 1, 1] = values[i];
            }
            string temp = "=dropdownvalues!$A$" + (newRow + 1) + ":$A$" + (newRow + values.Count);
            sheet.Cells.NumberFormat = "@"; // numbers to text for dropdown, looks better
            return temp;

        }

        private bool run()
        {
            // first we need to check how many entries we have, create a new workbook per 
            Application excel;
            Workbook workbook;
            // attempt at warning user
            try
            {
                excel = new Application
                {
                    Visible = false,
                    DisplayAlerts = false
                };
                workbook = excel.Workbooks.Add(Type.Missing);
            }
            catch
            {
                return false;
            }
            // create an empty worksheet for tables, if dropdownlists is true
            if (checklistoptions)
            {
                newWorkSheet(workbook, "dropdownvalues");
            }
            // now check the classes the user selected, it should not be empty if it is empty, do all!
            if (classes == null)
            {
                foreach (OTL_ObjectType type in OTL_ObjectTypes)
                {
                    processClass(workbook, type);
                }
            }
            else
            {
                foreach (string cla in classes)
                {
                    // check for valid, existing string in the OTL database, if not ignore and print console
                    bool found = false;
                    OTL_ObjectType temp = new OTL_ObjectType();
                    foreach (OTL_ObjectType otl in OTL_ObjectTypes)
                    {
                        if (otl.otlName.ToLower().Equals(cla.ToLower()))
                        {
                            found = true;
                            temp = otl;
                        }
                    }

                    if (found)
                    {
                        processClass(workbook, temp);
                    }
                    else
                    {
                        // object "cla" is unavailable in the database.
                    }
                }
            }
            // remove the Sheet1 default worksheet
            try
            {
                // remove the last sheet, which is the default one in any language
                Worksheet rem = workbook.Worksheets[workbook.Worksheets.Count];
                // Worksheet rem = workbook.Worksheets["Sheet1"]; (only for english)
                rem.Delete();
            }
            catch
            {
                // could not remove the sheet, this might happen on some computers (fix issue #13)
                // removing is always a risky operation, that is why try-catch is not a luxury in this case.
            }
            // save file
            try
            {
                workbook.SaveAs(path);
                workbook.Close();
                excel.Quit();
                return true;
            }
            catch
            {
                workbook.Close();
                excel.Quit();
                return false;
            }
        }

        private Worksheet newWorkSheet(Microsoft.Office.Interop.Excel.Workbook workbook, string sheetName)
        {
            var xlSheets = workbook.Sheets as Microsoft.Office.Interop.Excel.Sheets;
            var xlNewSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
            // ecel shenanigans
            if (sheetName.Length > 31)
            {
                xlNewSheet.Name = sheetName.Substring(0, 30);
                Console.WriteLine("Worksheet name exceeds maximum: " + sheetName + ". Will use: " + sheetName.Substring(0, 30));
            }
            else
            {
                xlNewSheet.Name = sheetName;
            }

            xlNewSheet.Cells.Font.Size = 12;
            return xlNewSheet;
        }
    }
}
