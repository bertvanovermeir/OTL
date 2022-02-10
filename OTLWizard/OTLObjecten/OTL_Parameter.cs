using System;
using System.Collections.Generic;

namespace OTLWizard
{
    /// <summary>
    /// Deze klasse omschrijft één parameter en de optionele invulwaarden. 
    /// (klasse is exporteerbaar via Serialize)
    /// </summary>
    [Serializable]
    public class OTL_Parameter
    {
        public string dotNotatie; // complete dot notation name
        public string friendlyName; // friendly name of the parameter
        public string description; // description of the parameter
        public List<string> dropdownValues; // a list of dropdownvalues.
        public Object defaultValue; // the default value used for the parameter, calculated at runtime.
        public string dataTypeString; // string containing name of datatype for parsing.
        public Enums.DataType dataType; // real acad datatype calculated at runtime.
        public bool deprecated; // is the parameter still in use
        
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
        public OTL_Parameter(string dotNotatie, string friendlyName, string description, string dataTypeString, bool deprecated)
        {
            dropdownValues = new List<string>();
            this.dotNotatie = dotNotatie;
            this.friendlyName = friendlyName;
            this.description = description;
            this.dataTypeString = dataTypeString;
            this.deprecated = deprecated;
        }
    }
}
