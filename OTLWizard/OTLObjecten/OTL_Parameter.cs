using System;
using System.Collections.Generic;

namespace OTLWizard
{
    /// <summary>
    /// Deze klasse omschrijft één parameter en de optionele invulwaarden. De klasse is zeer afhankelijk van de manier waarop de SQL uitgelezen wordt en dient 
    /// daarom altijd samen met de OTL DatabaseConnector gelezen te worden.
    /// Room for improvement in simplificatie.
    /// (klasse is exporteerbaar via Serialize)
    /// </summary>
    [Serializable]
    public class OTL_Parameter
    {
        public string dotNotationName; // complete dot notation name
        public string friendlyName; // friendly name of the parameter
        public string description; // description of the parameter
        public List<string> dropdownValues; // a list of dropdownvalues.
        public Object defaultValue; // the default value used for the parameter, calculated at runtime.
        public string dataTypeString; // string containing name of datatype for parsing.
        public Enums.DataType dataType; // real acad datatype calculated at runtime.
        
        public OTL_Parameter()
        {
            // for serialization
        }

        public OTL_Parameter(List<object> ParameterNames, List<object> cardinality, List<object> description, List<object> dataType)
        {
            // parse parameter description
            ParseParameterDescription(description);
            // parse parameter dotName and friendlyname
            ParseParameterNames(ParameterNames, cardinality);
            // parse parameter datatype
            ParseParameterDataType(dataType);         
            // initialize the empty dropdownvalue modifier.
            dropdownValues = new List<string>();         
        }

        /// <summary>
        /// parse data type of the parameter
        /// </summary>
        private void ParseParameterDataType(List<object>DataTypeList)
        {
            for (int j = 0; j < DataTypeList.Count; j++)
            {
                if (DataTypeList[j].GetType() != (System.DBNull.Value.GetType()) && DataTypeList[j] != null)
                    dataTypeString = (string)DataTypeList[j];
            }
        }

        /// <summary>
        /// parse parameter names with brute force
        /// </summary>
        private void ParseParameterNames(List<object> ParameterNames, List<object> cardinality)
        {
            string temp = "";
            // iterate over all parameternames
            for (int i = 0; i < ParameterNames.Count; i++)
            {
                if (ParameterNames[i].GetType() != (System.DBNull.Value.GetType()))
                {
                    string par = (string)ParameterNames[i];
                    string extra = "";
                    if (!cardinality[i].Equals("1"))
                        extra = "[]";

                    temp = temp + "." + par + extra;
                    // assign last entry to friendlyname
                    friendlyName = (string)ParameterNames[i];
                }
            }
            // assign temp to dotnotation
            dotNotationName = temp.Substring(1, temp.Length - 1);
        }


        /// <summary>
        /// parse parameter, repeat until the description for final entry is known (brute force)
        /// </summary>
        private void ParseParameterDescription(List<object> DescriptionList)
        {
            for (int j = 0; j < DescriptionList.Count; j++)
            {
                if (DescriptionList[j].GetType() != (System.DBNull.Value.GetType()) && DescriptionList[j] != null)
                {
                    string destemp = (string)DescriptionList[j];
                    if (destemp.Length > 254)
                        this.description = destemp.Substring(0, 254);
                    else
                        this.description = destemp;
                }

            }
        }
    }
}
