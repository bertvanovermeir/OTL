using Microsoft.Office.Interop.Excel;
using OTLWizard.ApplicationData;
using System;
using System.Collections.Generic;

namespace OTLWizard.OTLObjecten
{
    public class ArtefactExporterXLS : ArtefactExporter
    {
        public ArtefactExporterXLS()
        {
        }

        public override bool Export(string path, List<OTL_ArtefactType> artefacten)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = false;
            excel.DisplayAlerts = false;
            workbook = excel.Workbooks.Add(Type.Missing);
            // create an empty worksheet for tables
            Worksheet sheet = newWorkSheet(workbook, "Rapport");
            // hoofding
            sheet.Cells[1, 1] = "Naam OTL Object";
            sheet.Cells[1, 2] = "Verwacht Geometrietype";
            sheet.Cells[1, 3] = "Meten of Overerven";
            sheet.Cells[1, 4] = "Meetcriterium";
            sheet.Cells[1, 5] = "Overervingsgrens (in m)";
            sheet.Cells[1, 6] = "Meten volgens Steekkaart(en)";
            sheet.Cells[1, 7] = "Overerven van";
            sheet.Cells[1, 8] = "Overerven via Relatie";
            sheet.Cells[1, 9] = "Uitzonderingen";
            sheet.Cells[1, 10] = "Overervingsklasse in Subset";

            // invulling
            int i = 2;
            foreach (OTL_ArtefactType artefact in artefacten)
            {
                sheet.Cells[i, 1] = artefact.objectnaam;
                sheet.Cells[i, 2] = artefact.geometrie;
                sheet.Cells[i, 3] = artefact.overerving;
                sheet.Cells[i, 4] = artefact.meetcriterium;
                sheet.Cells[i, 5] = artefact.overervingsgrens;
                sheet.Cells[i, 6] = artefact.steekkaarten;
                sheet.Cells[i, 7] = artefact.overervenvan;
                sheet.Cells[i, 8] = artefact.viarelatie;
                sheet.Cells[i, 9] = artefact.uitzonderingen;
                sheet.Cells[i, 10] = artefact.opmerkingen;
                i++;
            }

            sheet.Columns.AutoFit();
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
            // excel shenanigans
            if (sheetName.Length > 31)
            {
                xlNewSheet.Name = sheetName.Substring(0, 30);
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
