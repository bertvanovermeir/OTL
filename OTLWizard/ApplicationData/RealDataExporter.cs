﻿using CsvHelper;
using CsvHelper.Configuration;
using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OTLWizard.Helpers
{
    public static class RealDataExporter
    {

        public static bool Export(string path, List<OTL_Relationship> relations)
        {
            // dictionary containing parameter name and values (per column - empty when not a value for this relationship)
            var matrix = new Dictionary<string, List<string>>();
            var uniqueParameterNames = new Dictionary<string, string>();

            // first get all possible parameter Names
            foreach(OTL_Relationship rel in relations)
            {
                foreach(string name in rel.Properties.Keys)
                {
                    if(!uniqueParameterNames.ContainsKey(name))
                        uniqueParameterNames.Add(name, name);
                }
            }
            // add header to matrix
            var header = new List<string>();
            foreach(string name in uniqueParameterNames.Keys)
            {
                matrix.Add(name, new List<string>());
            }        

            // fill the matrix add empty value if it does not exist for the relationship
            foreach(OTL_Relationship rel in relations)
            {
                var parameters = rel.Properties;
                foreach(string headerName in matrix.Keys)
                {
                    if(parameters.ContainsKey(headerName))
                    {
                        matrix[headerName].Add(parameters[headerName]);
                    } else
                    {
                        matrix[headerName].Add("");
                    }
                }
            }
            // matrix is created, now transform it for export
            var realMatrix = new List<string[]>();
            var matrixHeader = new string[matrix.Count];
            var count = 0;
            foreach(string headerData  in matrix.Keys)
            {
                matrixHeader[count] = headerData;
                count++;
            }
            realMatrix.Add(matrixHeader);
            // get info for all relationships per column and header
            var rowLength = matrix.First().Value.Count;
            for (int i = 0; i < rowLength; i++)
            {
                var row = new List<string>();
                foreach (KeyValuePair<string, List<string>> column in matrix)
                {
                    var valueList = column.Value.ToArray();
                    var valueSingle = valueList[i];
                    row.Add(valueSingle);
                }
                realMatrix.Add((row.ToArray()));
            }
            
            try
            {
                using (var w = new StreamWriter(path))
                {
                    foreach (string[] row in realMatrix)
                    {
                        w.WriteLine(string.Join(";", row));
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ExportSDF(string savepath, List<OTL_Entity> entities, string newpath)
        {
            if (!File.Exists(newpath))
                return false;
            if (!newpath.ToLower().EndsWith(".sdf"))
                return false;

            // start process of sdf saving
            var tempLocation = System.IO.Path.GetTempPath() + "otldavieworkaround\\";
            if(Directory.Exists(tempLocation))
                Directory.Delete(tempLocation, true);
            Directory.CreateDirectory(tempLocation);
            // get schema name
            var schemaName = SDFHandler.GetSchemaName(newpath);
            schemaName = schemaName.TrimEnd('\r','\n');
            // dump schema to file
            SDFHandler.DumpSchemaFromFile(schemaName, newpath, tempLocation + "schema.xml");
            // replace xs:decimal with xs:string
            var text = File.ReadAllText(tempLocation + "schema.xml");
            text = text.Replace("xs:decimal", "xs:string");
            File.WriteAllText(tempLocation + "schema.xml", text);
            // create new sdf file
            SDFHandler.CreateNewFile(savepath, tempLocation + "schema.xml");
            // Get all classes in file
            var classes = SDFHandler.GetClasses(newpath);
            // get assetids for entities
            List<string> assetIds = new List<string>();
            foreach(var entity in entities)
            {
                assetIds.Add(entity.AssetId);
            }
            // copy filter per class to new file
            foreach(var cla in classes)
            {
                SDFHandler.CopyClass(newpath, savepath, cla, assetIds.ToArray(), schemaName, schemaName);
            }
            return true;
        }

        public static bool ExportCSV(string path, List<OTL_Entity> entities, bool emptyColumns, bool featid)
        {
            // first divide records into classes otherwise header is wrong
            var typeuris = new List<string>();
            var typeurisDistinct = new List<string>();

            foreach (var entity in entities)
            {
                typeuris.Add(entity.TypeUri);
            }
            typeurisDistinct.AddRange(typeuris.Distinct());
            
            // now write to csv per class

            foreach (var typeuri in typeurisDistinct)
            {
                var hasHeaderBeenWritten = false;
                var tempEntities = entities.Where(e => e.TypeUri.Equals(typeuri)).ToList();
                var tempPath = path.ToLower().Replace(".csv", "") + typeuri.Split('#')[1] + ".csv";

                // also define the maximum amount of attributes per typeuri by using a dictionary
                var attributes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var entity in tempEntities)
                {
                    foreach (var property in entity.GetProperties())
                    {
                        if (!attributes.ContainsKey(property.Key))
                            attributes.Add(property.Key, "");
                    }

                }

                if(!featid)
                    attributes.Remove("featid");

                // remove empty columns when asked
                if (!emptyColumns)
                {
                    var tempAttributes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    foreach(var attribute in attributes)
                    {
                        var result = tempEntities.Where(t => (t.GetProperties().ContainsKey(attribute.Key) && !t.GetProperties()[attribute.Key].Equals("")));
                        if(result.Any())
                            tempAttributes.Add(attribute.Key, "");
                    }
                    attributes = tempAttributes;
                }
                // now sort the attribute keys
                // sort the properties
                var keysSorted = attributes.Keys.ToList();
                keysSorted.Sort();

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ";",
                    SanitizeForInjection = false,
                };
                using (var writer = new StreamWriter(tempPath))
                using (var csv = new CsvWriter(writer, config))
                {
                    foreach (var row in tempEntities)
                    {
                        if (!hasHeaderBeenWritten)
                        {
                            foreach (var key in keysSorted)
                            {
                                // workaround Geometry Hoofdletter
                                var hoofding = key;
                                if (key.Equals("Geometry"))
                                    hoofding = "geometry";
                                csv.WriteField(hoofding);
                            }

                            hasHeaderBeenWritten = true;

                            csv.NextRecord();
                        }

                        foreach (var key in keysSorted)
                        {
                            if(row.GetProperties().ContainsKey(key))
                                csv.WriteField(row.GetProperties()[key]);
                            else
                                csv.WriteField("");
                        }

                        csv.NextRecord();
                    }
                }
            }
            return true;
        }
    }
}