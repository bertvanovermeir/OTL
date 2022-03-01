using CsvHelper;
using CsvHelper.Configuration;
using OTLWizard.OTLObjecten;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OTLWizard.ApplicationData
{
    public class SubsetExporterCSV : SubsetExporter
    {
        public SubsetExporterCSV()
        {}

        public override bool Export(string path, bool help, bool checklistoptions = false)
        {
            foreach (OTL_ObjectType otlklasse in OTL_ObjectTypes)
            {
                var matrix = new List<string[]>();
                var otlnaam = classes.ToList().FirstOrDefault(o => o == otlklasse.otlName);
                if (otlnaam == null)
                    continue;
                var filename = path.Substring(0, path.LastIndexOf('.')) + "_" + otlnaam + ".csv";
                // fill the matrix               
                string[] dotnotaties = otlklasse.GetParameters().Select(y => y.DotNotatie).ToArray();
                if (help)
                    matrix.Add(otlklasse.GetParameters().Select(z => z.Description).ToArray());
                matrix.Add(dotnotaties);
                // write to file
                var success = WriteCSV(filename, matrix, ';');
                if (!success)
                    return false;                   
            }
            return true;
        }


        private bool WriteCSV(string pathForSingleFile, List<string[]> matrix, char separator)
        {
            try
            {
                using (var w = new StreamWriter(pathForSingleFile))
                {
                    foreach (string[] row in matrix)
                    {
                        w.WriteLine(string.Join(separator.ToString(), row));
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
