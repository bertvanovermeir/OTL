using System;
using System.Collections.Generic;

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
        public string DotNotatie { get; set; } // complete dot notation name
        public string FriendlyName { get; set; } // friendly name of the parameter
        public string Description { get; set; } // description of the parameter
        public List<string> DropdownValues { get; set; } // a list of dropdownvalues.
        public Object DefaultValue { get; set; } // the default value used for the parameter, calculated at runtime.
        public bool Deprecated { get; set; } // is the parameter still in use
        public Enums.DataType DataType { get; set; } // real acad datatype calculated at runtime.
        public string DataTypeString { get; set; }

        // parameters SDF (ComplexType element)
        public string SDF_Name { get; set; } // vb: huidigbeeld_omschrijving
        public string SDF_RestrictionBase { get; set; } //vb: xs:string
        //TBD

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
        public OTL_Parameter(bool keuzelijsten, string dotNotatie, string friendlyName, string description, string dataTypeString, bool deprecated)
        {
            DropdownValues = new List<string>();
            this.DotNotatie = dotNotatie;
            this.FriendlyName = friendlyName;
            this.Description = description;
            this.Deprecated = deprecated;
            this.DataTypeString = dataTypeString;
            DataType = ParameterHandler.GetDataType(dataTypeString);
            DefaultValue = ParameterHandler.GetDefaultValue(dataTypeString);
            DropdownValues = ParameterHandler.GetDropDownValues(dataTypeString, keuzelijsten);
        }
    }
}
