﻿using System;
using System.Collections.Generic;

namespace OTLWizard.Helpers
{
    /// <summary>
    /// Een relatie tussen typeobject A en typeobject B. Gedefinieerd door hun otl klassenaam (verder bron en doelURI genoemd).
    /// Deze klasse wordt niet actief gebruikt in het programma, wel worden alle relatietypes aangemaakt bij import van een subset.
    /// Op deze manier kan er op termijn een eventuele uitbreiding op eenvoudige wijze toegevoegd worden.
    /// (klasse is exporteerbaar via Serialize)
    /// </summary>
    [Serializable]
    public class OTL_RelationshipType
    {
        public string bronURI;
        public string bronOTLName;
        public string doelURI;
        public string doelOTLName;
        public string relationshipURI;
        public string relationshipName;
        public bool isDirectional;
        public bool isAbstract = false;
        public bool isImplementatieElement = false;
        public List<OTL_Parameter> properties;

        public string DisplayName { get; set; }

        public OTL_RelationshipType()
        {
            // for serialization instances
            properties = new List<OTL_Parameter>();
        }

        /// <summary>
        /// Maak een nieuw type van relatie aan tussen twee typeobjecten uit de OTL
        /// </summary>
        /// <param name="bronURI"></param>
        /// <param name="doelURI"></param>
        /// <param name="relationshipURI"></param>
        /// <param name="direction"></param>
        public OTL_RelationshipType(string bronURI, string doelURI, string relationshipURI, string direction)
        {
            properties = new List<OTL_Parameter>();

            this.bronURI = bronURI;
            this.doelURI = doelURI;
            this.relationshipURI = relationshipURI;
            try
            {
                bronOTLName = bronURI.Split('#')[1];
            }
            catch
            {
                bronOTLName = bronURI;
            }
            try
            {
                doelOTLName = doelURI.Split('#')[1];
            }
            catch
            {
                doelOTLName = doelURI;
            }
            relationshipName = relationshipURI.Split('#')[1];
            if (direction.Equals("Unspecified"))
            {
                isDirectional = false;
            }
            else
            {
                isDirectional = true;
            }
            if ((this.bronURI.Contains("abstracten#") || this.doelURI.Contains("abstracten#")))
                isAbstract = true;
            else if ((this.bronURI.Contains("implementatieelement#") || this.doelURI.Contains("implementatieelement#")))
                isImplementatieElement = true;
        }
    }
}
