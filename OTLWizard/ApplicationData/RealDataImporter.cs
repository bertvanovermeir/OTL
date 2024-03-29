﻿using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Office.Interop.Excel;
using OTLWizard.Helpers;
using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OTLWizard.Helpers
{
    public class RealDataImporter
    {
        private List<OTL_Entity> entities;
        private Dictionary<string, OTL_Entity> OTL_Entities;
        private Dictionary<string, OTL_GeometryEntity> OTL_GeometryEntities;

        private List<OTL_Relationship> OTL_Relationships;
        private Dictionary<string, OTL_Relationship> OTL_RelationshipsDictionary;
        private SubsetImporter subsetImporter;
        private List<ErrorContainer> errors;
        private string currentFileName;

        public RealDataImporter()
        {
            entities = new List<OTL_Entity>();
            OTL_Entities = new Dictionary<string, OTL_Entity>();
            OTL_GeometryEntities = new Dictionary<string, OTL_GeometryEntity>();
            OTL_Relationships = new List<OTL_Relationship>();
            OTL_RelationshipsDictionary = new Dictionary<string, OTL_Relationship>();
            errors = new List<ErrorContainer>();
        }

        public OTL_GeometryEntity GetGeometryEntityByAssetId(string AssetId)
        {
            if (OTL_GeometryEntities.ContainsKey(AssetId))
                return OTL_GeometryEntities[AssetId];
            else
                return null;
        }

        public List<OTL_GeometryEntity> GetGeometryEntities()
        {
            return OTL_GeometryEntities.Values.ToList();
        }

        public void SetAllGeometryEntitiesBackground(bool resetSourceTargetState)
        {
            if(resetSourceTargetState)
            {
                foreach (OTL_GeometryEntity geo in OTL_GeometryEntities.Values)
                {
                    if(!geo.IsBackgroundAsset())
                    geo.SetAsBackGroundAsset();
                }
            } else
            {
                foreach (OTL_GeometryEntity geo in OTL_GeometryEntities.Values)
                {
                    if(!geo.IsSourceAsset() && !geo.IsBackgroundAsset())
                        geo.SetAsBackGroundAsset();
                }
            }           
        }

        public void SetRelationTypeData(SubsetImporter s)
        {
            subsetImporter = s;
        }

        public List<ErrorContainer> GetErrors()
        {
            return errors;
        }
        public void ResetErrors()
        {
            errors = new List<ErrorContainer>();
        }

        public void Import(string path, Enums.ImportType type)
        {
            currentFileName = GetFileName(path);
            switch (type)
            {
                case Enums.ImportType.CSV:
                    ImportCSV(path);
                    break;
                case Enums.ImportType.XLS:
                    ImportXLS(path);
                    break;
                case Enums.ImportType.SDF:
                    ImportSDF(path);
                    break;
                default:
                    errors.Add(new ErrorContainer(currentFileName, "/", "Error", Language.Get("filenotsupported")));
                    break;
            }
        }

        private string GetFileName(string path)
        {
            var filename = "";
            try
            {
                filename = path.Split('\\').Last();
            }
            catch
            {
                filename = path;
            }
            return filename;
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

        //SDF
        private void ImportSDF(string path)
        {
            List<string> files = new List<string>();
            // convert SDF to CSV
            if (SDFHandler.checkDependencies())
            {
                files = SDFHandler.GenerateCSVExportFiles(path);

                // import the converted data from CSV
                foreach (string file in files)
                {
                    ImportCSV(file);
                }
            }
            else
            {
                errors.Add(new ErrorContainer(currentFileName, "/", "Error", Language.Get("dependencymissing")));
            }
        }

        // CSV
        private void ImportCSV(string path)
        {
            currentFileName = GetFileName(path);
            // check if separator is ; or , by trial and error
            // check if first line contains , or ; and use method for reading defined by that
            // Read a text file line by line.  
            char separator = '/';
            string[] lines = File.ReadAllLines(path);
            if (lines != null)
            {
                if (lines[0].Contains(","))
                {
                    separator = ',';
                }
                else if (lines[0].Contains(";"))
                {
                    separator = ';';
                }
                else if (lines[0].Contains("\t"))
                {
                    separator = '\t';
                }
                else
                {
                    // not a valid file
                }
            }
            else
            {
                // not a valid file
            }
            if (separator != '/')
            {
                var temp = readCSV(path, separator);

                if (temp != null && temp.Columns.Count > 1)
                {
                    processCSVData(temp);
                }
                else
                {
                    errors.Add(new ErrorContainer(currentFileName, "/", "Error", Language.Get("foutpos2")));
                }
            }
            else
            {
                errors.Add(new ErrorContainer(currentFileName, "/", "Error", Language.Get("foutpos1")));
            }
        }

        public void initGeometryAssetsForBingMaps()
        {
            int count = 0;
            foreach (OTL_Entity entity in entities)
            {
                var temp = new OTL_GeometryEntity(entity);
                temp.SetAsBackGroundAsset();
                temp.SetSerialNumber(count);
                temp.generateLocationPointers();
                temp.generateGeometryPointers();
                count++;
                if(temp.GetBingLocationString() != null)
                    OTL_GeometryEntities.Add(entity.AssetId,temp );
            }
        }

        private System.Data.DataTable readCSV(string path, char separator)
        {
            List<string> badRecord = new List<string>();
            var dt = new System.Data.DataTable();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = separator.ToString(),
                Quote = '"',
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

            if (badRecord.Count > 0)
            {
                foreach (var record in badRecord)
                {
                    errors.Add(new ErrorContainer(currentFileName, string.Join(string.Empty, record.ToArray()), "Bad Record", Language.Get("foutpos8")));
                }
                return null;
            }
            // remove empty rows from datatable
            RemoveEmptyRows(dt);
            return dt;
        }

        private void RemoveEmptyRows(System.Data.DataTable source)
        {
            for (int i = source.Rows.Count; i >= 1; i--)
            {
                bool found = false;
                DataRow currentRow = source.Rows[i - 1];
                foreach (var colValue in currentRow.ItemArray)
                {
                    if (!string.IsNullOrEmpty(colValue.ToString())) { found = true; break; }
                }
                if (!found)
                    source.Rows[i - 1].Delete();
            }
            source.AcceptChanges();
        }

        private void processCSVData(System.Data.DataTable data)
        {
            // first line is the identifier line for specific OTL data. It is presumed this will not change, but just
            // in case we use the settings file from the other part of the program
            var id = Settings.Get("otlidentifier").ToLower();
            var agentid = Settings.Get("otlidentifieragent").ToLower();
            var uri = Settings.Get("otlclassuri").ToLower();
            var src = Settings.Get("otlsrcrel").ToLower();
            var trgt = Settings.Get("otltrgtrel").ToLower();
            var act = Settings.Get("otlactiverel").ToLower();
            var agenturi = Settings.Get("agenturi").ToLower();

            // either add to entites or relations, depending on content of cells
            // we do not (in an easy way) know which columns belong to which object. Empty rows will be omitted to "simulate" wise behaviour.
            // use OTL_Entities and OTL_Relationships

            data.CaseSensitive = false;
            var idindex = data.Columns.IndexOf(id);
            var uriindex = data.Columns.IndexOf(uri);
            var relsrcindex = data.Columns.IndexOf(src);
            var reltrgtindex = data.Columns.IndexOf(trgt);
            var activeindex = data.Columns.IndexOf(act);
            var agentidindex = data.Columns.IndexOf(agentid);

            foreach (DataRow line in data.Rows)
            {
                var isagent = false;
                if (agentidindex > -1)
                {
                    // we have an agent object, this is special...
                    // it does not have an URI, and has an agentid.identificator
                    // set idindex as agentidindex
                    idindex = agentidindex;
                    uriindex = agentidindex;
                    isagent = true;
                }
                if (idindex < 0 | uriindex < 0)
                {
                    // these parameters were not found in the CSV file, invalid file
                    errors.Add(new ErrorContainer(currentFileName, data.Rows.IndexOf(line).ToString(), "Error", Language.Get("foutpos3")));
                }
                else if (line.ItemArray[idindex].Equals("") | line.ItemArray[uriindex].Equals(""))
                {
                    // assetid or asseturi not filled in
                    errors.Add(new ErrorContainer(currentFileName, data.Rows.IndexOf(line).ToString(), "Error", Language.Get("foutpos4")));
                }
                else
                {
                    // check if relationship columns exist in file
                    var relvalue = "";
                    if (relsrcindex > -1 & reltrgtindex > -1)
                    {
                        relvalue = (string)line.ItemArray[relsrcindex];
                    }
                    if (relvalue.Equals(""))
                    {
                        // entity
                        OTL_Entity entity = new OTL_Entity();
                        entity.AssetId = (string)line.ItemArray[idindex];
                        entity.TypeUri = (string)line.ItemArray[uriindex];
                        entity.Name = (string)line.ItemArray[uriindex];
                        entity.Name = entity.Name.Split('#').Last();
                        if (isagent)
                        {
                            entity.TypeUri = agenturi;
                            entity.Name = agenturi.Split('/').Last();
                        }

                        // properties, if they are empty. Ignore them
                        for (int i = 0; i < line.ItemArray.Length; i++)
                        {
                            var key = data.Columns[i].ColumnName;
                            var value = (string)line.ItemArray[i];
                            if (!value.Equals(""))
                            {
                                entity.GetProperties().Add(key, value);
                            }
                        }
                        entity.GenerateDisplayName();

                        // calculate WKT to GPS coordinates and save for faster processing.
                        var maplocationlisting = OTLUtils.GenerateMapLocation(entity);
                        int iter = 0;
                        foreach (var loc in maplocationlisting)
                        {   // for serialization we add same
                            entity.GlobalWKT.Add(iter.ToString(), loc);
                            iter++;
                        }
                        // check if exists
                        if (OTL_Entities.ContainsKey(entity.AssetId))
                        {
                            // check if "extern asset"
                            if (OTL_Entities[entity.AssetId].Name.Equals(Language.Get("userdefinedasset")))
                            {
                                // it is a double entity but currently an external asset, convert to real asset.
                                OTL_Entities[entity.AssetId] = entity;
                                entities.RemoveAll(e => e.AssetId.Equals(entity.AssetId));
                                entities.Add(entity);
                                errors.Add(new ErrorContainer(currentFileName, data.Rows.IndexOf(line).ToString(), "Warning", Language.Get("foutpos6")));
                            }
                            else
                            {
                                errors.Add(new ErrorContainer(currentFileName, data.Rows.IndexOf(line).ToString(), "Warning", Language.Get("foutpos5")));
                            }
                        }
                        else
                        {
                            entities.Add(entity);
                            OTL_Entities.Add(entity.AssetId, entity);
                        }
                    }
                    else
                    {
                        // relation
                        var temp = new OTL_Relationship();
                        temp.AssetId = (string)line.ItemArray[idindex];
                        temp.doelID = (string)line.ItemArray[reltrgtindex];
                        temp.bronID = (string)line.ItemArray[relsrcindex];
                        temp.relationshipURI = (string)line.ItemArray[uriindex];
                        // some relationship CSV's might not contain isActief column
                        if (activeindex == -1)
                        {
                            temp.isActive = true;
                        }
                        else
                        {
                            var actstring = line.ItemArray[activeindex];
                            if (actstring.Equals(""))
                            {
                                temp.isActive = true;
                            }
                            else
                            {
                                temp.isActive = Boolean.Parse((string)actstring);
                            }
                        }

                        // properties
                        for (int i = 0; i < line.ItemArray.Length; i++)
                        {
                            //var key = data.Columns[i].ColumnName.ToLower();
                            var key = data.Columns[i].ColumnName;
                            var value = (string)line.ItemArray[i];
                            if (!value.Equals(""))
                            {
                                temp.Properties.Add(key, value);
                            }
                        }
                        // is it directional?
                        var reldir = subsetImporter.GetOTLRelationshipTypes().Where(x => x.relationshipURI.ToLower().Equals(temp.relationshipURI.ToLower())).FirstOrDefault();
                        if (reldir != null)
                            temp.isDirectional = reldir.isDirectional;
                        else
                            temp.isDirectional = false;
                        temp.GenerateDisplayName();

                        // check if exists
                        if (OTL_RelationshipsDictionary.ContainsKey(temp.AssetId))
                        {
                            // it is a double entry
                            errors.Add(new ErrorContainer(currentFileName, data.Rows.IndexOf(line).ToString(), "Warning", Language.Get("foutpos7")));

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
            List<string> files = new List<string>();

            // convert XLSX to CSV and then read it.
            // first we need to check how many entries we have, create a new workbook per            
            try
            {
                Application excel;
                Workbook workbook;
                // attempt at warning user
                excel = new Application
                {
                    Visible = false,
                    DisplayAlerts = false
                };
                excel.Application.DecimalSeparator = ".";
                excel.Application.ThousandsSeparator = "";
                excel.Application.UseSystemSeparators = false;
                workbook = excel.Workbooks.Open(path);

                foreach (Worksheet item in workbook.Worksheets)
                {

                    item.Activate();
                    // dowload the subset from the website to a temp folder
                    string filename = item.Name + ".csv";
                    var localPath = System.IO.Path.GetTempPath() + "otltempconversion\\";
                    // create the folder if it does not exist
                    Directory.CreateDirectory(localPath);

                    workbook.SaveAs(localPath + filename, XlFileFormat.xlCSVWindows);
                    files.Add(localPath + filename);
                }

                workbook.Close();
                excel.Application.UseSystemSeparators = true;
            }
            catch
            {
                //
            }

            foreach (string file in files)
            {
                ImportCSV(file);
            }

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
            // update the displaynames of the relations according to current settings
            foreach (OTL_Relationship rel in relationships)
            {
                rel.GenerateDisplayName();
            }

            // now set data to dbs
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
            OTL_RelationshipsDictionary.Add(rel.AssetId, rel);
        }

        public void RemoveRelationship(string relID, bool softremove)
        {
            var rel = OTL_Relationships.Where(x => x.AssetId.Equals(relID)).FirstOrDefault();
            // remove anyway to clean the list objects
            OTL_Relationships.Remove(OTL_Relationships.Where(x => x.AssetId.Equals(relID)).FirstOrDefault());
            OTL_RelationshipsDictionary.Remove(relID);

            if (softremove)
            {
                rel.isActive = false;
                rel.GenerateDisplayName();
                AddRelationship(rel);

            }
            else
            {
                // that is it, do not re-add the relationship
            }
        }

        public void ActivateRelationship(string relID)
        {
            var rel = OTL_Relationships.Where(x => x.AssetId.Equals(relID)).FirstOrDefault();
            // remove anyway to clean the list objects
            OTL_Relationships.Remove(OTL_Relationships.Where(x => x.AssetId.Equals(relID)).FirstOrDefault());
            OTL_RelationshipsDictionary.Remove(relID);
            rel.isActive = true;
            rel.GenerateDisplayName();
            AddRelationship(rel);
        }

        public void SetEntities(List<OTL_Entity> ent)
        {
            // reset displaynames according to latest settings.
            foreach (OTL_Entity e in ent)
            {
                // set displayname
                e.GenerateDisplayName();
                // for each entity, calculate WKT string in case of legacy files
                // first reset the map location
                e.GlobalWKT = new SerializableDictionary<string, string>();
                var maplocationlisting = OTLUtils.GenerateMapLocation(e);
                int iter = 0;
                foreach (var loc in maplocationlisting)
                {   // for serialization we add same
                    e.GlobalWKT.Add(iter.ToString(), loc);
                    iter++;
                }

            }
            OTL_Entities = new Dictionary<string, OTL_Entity>();
            entities = ent;
            foreach (OTL_Entity entity in entities)
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
            e2.GenerateDisplayName();
            e2.GetProperties().Add(Settings.Get("otlidentifier"), doelID);
            e2.GetProperties().Add(Settings.Get("otlclassuri"), Language.Get("userdefinedasset"));
            AddEntity(e2);
        }
    }
}
