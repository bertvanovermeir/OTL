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
        private Dictionary<string,OTL_Entity> loadedEntities;

        public RealDataImporter()
        {
            entities = new List<OTL_Entity>();
            loadedEntities = new Dictionary<string,OTL_Entity>();
        }

        public bool Import(string path, Enums.ImportType type)
        {
            var returnvalue = false;

            switch (type)
            {
                case Enums.ImportType.CSV:
                    returnvalue = ImportCSV(path);
                    break;
                case Enums.ImportType.JSON:
                    returnvalue = false;
                    break;
                case Enums.ImportType.XLSX:
                    returnvalue = false;
                    break;
                case Enums.ImportType.XLS:
                    returnvalue = false;
                    break;
                default:
                    returnvalue=false;
                    break;
            }
            

            return returnvalue;
        }

        private bool ImportCSV(string path)
        {
            // check if separator is ; or , by trial and error
            var temp = readCSV(path, ';');
            if (temp[0].Length < 2)
            {
                temp = readCSV(path, ',');
                if (temp[0].Length < 2)
                {
                    return false;
                }
                else
                {
                    return processCSVData(temp);
                }
            }
            else
            {
                return processCSVData(temp);
            }
        }

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
            
            // first line is the identifier line for specific OTL data. It is presumed this will not change, but just
            // in case we use the settings file from the other part of the program
            var id = Settings.Get("otlidentifier");
            var uri = Settings.Get("otlclassuri");
            data[0] = data[0].Select(s => s.ToLowerInvariant()).ToArray();
            var idindex = Array.IndexOf(data[0], id);
            var uriindex = Array.IndexOf(data[0], uri);
            var header = data[0];
            data.RemoveAt(0);

            foreach (var line in data)
            {
                OTL_Entity entity = new OTL_Entity();
                entity.AssetId = line[idindex];
                entity.TypeUri = line[uriindex];
                entity.Name = line[uriindex].Split('/').Last();
                // properties
                for (int i = 0; i < line.Length; i++)
                {
                    entity.Properties.Add(header[i], line[i]);
                }
                // check if exists
                if(loadedEntities.ContainsKey(entity.AssetId))
                {
                     // it is a double entry
                } else
                {
                    entities.Add(entity);
                    loadedEntities.Add(entity.AssetId, entity);
                }
                
            }
            return true;
        }

        public List<OTL_Entity> GetEntities()
        {
            return entities;
        }

        public void ResetEntities()
        {
            entities = new List<OTL_Entity>();
        }

        public OTL_Entity GetEntity(string assetid)
        {
            return entities.Where(w => w.AssetId.Equals(assetid)).FirstOrDefault();
        }
    }

    
}
