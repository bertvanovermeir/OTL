﻿using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.ApplicationData
{
    public static class ParameterHelper
    {
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

        public static List<string> GetDropDownValues(string DataTypeString, string KeuzelijstenPad)
        {
            if (DataTypeString.Contains("XMLSchema#") || DataTypeString.Contains("rdf-schema#") || DataTypeString.Contains("generiek#Getal") || DataTypeString.Contains("#Dte"))
            {
                string temp = DataTypeString.Split('#')[1];
                if(temp.Equals("Boolean"))
                {
                    return new List<string> { "- ", "True", "False" };
                }
            }
            else if (DataTypeString.Contains("#Kl"))
            {
                //webrequest needed here or internal files
                var DropdownValues = new List<string> { "-" };
                // query the files in attachment.
                // find the correct file in folder
                string filename = DataTypeString.Split('#')[1] + ".ttl";
                if (KeuzelijstenPad != null)
                {
                    if (File.Exists(KeuzelijstenPad + "\\" + filename))
                    {
                        string[] lines = File.ReadAllLines(KeuzelijstenPad + "\\" + filename, System.Text.Encoding.UTF8);
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
