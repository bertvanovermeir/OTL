using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using static OTLWizard.OTLObjecten.Enums;

namespace OTLWizard.OTLObjecten
{
    /// <summary>
    /// convenience classes to help parse some parameter values
    /// </summary>
    public static class ParameterHandler
    {
        /// <summary>
        /// retrieve the (enum) datatype
        /// </summary>
        /// <param name="DataTypeString"></param>
        /// <returns></returns>
        public static Enums.DataType GetDataType(string DataTypeString)
        {
            Enums.DataType DataType;

            if (DataTypeString.Contains("XMLSchema#") || DataTypeString.Contains("rdf-schema#") || DataTypeString.Contains("generiek#Getal") || DataTypeString.Contains("#Dte"))
            {
                string temp = DataTypeString.Split('#')[1].ToLower();
                switch (temp)
                {
                    case "getal":
                        DataType = Enums.DataType.Real;
                        break;
                    case "integer":
                        DataType = Enums.DataType.Integer;
                        break;
                    case "decimal":
                        DataType = Enums.DataType.Real;
                        break;
                    case "datetime":
                        DataType = Enums.DataType.Text;
                        break;
                    case "date":
                        DataType = Enums.DataType.Text;
                        break;
                    case "time":
                        DataType = Enums.DataType.Text;
                        break;
                    case "string":
                        DataType = Enums.DataType.Text;
                        break;
                    case "boolean":
                        DataType = Enums.DataType.List;
                        break;
                    case "literal":
                        DataType = Enums.DataType.Real;
                        break;
                    default:
                        DataType = Enums.DataType.Text;
                        break;
                }
            }
            else if (DataTypeString.Contains("#KwantWrdIn"))
            {
                DataType = Enums.DataType.Real;
            }
            else if (DataTypeString.Contains("#Kl"))
            {
                DataType = Enums.DataType.List;
            }
            else
            {
                DataType = Enums.DataType.Text;
            }
            return DataType;
        }

        /// <summary>
        /// retrieve the possible dropdownvalues, if applicable
        /// </summary>
        /// <param name="DataTypeString"></param>
        /// <param name="Keuzelijsten"></param>
        /// <returns>list with dropdownvalues or NULL if not a list or boolean</returns>
        public static List<string> GetDropDownValues(string DataTypeString, bool Keuzelijsten)
        {
            if (DataTypeString.Contains("XMLSchema#") || DataTypeString.Contains("rdf-schema#") || DataTypeString.Contains("generiek#Getal") || DataTypeString.Contains("#Dte"))
            {
                string temp = DataTypeString.Split('#')[1].ToLower();
                if (temp.Equals("boolean"))
                {
                    return new List<string> { "- ", "True", "False" };
                }
            }
            else if (DataTypeString.Contains("#Kl"))
            {
                var DropdownValues = new List<string> { "-" };
                // find the correct file
                if (Keuzelijsten)
                {
                    string filename = DataTypeString.Split('#')[1] + ".ttl";
                    var localPath = System.IO.Path.GetTempPath() + "codelijsten\\";
                    // create the folder if it does not exist
                    Directory.CreateDirectory(localPath);
                    // download the TTL file
                    try
                    {
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(Settings.Get("klpath") + filename, localPath + filename);
                        }
                    }
                    catch
                    {
                        // download mislukt, de keuzelijst bestaat waarschijnlijk niet.
                    }

                    if (File.Exists(localPath + filename))
                    {
                        string[] lines = File.ReadAllLines(localPath + filename, System.Text.Encoding.UTF8);
                        string sublistText = "";
                        foreach (string item in lines)
                        {
                            if (item.Contains("skos:Concept;"))
                            {
                                string listText = item.Split('>')[0];
                                sublistText = listText.Split('/')[listText.Split('/').Length - 1];
                                DropdownValues.Add(sublistText);
                            }
                            if (item.Contains("https://wegenenverkeer.data.vlaanderen.be/id/concept/KlAdmsStatus/uitgebruik"))
                            {
                                if (DropdownValues.Contains(sublistText))
                                    DropdownValues.Remove(sublistText);
                            }
                        }
                        DropdownValues.Sort();
                    }
                }
                return DropdownValues;
            }
            return null;
        }

        public static SDFAttributeTypes GetDataTypeSDF(string DataTypeString)
        {
            SDFAttributeTypes tempType;
            if (DataTypeString.Contains("XMLSchema#") || DataTypeString.Contains("rdf-schema#") || DataTypeString.Contains("generiek#Getal") || DataTypeString.Contains("#Dte"))
            {
                string temp = DataTypeString.Split('#')[1].ToLower();
                switch (temp)
                {
                    case "getal":
                        tempType = SDFAttributeTypes.Real;
                        break;
                    case "integer":
                        tempType = SDFAttributeTypes.Integer;
                        break;
                    case "decimal":
                        tempType = SDFAttributeTypes.Real;
                        break;
                    case "string":
                        tempType = SDFAttributeTypes.Simple;
                        break;
                    case "boolean":
                        tempType = SDFAttributeTypes.Bool;
                        break;
                    case "literal":
                        tempType = SDFAttributeTypes.Real;
                        break;
                    default:
                        tempType = SDFAttributeTypes.Simple;
                        break;
                }
            }
            // kwantWaarde
            else if (DataTypeString.Contains("#KwantWrdIn"))
            {
                tempType = SDFAttributeTypes.Real;
            }
            // Enums TTL (lists in acad)
            else if (DataTypeString.Contains("#Kl"))
            {
                tempType = SDFAttributeTypes.List;
            }
            else
            {
                tempType = SDFAttributeTypes.Simple;
            }

            return tempType;
        }

        /// <summary>
        /// retrieve the default value 
        /// </summary>
        /// <param name="DataTypeString"></param>
        /// <returns></returns>
        public static Object GetDefaultValue(string DataTypeString)
        {
            Object DefaultValue = null;

            if (DataTypeString.Contains("XMLSchema#") || DataTypeString.Contains("rdf-schema#") || DataTypeString.Contains("generiek#Getal") || DataTypeString.Contains("#Dte"))
            {
                string temp = DataTypeString.Split('#')[1].ToLower();
                switch (temp)
                {
                    case "getal":
                        DefaultValue = "";
                        break;
                    case "integer":
                        DefaultValue = "";
                        break;
                    case "decimal":
                        DefaultValue = "";
                        break;
                    case "datetime":
                        break;
                    case "date":
                        break;
                    case "time":
                        break;
                    case "string":
                        DefaultValue = "";
                        break;
                    case "boolean":
                        DefaultValue = "-";
                        break;
                    case "literal":
                        DefaultValue = "";
                        break;
                    default:
                        DefaultValue = "";
                        break;
                }
            }
            // kwantWaarde
            else if (DataTypeString.Contains("#KwantWrdIn"))
            {
                DefaultValue = "";
            }
            // Enums TTL (lists in acad)
            else if (DataTypeString.Contains("#Kl"))
            {
                DefaultValue = "-";
            }
            else
            {
                DefaultValue = "";
            }
            return DefaultValue;
        }
    }

}
