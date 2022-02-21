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
        public SubsetExporterCSV()
        {

        }

        public bool Export(string path, List<OTL_ObjectType> objects)
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

                    csv.WriteRecords(objects);
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
