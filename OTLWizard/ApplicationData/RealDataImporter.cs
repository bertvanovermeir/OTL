using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.ApplicationData
{
    public class RealDataImporter
    {
        private List<OTL_Entity> entities;

        private List<string[]> readCSV(string path, char separator)
        {
            var lines = new List<string[]>();
            using (var sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                    lines.Add(sr.ReadLine().Split(separator));
            }
            return lines;
        }

        private bool processCSVData(List<string[]> data)
        {
            entities = new List<OTL_Entity>();
            // first line is the identifier line for specific OTL data. It is presumed this will not change, but just
            // in case we use the settings file from the other part of the program
            var id = Settings.Get("otlidentifier");
            var uri = Settings.Get("otlclassuri");
            data[0] = data[0].Select(s => s.ToLowerInvariant()).ToArray();
            var idindex = Array.IndexOf(data[0], id);
            var uriindex = Array.IndexOf(data[0], uri);
            data.RemoveAt(0);

            foreach (var line in data)
            {
               OTL_Entity entity = new OTL_Entity();
                entity.AssetId = line[idindex];
                entity.TypeUri = line[uriindex];
                entity.Name = line[uriindex].Split('/').Last();
                // properties do no interest us
                entities.Add(entity);
            }
            return true;
        }


        public bool ImportCSV(string path)
        {
            // check if separator is ; or , by trial and error
            var temp = readCSV(path, ';');
            if (temp[0].Length < 2)
            {
                temp = readCSV(path, ',');
                if(temp[0].Length < 2)
                {
                    return false;
                } else
                {
                    return processCSVData(temp);
                }
            } else
            {
                return processCSVData(temp);
            }
        }
      

        public void ImportXLSX()
        {
            // not yet supported
        }

        public void ImportJSON()
        {
            // not yet supported
        }

        public List<OTL_Entity> GetEntities()
        {
            return entities;
        }
    }

    
}
