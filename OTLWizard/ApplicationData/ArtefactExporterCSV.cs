﻿using CsvHelper;
using CsvHelper.Configuration;
using OTLWizard.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace OTLWizard.Helpers
{
    public class ArtefactExporterCSV : ArtefactExporter
    {
        public ArtefactExporterCSV()
        {

        }

        public override bool Export(string path, List<OTL_ArtefactType> artefacts)
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

                    csv.WriteRecords(artefacts);
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
