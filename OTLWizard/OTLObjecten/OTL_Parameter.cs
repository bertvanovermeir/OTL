using System;
using System.Collections.Generic;
using System.IO;

namespace OTLWizard.OTLObjecten
{
    /// <summary>
    /// Deze klasse omschrijft één parameter en de optionele invulwaarden. 
    /// (klasse is exporteerbaar via Serialize)
    /// </summary>
    [Serializable]
    public class OTL_Parameter
    {
        // parameters are public for eventual serialization
        public string dotNotatie; // complete dot notation name
        public string friendlyName; // friendly name of the parameter
        public string description; // description of the parameter
        public List<string> dropdownValues; // a list of dropdownvalues.
        public Object defaultValue; // the default value used for the parameter, calculated at runtime.
        public bool deprecated; // is the parameter still in use
        public Enums.DataType dataType; // real acad datatype calculated at runtime.   

        private string dataTypeString; // string containing name of datatype for parsing.            
        private string klPath; // pad keuzelijsten
        
        public OTL_Parameter()
        {
            // for serialization
        }

        /// <summary>
        /// maakt een nieuwe parameter aan
        /// </summary>
        /// <param name="dotNotatie"></param>
        /// <param name="friendlyName"></param>
        /// <param name="description"></param>
        /// <param name="dataTypeString"></param>
        /// <param name="deprecated"></param>
        public OTL_Parameter(string klPath, string dotNotatie, string friendlyName, string description, string dataTypeString, bool deprecated)
        {
            dropdownValues = new List<string>();
            this.dotNotatie = dotNotatie;
            this.friendlyName = friendlyName;
            this.description = description;
            this.dataTypeString = dataTypeString;
            this.deprecated = deprecated;
            this.klPath = klPath;
            ParseDataType();
        }

        /// <summary>
        /// parse each parameter for data type, to define default value and list if necessary
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private void ParseDataType()
        {            
            if (dataTypeString.Contains("XMLSchema#") || dataTypeString.Contains("rdf-schema#") || dataTypeString.Contains("generiek#Getal") || dataTypeString.Contains("#Dte"))
            {
                string temp = dataTypeString.Split('#')[1];
                switch (temp)
                {
                    case "Getal":
                        dataType = Enums.DataType.Real;
                        defaultValue = -99999.99d;
                        break;
                    case "Integer":
                        dataType = Enums.DataType.Integer;
                        defaultValue = 99999;
                        break;
                    case "Decimal":
                        dataType = Enums.DataType.Real;
                        defaultValue = -99999.99d;
                        break;
                    case "DateTime":
                        dataType = Enums.DataType.Text;
                        break;
                    case "Date":
                        dataType = Enums.DataType.Text;
                        break;
                    case "Time":
                        dataType = Enums.DataType.Text;
                        break;
                    case "String":
                        dataType = Enums.DataType.Text;
                        defaultValue = "-";
                        break;
                    case "Boolean":
                        dataType = Enums.DataType.List;
                        dropdownValues = new List<string> { "- ", "True", "False" };
                        defaultValue = "-";
                        break;
                    case "Literal":
                        dataType = Enums.DataType.Real;
                        defaultValue = -99999.99d;
                        break;
                    default:
                        dataType = Enums.DataType.Text;
                        defaultValue = "-";
                        break;
                }
            }
            // kwantWaarde
            else if (dataTypeString.Contains("#KwantWrdIn"))
            {
                dataType = Enums.DataType.Real;
                defaultValue = -99999.99d;
            }
            // Enums TTL (lists in acad)
            else if (dataTypeString.Contains("#Kl"))
            {
                //webrequest needed here or internal files
                dataType = Enums.DataType.List;
                dropdownValues = new List<string> { "-" };
                // query the files in attachment.
                // find the correct file in folder
                string filename = dataTypeString.Split('#')[1] + ".ttl";
                if (File.Exists(klPath + "\\" + filename))
                {
                    string[] lines = File.ReadAllLines(klPath + "\\" + filename, System.Text.Encoding.UTF8);
                    foreach (string item in lines)
                    {
                        if (item.Contains("skos:Concept;"))
                        {
                            string listText = item.Split('>')[0];
                            string sublistText = listText.Split('/')[listText.Split('/').Length - 1];
                            dropdownValues.Add(sublistText);
                        }
                    }
                }
                defaultValue = "-";
            }
            else
            {
                dataType = Enums.DataType.Text;
                defaultValue = "-";
            }
        }
    }
}
