using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using OTLWizard.Helpers;

namespace OTLWizard.Helpers
{
    /// <summary>
    /// Deze klasse omschrijft de connectie met XLS. Ze maakt een XLS template aan.
    /// </summary>
    public class TemplateExporter
    {
        private List<OTL_ObjectType> OTL_ObjectTypes;
        private string[] classes;
        private string path;
        private bool help;
        private bool checklistoptions;

        public TemplateExporter()
        {
        }

        /// <summary>
        /// set the complete OTL subset that the exporter needs to use.
        /// </summary>
        /// <param name="OTL_ObjectTypes"></param>
        /// <returns>FALSE if something went wrong with the subset</returns>
        public bool SetOTLSubset(List<OTL_ObjectType> OTL_ObjectTypes)
        {
            if(OTL_ObjectTypes == null)
            {
                return false;
            } else if(OTL_ObjectTypes.Count == 0) {
                return false;
            } else
            {
                this.OTL_ObjectTypes = OTL_ObjectTypes;
                return true;
            }          
        }

        /// <summary>
        /// Set the user selection of a certain subset
        /// </summary>
        /// <param name="classes"></param>
        /// <returns>FALSE if user selection is invalid</returns>
        public bool SetSelectedClassesByUser(string[] classes)
        {
            if(classes == null)
            {
                this.classes = null;
                return true;
            } else if(classes.Length == 0)
            {
                return false;
            } else
            {
                this.classes = classes;
                return true;
            }          
        }

        public bool ExportXls(string path, bool help, bool checklistoptions)
        {
            this.path = path;
            this.help = help;
            this.checklistoptions = checklistoptions;
            return run();
        }

        private void processClass(Workbook workbook, OTL_ObjectType temp)
        {

            // set the starting value (if help enabled first row will be descriptions (wordwrap)
            int start = 1;
            if (help)
            {
                start = 2;
            }

            Worksheet sheet = newWorkSheet(workbook, temp.otlName);
            // extract all necessary data from the otl table (parameters)
            for (int i = 0; i < temp.GetParameters().Count; i++)
            {
                OTL_Parameter p = temp.GetParameters()[i];
                // main column name
                sheet.Cells[start, i + 1] = p.DotNotatie;
                // now set the help messages
                if (help)
                {
                    sheet.Cells[1, i + 1] = p.Description;
                    sheet.Range[sheet.Cells[1, i + 1], sheet.Cells[1, i + 1]].EntireRow.WrapText = true;
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
                // set default value
                sheet.Cells[start + 1, i + 1] = p.DefaultValue;
            }
            sheet.Columns.AutoFit();
            sheet.Range["A1:F1"].EntireRow.Interior.Color = System.Drawing.Color.LightGray;
            sheet.Range["A1:Z1"].BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, (Microsoft.Office.Interop.Excel.XlColorIndex)3, ColorTranslator.ToOle(Color.Black));

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
            excel = new Application
            {
                Visible = false,
                DisplayAlerts = false
            };
            workbook = excel.Workbooks.Add(Type.Missing);
            // create an empty worksheet for tables, if dropdownlists is true
            if(checklistoptions)
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
                        Console.WriteLine("the object " + cla + "  is not available in the database and thus ignored.\n");
                    }
                }
            }
            // remove the Sheet1 default worksheet           
            Worksheet rem = workbook.Worksheets["Sheet1"];
            rem.Delete();
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
            if(sheetName.Length > 31)
            {
                xlNewSheet.Name = sheetName.Substring(0,30);
                Console.WriteLine("Worksheet name exceeds maximum: " + sheetName + ". Will use: " + sheetName.Substring(0, 30));
            } else
            {
                xlNewSheet.Name = sheetName;
            }
            
            xlNewSheet.Cells.Font.Size = 12;
            return xlNewSheet;
        }
    }
}
