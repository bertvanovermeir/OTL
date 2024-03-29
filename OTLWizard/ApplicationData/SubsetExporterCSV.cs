﻿using OTLWizard.Helpers;
using OTLWizard.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OTLWizard.Helpers
{
    public class SubsetExporterCSV : SubsetExporter
    {
        public SubsetExporterCSV()
        { }

        public override bool Export(string path, List<OTL_ArtefactType> oTL_ArtefactTypes, int amountExamples, bool help, bool dummydata, bool wkt, bool deprecated, bool checklistoptions = false)
        {
            foreach (OTL_ObjectType otlklasse in OTL_ObjectTypes)
            {
                var matrix = new List<string[]>();
                var otlnaam = classes.ToList().FirstOrDefault(o => o == otlklasse.otlName);
                if (otlnaam == null)
                    continue;
                var filename = path.Substring(0, path.LastIndexOf('.')) + "_" + otlnaam + ".csv";
                // fill the matrix
                if (wkt)
                {
                    otlklasse.AddParameter(new OTL_Parameter(false,
                        "geometry", "geometry",
                        "De geometrische representatie van het OTL object beschreven in een WKT-string.",
                        "WKT", false));
                }
                if (deprecated)
                {
                    foreach (OTL_Parameter o in otlklasse.GetParameters())
                    {
                        if (o.Deprecated)
                        {
                            o.DotNotatie = "[DEPRECATED]" + o.DotNotatie;
                        }
                    }
                }

                string[] dotnotaties = otlklasse.GetParameters().Select(y => y.DotNotatie).ToArray();
                if (help)
                    matrix.Add(otlklasse.GetParameters().Select(z => z.Description).ToArray());
                matrix.Add(dotnotaties);
                if (dummydata)
                {
                    DummyDataHandler.initRandom(oTL_ArtefactTypes);
                    for (int i = 0; i < amountExamples; i++)
                    {
                        matrix.Add(otlklasse.GetParameters().Select(y => DummyDataHandler.GetDummyValue(y, otlklasse)).ToArray());
                    }
                }
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
