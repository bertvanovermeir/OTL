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

        public void Import(string path, Enums.ImportType type)
        {
            switch (type)
            {
                case Enums.ImportType.CSV:
                    ImportCSV(path);
                    break;
                case Enums.ImportType.JSON:
                    ImportJSON(path);
                    break;
                case Enums.ImportType.XLSX:
                    ImportXLS(path);
                    break;
                case Enums.ImportType.XLS:
                    ImportXLS(path);
                    break;
                default:
                    throw new Exception(Language.Get("filenotsupported"));
            }
        }

        // CSV
        private void ImportCSV(string path)
        {
            // check if separator is ; or , by trial and error
            var temp = readCSV(path, ';');
            if (temp[0].Length < 2)
            {
                temp = readCSV(path, ',');
                if (temp[0].Length < 2)
                {
                    throw new Exception(Language.Get("csvinvalid"));
                }
                else if (Array.ConvertAll(temp[0], d => d.ToLower()).Contains("doelassetid.identificator"))
                {
                    throw new Exception(Language.Get("csvinvalid"));
                } else
                {
                    processCSVData(temp);
                }
            }
            else
            {
                processCSVData(temp);
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

        private void processCSVData(List<string[]> data)
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
                entity.Name = line[uriindex].Split('#').Last();
                entity.DisplayName = entity.AssetId + " | " + entity.Name;
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
        }

//XLS(X)

        private void ImportXLS(string path)
        {
            throw new Exception(Language.Get("filenotsupported"));
        }

        //JSON

        private void ImportJSON(string path)
        {
            throw new Exception(Language.Get("filenotsupported"));
        }


        // GENERAL

        public List<OTL_Entity> GetEntities()
        {
            entities = entities.OrderBy(o => o.TypeUri).ToList();
            return entities;
        }

        public void SetEntities(List<OTL_Entity> ent)
        {
            loadedEntities = new Dictionary<string, OTL_Entity>();
            entities = ent;
            foreach(OTL_Entity entity in entities)
            {
                loadedEntities.Add(entity.AssetId, entity);
            }
        }

        public void AddEntity(OTL_Entity e)
        {
            entities.Add(e);
        }

    }   
}
