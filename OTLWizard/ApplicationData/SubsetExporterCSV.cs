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
    public class SubsetExporterCSV
    {

        private List<OTL_ObjectType> OTL_ObjectTypes;
        private string[] classes;


        public SubsetExporterCSV()
        {

        }

        public bool SetOTLSubset(List<OTL_ObjectType> OTL_ObjectTypes)
        {
            if (OTL_ObjectTypes == null)
            {
                return false;
            }
            else if (OTL_ObjectTypes.Count == 0)
            {
                return false;
            }
            else
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
            if (classes == null)
            {
                this.classes = null;
                return true;
            }
            else if (classes.Length == 0)
            {
                return false;
            }
            else
            {
                this.classes = classes;
                return true;
            }
        }


        public bool Export(string path, bool help)
        {
            // construct matrix
            var matrix = new List<string[]>();

            foreach (OTL_ObjectType otlklasse in OTL_ObjectTypes)
            {
                var otlnaam = classes.ToList().FirstOrDefault(o => o == otlklasse.otlName);
                if (otlnaam == null)
                    continue;
                var filename = path.Substring(0,path.LastIndexOf('.')) + "_" + otlnaam + ".csv";
                WriteCSV(filename, matrix, ';');
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
