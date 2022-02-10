using System;
using System.Collections.Generic;

namespace OTLWizard
{
    /// <summary>
    /// Deze klasse omschrijft een OTL objecttype en de daarbijhorende relatietypes en parameters.
    /// Door te verwijzen in de code naar de lijsten in deze klasse, kan je de volledige subset uitlezen via C objecten.
    /// (klasse is serializable)
    /// </summary>
    [Serializable]
    public class OTL_ObjectType
    {
        public string otlName;
        public string friendlyName;
        public string description;
        public string uri;

        private List<OTL_Parameter> parameters; // a dictionary with parameters
        private List<OTL_RelationshipType> relationTypes; // a dictionary with possible relationships for this object, according to SQL relationships

        public OTL_ObjectType()
        {
            // for serialization
        }

        // contains name of object, parameters in correct notation, defaultvalues
        // everything to create a pset element
        public OTL_ObjectType(string otlName, string friendlyName, string description, string uri)
        {
            this.otlName = otlName;
            this.friendlyName = friendlyName;
            this.description = description;
            this.uri = uri;
            parameters = new List<OTL_Parameter>();
            relationTypes = new List<OTL_RelationshipType>();
        }

        public void AddParameter(OTL_Parameter parameter)
        {
            parameters.Add(parameter);
        }

        public List<OTL_Parameter> GetParameters()
        {
            return parameters;
        }

        public List<OTL_RelationshipType> GetOTL_RelationshipTypes()
        {
            return relationTypes;
        }

        public void AddRelationshipType(OTL_RelationshipType type)
        {
            relationTypes.Add(type);
        }

        public List<OTL_RelationshipType> GetRelationshipTypes()
        {
            return relationTypes;
        }

    }
}
