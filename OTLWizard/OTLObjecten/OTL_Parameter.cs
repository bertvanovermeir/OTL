using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace OTLWizard.Helpers
{
    /// <summary>
    /// Deze klasse omschrijft één parameter en de optionele invulwaarden. 
    /// (klasse is exporteerbaar via Serialize)
    /// </summary>
    [Serializable]
    public class OTL_Parameter
    {
        // parameters are public for eventual serialization
        public string dotNotatie { get; set; } // complete dot notation name
        public string friendlyName { get; set; } // friendly name of the parameter
        public string description { get; set; } // description of the parameter
        public List<string> dropdownValues { get; set; } // a list of dropdownvalues.
        public Object defaultValue { get; set; } // the default value used for the parameter, calculated at runtime.
        public bool deprecated { get; set; } // is the parameter still in use
        public Enums.DataType dataType { get; set; } // real acad datatype calculated at runtime.   

        private string dataTypeString; // string containing name of datatype for parsing.            
        private string keuzelijstenPad; // pad keuzelijsten
        
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
        public OTL_Parameter(string keuzelijstenPad, string dotNotatie, string friendlyName, string description, string dataTypeString, bool deprecated)
        {
            dropdownValues = new List<string>();
            this.dotNotatie = dotNotatie;
            this.friendlyName = friendlyName;
            this.description = description;
            this.dataTypeString = dataTypeString;
            this.deprecated = deprecated;
            this.keuzelijstenPad = keuzelijstenPad;
            dataType = ParameterHandler.GetDataType(dataTypeString);
            defaultValue = ParameterHandler.GetDefaultValue(dataTypeString);
            dropdownValues = ParameterHandler.GetDropDownValues(dataTypeString, keuzelijstenPad);
        }
    }
}
