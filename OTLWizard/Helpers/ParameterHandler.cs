using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.Helpers
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
                string temp = DataTypeString.Split('#')[1];
                switch (temp)
                {
                    case "Getal":
                        DataType = Enums.DataType.Real;
                        break;
                    case "Integer":
                        DataType = Enums.DataType.Integer;
                        break;
                    case "Decimal":
                        DataType = Enums.DataType.Real;
                        break;
                    case "DateTime":
                        DataType = Enums.DataType.Text;
                        break;
                    case "Date":
                        DataType = Enums.DataType.Text;
                        break;
                    case "Time":
                        DataType = Enums.DataType.Text;
                        break;
                    case "String":
                        DataType = Enums.DataType.Text;
                        break;
                    case "Boolean":
                        DataType = Enums.DataType.List;
                        break;
                    case "Literal":
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
                string temp = DataTypeString.Split('#')[1];
                if (temp.Equals("Boolean"))
                {
                    return new List<string> { "- ", "True", "False" };
                }
            }
            else if (DataTypeString.Contains("#Kl"))
            {
                var DropdownValues = new List<string> { "-" };
                // find the correct file
                if(Keuzelijsten)
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
                            client.DownloadFile("https://raw.githubusercontent.com/Informatievlaanderen/OSLOthema-wegenenverkeer/master/codelijsten/" + filename, localPath + filename);
                        }
                    }
                    catch
                    {
                        // download mislukt, de keuzelijst bestaat waarschijnlijk niet.
                    }

                    if (File.Exists(localPath + filename))
                    {
                        string[] lines = File.ReadAllLines(localPath + filename, System.Text.Encoding.UTF8);
                        foreach (string item in lines)
                        {
                            if (item.Contains("skos:Concept;"))
                            {
                                string listText = item.Split('>')[0];
                                string sublistText = listText.Split('/')[listText.Split('/').Length - 1];
                                DropdownValues.Add(sublistText);
                            }
                        }
                    }
                }              
                return DropdownValues;
            }
            return null;
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
                string temp = DataTypeString.Split('#')[1];
                switch (temp)
                {
                    case "Getal":
                        DefaultValue = -99999.99d;
                        break;
                    case "Integer":
                        DefaultValue = 99999;
                        break;
                    case "Decimal":
                        DefaultValue = -99999.99d;
                        break;
                    case "DateTime":
                        break;
                    case "Date":
                        break;
                    case "Time":
                        break;
                    case "String":
                        DefaultValue = "-";
                        break;
                    case "Boolean":
                        DefaultValue = "-";
                        break;
                    case "Literal":
                        DefaultValue = -99999.99d;
                        break;
                    default:
                        DefaultValue = "-";
                        break;
                }
            }
            // kwantWaarde
            else if (DataTypeString.Contains("#KwantWrdIn"))
            {
                DefaultValue = -99999.99d;
            }
            // Enums TTL (lists in acad)
            else if (DataTypeString.Contains("#Kl"))
            {
                DefaultValue = "-";
            }
            else
            {
                DefaultValue = "-";
            }
            return DefaultValue;
        }
    }

}
