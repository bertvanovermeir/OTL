using CsvHelper;
using CsvHelper.Configuration;
using OTLWizard.OTLObjecten;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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

        // TODO
        public bool Export(string path, bool withDescriptions, bool withChecklistOptions)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ";",
                    SanitizeForInjection = false,
                };
                using (var writer = new StreamWriter(path))
                using (var csv = new CsvWriter(writer, config))
                {

                    csv.WriteRecords(OTL_ObjectTypes);
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
