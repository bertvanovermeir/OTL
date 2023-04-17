using System;
using System.Collections.Generic;

namespace OTLWizard.Helpers
{
    /// <summary>
    /// Deze klasse omschrijft een OTL objecttype en de daarbijhorende relatietypes en parameters.
    /// Door te verwijzen in de code naar de lijsten in deze klasse, kan je de volledige subset uitlezen via C objecten.
    /// (klasse is serializable)
    /// </summary>
    [Serializable]
    public class OTL_ObjectType
    {
        // parameters are public for eventual serialization
        public string otlName;
        public string friendlyName;
        public string description;
        public string uri;
        public bool deprecated;
        public string[] geometryRepresentation;

        private List<OTL_Parameter> parameters; // a dictionary with parameters
        private List<OTL_RelationshipType> relationTypes; // a dictionary with possible relationships for this object, according to SQL relationships

        /// <summary>
        /// creates a new type OTL object
        /// </summary>
        /// <param name="otlName"></param>
        /// <param name="friendlyName"></param>
        /// <param name="description"></param>
        /// <param name="uri"></param>
        /// <param name="deprecated"></param>
        public OTL_ObjectType()
        {
            parameters = new List<OTL_Parameter>();
            relationTypes = new List<OTL_RelationshipType>();
        }

        public void AddParameter(OTL_Parameter parameter)
        {
            parameters.Add(parameter);
        }

        public List<OTL_Parameter> GetParameters()
        {
            parameters.Sort((x, y) => x.DotNotatie.CompareTo(y.DotNotatie));
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
