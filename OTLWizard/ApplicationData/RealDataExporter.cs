using CsvHelper;
using CsvHelper.Configuration;
using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.ApplicationData
{
    public class RealDataExporter
    {

        public static bool Export(string path, List<OTL_Relationship> relations)
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

                    csv.WriteRecords(relations);
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

   
