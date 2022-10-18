using CsvHelper;
using CsvHelper.Configuration;
using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.ApplicationData
{
    public class RealDataImporter
    {
        private List<OTL_Entity> entities;
        private Dictionary<string,OTL_Entity> OTL_Entities;
        private List<OTL_Relationship> OTL_Relationships;
        private Dictionary<string,OTL_Relationship> OTL_RelationshipsDictionary;
        private SubsetImporter subsetImporter;

        public RealDataImporter()
        {
            entities = new List<OTL_Entity>();
            OTL_Entities = new Dictionary<string,OTL_Entity>();
            OTL_Relationships = new List<OTL_Relationship>();
            OTL_RelationshipsDictionary = new Dictionary<string, OTL_Relationship>();
        }

        public void SetRelationTypeData(SubsetImporter s)
        {
            subsetImporter = s;
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

        public void CheckAssetCompliance()
        {
            // now do a final check, for all relations that do not have entities
            // create user defined asset

            foreach (var rel in OTL_Relationships)
            {
                var bron = entities.Where(ent => rel.bronID == ent.AssetId).ToList().Select(x => x.AssetId).FirstOrDefault();
                if (bron == null)
                    CreateUserAsset(rel.bronID);
                var doel = entities.Where(ent => rel.doelID == ent.AssetId).ToList().Select(x => x.AssetId).FirstOrDefault();
                if (doel == null)
                    CreateUserAsset(rel.doelID);
            }
        }

        // CSV
        private void ImportCSV(string path)
        {
            // check if separator is ; or , by trial and error
            var temp = readCSV(path, ';');
            if(temp.Columns.Count < 2)
            {
                temp = readCSV(path, ',');
            }
            // try again
            if(temp.Columns.Count < 2)
            {
                throw new Exception(Language.Get("csvinvalid"));
            } else
            {
                processCSVData(temp);
            }           
        }

        private DataTable readCSV(string path, char separator)
        {
            List<string> badRecord = new List<string>();
            var dt = new DataTable();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = separator.ToString(),
                BadDataFound = context => badRecord.Add(context.RawRecord)
            };
            using (var reader = new StreamReader(path))

                using (var csv = new CsvReader(reader, config))
                {
                    // Do any configuration to `CsvReader` before creating CsvDataReader.
                    using (var dr = new CsvDataReader(csv))
                    {                        
                        dt.Load(dr);
                    }
                }

            if(badRecord.Count > 0)
            {
                throw new Exception(Language.Get("csvinvalid3"));
            }

            return dt;
        }

        private void processCSVData(DataTable data)
        {           
            // first line is the identifier line for specific OTL data. It is presumed this will not change, but just
            // in case we use the settings file from the other part of the program
            var id = Settings.Get("otlidentifier").ToLower();
            var uri = Settings.Get("otlclassuri").ToLower();
            var src = Settings.Get("otlsrcrel").ToLower();
            var trgt = Settings.Get("otltrgtrel").ToLower();
            var act = Settings.Get("otlactiverel").ToLower();

            // either add to entites or relations, depending on content of cells
            // we do not (in an easy way) know which columns belong to which object. Empty rows will be omitted to "simulate" wise behaviour.
            // use OTL_Entities and OTL_Relationships

            data.CaseSensitive = false;
            var idindex = data.Columns.IndexOf(id);
            var uriindex = data.Columns.IndexOf(uri);
            var relsrcindex = data.Columns.IndexOf(src);
            var reltrgtindex = data.Columns.IndexOf(trgt);
            var activeindex = data.Columns.IndexOf(act);

            foreach (DataRow line in data.Rows)
            {
                if(line.ItemArray[idindex].Equals("") | line.ItemArray[uriindex].Equals("")) {
                    // invalid CSV, do not process
                    throw new Exception(Language.Get("csvinvalid2"));
                } else
                {
                    // check if relationship columns exist in file
                    var relvalue = "";
                    if (relsrcindex > -1 & reltrgtindex > -1)
                    {
                        relvalue = (string) line.ItemArray[relsrcindex];
                    }
                    if(relvalue.Equals(""))
                    {
                        // entity
                        OTL_Entity entity = new OTL_Entity();
                        entity.AssetId = (string)line.ItemArray[idindex];
                        entity.TypeUri = (string)line.ItemArray[uriindex];
                        entity.Name = (string)line.ItemArray[uriindex];
                        entity.Name = entity.Name.Split('#').Last();
                        entity.DisplayName = entity.AssetId + " | " + entity.Name;

                        // properties
                        for (int i = 0; i < line.ItemArray.Length; i++)
                        {
                            var key = data.Columns[i].ColumnName.ToLower();
                            var value = (string)line.ItemArray[i];
                            if (!value.Equals(""))
                            {
                                entity.Properties.Add(key, value);
                            }
                        }

                        // check if exists
                        if (OTL_Entities.ContainsKey(entity.AssetId))
                        {
                            // it is a double entry
                        }
                        else
                        {
                            entities.Add(entity);
                            OTL_Entities.Add(entity.AssetId, entity);
                        }
                    } else
                    {
                        // relation
                        var temp = new OTL_Relationship();
                        temp.AssetId = (string) line.ItemArray[idindex];
                        temp.doelID = (string) line.ItemArray[reltrgtindex];
                        temp.bronID = (string)line.ItemArray[relsrcindex];
                        temp.relationshipURI = (string) line.ItemArray[uriindex];
                        // some relationship CSV's might not contain isActief column
                        if(activeindex == -1)
                        {
                            temp.Activated = true;
                        } else
                        {
                            var actstring = line.ItemArray[activeindex];
                            if(actstring.Equals(""))
                            {
                                temp.Activated = true;
                            } else
                            {
                                temp.Activated = Boolean.Parse((string)actstring);
                            }
                        }
                        
                        // properties
                        for (int i = 0; i < line.ItemArray.Length; i++)
                        {
                            var key = data.Columns[i].ColumnName.ToLower();
                            var value = (string)line.ItemArray[i];
                            if (!value.Equals(""))
                            {
                                temp.Properties.Add(key, value);
                            }
                        }
                        // is it directional?
                        var reldir = subsetImporter.GetOTLRelationshipTypes().Where(x=>x.relationshipURI.ToLower().Equals(temp.relationshipURI.ToLower())).FirstOrDefault();                       
                        if(reldir != null)
                            temp.isDirectional = reldir.isDirectional; 
                        else
                            temp.isDirectional = false;
                        temp.GenerateDisplayName();

                        // check if exists
                        if (OTL_RelationshipsDictionary.ContainsKey(temp.AssetId))
                        {
                            // it is a double entry
                        }
                        else
                        {
                            OTL_Relationships.Add(temp);
                            OTL_RelationshipsDictionary.Add(temp.AssetId, temp);
                        }
                    }                 
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

        public List<OTL_Relationship> GetRelationships()
        {
            return OTL_Relationships;
        }

        public void SetRelationships(List<OTL_Relationship> relationships)
        {
            OTL_RelationshipsDictionary = new Dictionary<string, OTL_Relationship>();
            OTL_Relationships = relationships;
            foreach (OTL_Relationship rel in relationships)
            {
                OTL_RelationshipsDictionary.Add(rel.AssetId, rel);
            }
        }

        public void AddRelationship(OTL_Relationship rel)
        {
            OTL_Relationships.Add(rel);
            OTL_RelationshipsDictionary.Add(rel.AssetId,rel);
        }

        public void RemoveRelationship(string relID, bool softremove)
        {
            var rel = OTL_Relationships.Where(x => x.AssetId.Equals(relID)).FirstOrDefault();
            // remove anyway to clean the list objects
            OTL_Relationships.Remove(OTL_Relationships.Where(x => x.AssetId.Equals(relID)).FirstOrDefault());
            OTL_RelationshipsDictionary.Remove(relID);

            if (softremove)
            {
                rel.Activated = false;
                rel.GenerateDisplayName();
                AddRelationship(rel);
                
            } else
            {
                // that is it, do not re-add the relationship
            }         
        }

        public void SetEntities(List<OTL_Entity> ent)
        {
            OTL_Entities = new Dictionary<string, OTL_Entity>();
            entities = ent;
            foreach(OTL_Entity entity in entities)
            {
                OTL_Entities.Add(entity.AssetId, entity);
            }
        }

        public void AddEntity(OTL_Entity e)
        {
            entities.Add(e);
            OTL_Entities.Add(e.AssetId, e);
        }

        public void CreateUserAsset(string doelID)
        {
            OTL_Entity e2 = new OTL_Entity();
            e2.AssetId = doelID;
            e2.Name = Language.Get("userdefinedasset");
            e2.TypeUri = "";
            e2.DisplayName = doelID + " | " + Language.Get("userdefinedasset");
            e2.Properties.Add(Settings.Get("otlidentifier"), doelID);
            e2.Properties.Add(Settings.Get("otlclassuri"), Language.Get("userdefinedasset"));
            AddEntity(e2);
        }
    }   
}
