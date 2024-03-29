﻿using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace OTLWizard.Helpers
{
    /// <summary>
    /// Deze klasse omschrijft de import van de DB subset
    /// </summary>
    public class SubsetImporter
    {
        private readonly SQLiteConnection SqliteConnection;
        private readonly bool Keuzelijsten;
        private List<OTL_ObjectType> OTL_ObjectTypes;
        private List<OTL_RelationshipType> OTL_RelationTypes;
        private string OTLVersion;

        /// <summary>
        /// Maak een nieuwe databaseconnectie met een subset aan
        /// </summary>
        /// <param name="DatabasePad"></param>
        /// <param name="KeuzelijstenPad"></param>
        public SubsetImporter(string DatabasePad, bool Keuzelijsten = false)
        {
            // initialize
            OTL_ObjectTypes = new List<OTL_ObjectType>();
            OTL_RelationTypes = new List<OTL_RelationshipType>();
            // create a new database connection and set the keuzelijstenpad to the private variable
            SqliteConnection = new SQLiteConnection("Data Source = " + DatabasePad + "; Version = 3; Read Only = True;");
            this.Keuzelijsten = Keuzelijsten;
        }

        public SubsetImporter()
        {
            // initialize
            OTL_ObjectTypes = new List<OTL_ObjectType>();
            OTL_RelationTypes = new List<OTL_RelationshipType>();
        }

        /// <summary>
        /// return all OTL objecttypes in imported subset
        /// </summary>
        /// <returns></returns>
        public List<OTL_ObjectType> GetOTLObjectTypes()
        {
            return OTL_ObjectTypes;
        }

        public List<OTL_ObjectType> GetOTLObjectTypesFor(string[] classnames)
        {
            List<OTL_ObjectType> result = new List<OTL_ObjectType>();
            foreach (string classname in classnames)
            {
                foreach (OTL_ObjectType o in OTL_ObjectTypes)
                {
                    if (o.otlName == classname)
                    {
                        result.Add(o);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// return all OTL relations in imported subset
        /// </summary>
        /// <returns></returns>
        public List<OTL_RelationshipType> GetOTLRelationshipTypes()
        {
            return OTL_RelationTypes;
        }

        public void SetOTLRelationshipTypes(List<OTL_RelationshipType> types)
        {
            OTL_RelationTypes = types;
        }

        public string GetOTLVersion()
        {
            return OTLVersion;
        }

        public void ImportSelectiveAssetSet(OTL_Entity entity)
        {
            
        }


        public void ImportSelectiveRelationSet(OTL_Entity entity)
        {
            // RELATIONS 
            using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
            {
                string tempquery = QueryHandler.Get(Enums.Query.RelationSpecific).Replace("[OSLOCLASS]", entity.TypeUri);
                sqlite_cmd.CommandText = tempquery;
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // check columns in query to know what to transfer, 
                    OTL_RelationshipType temp = new OTL_RelationshipType((string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(0),
                        (string)sqlite_datareader.GetValue(2), (string)sqlite_datareader.GetValue(3));
                    OTL_RelationTypes.Add(temp);
                }
                sqlite_datareader.Close();
            }

            // RELATION ATTRIBUTES
            using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
            {
                foreach (OTL_RelationshipType RelType in OTL_RelationTypes)
                {
                    if(RelType.properties.Count == 0)
                    {
                        string tempquery = QueryHandler.Get(Enums.Query.Parameters).Replace("[OSLOCLASS]", RelType.relationshipURI);
                        sqlite_cmd.CommandText = tempquery;
                        var sqlite_datareader = sqlite_cmd.ExecuteReader();
                        // The SQLiteDataReader allows us to run through each row per loop
                        while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                        {
                            // check columns in query to know what to transfer, 
                            OTL_Parameter p = new OTL_Parameter(Keuzelijsten, (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(3),
                                (string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(2), bool.Parse((string)sqlite_datareader.GetValue(4)));
                            // override default value of the parameter if name is typeURI, this will autofill this parameter field upon export
                            if (p.FriendlyName.Contains("typeURI"))
                            {
                                p.DefaultValue = RelType.relationshipURI;
                            }
                            RelType.properties.Add(p);
                        }
                        sqlite_datareader.Close();
                    }                  
                }
            }
        }

        /// <summary>
        /// start parsing the database for OTL classes.
        /// </summary>
        public void Import(bool importRelations, bool importAssets)
        {
            //clear the lists to avoid repeating imports
            OTL_ObjectTypes = new List<OTL_ObjectType>();
            OTL_RelationTypes = new List<OTL_RelationshipType>();

            // open the connection
            SqliteConnection.Open();

            if (!File.Exists(SqliteConnection.FileName))
                throw new FileNotFoundException("Could not find the database file.");

            if(importAssets)
            {
                // OBJECT TYPES
                using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
                {
                    sqlite_cmd.CommandText = QueryHandler.Get(Enums.Query.Objects);
                    var sqlite_datareader = sqlite_cmd.ExecuteReader();
                    // The SQLiteDataReader allows us to run through each row per loop
                    while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                    {
                        // check columns in query to know what to transfer, 
                        OTL_ObjectType temp = new OTL_ObjectType
                        {
                            otlName = (string)sqlite_datareader.GetValue(0),
                            friendlyName = (string)sqlite_datareader.GetValue(1),
                            description = (string)sqlite_datareader.GetValue(2),
                            uri = (string)sqlite_datareader.GetValue(3),
                            deprecated = bool.Parse((string)sqlite_datareader.GetValue(4))
                        };
                        OTL_ObjectTypes.Add(temp);
                    }
                    sqlite_datareader.Close();
                }

                // PARAMETERS
                // this should be executed for each OTL class, therefore:
                using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
                {
                    foreach (OTL_ObjectType OTLClass in OTL_ObjectTypes)
                    {
                        string tempquery = QueryHandler.Get(Enums.Query.Parameters).Replace("[OSLOCLASS]", OTLClass.uri);
                        sqlite_cmd.CommandText = tempquery;
                        var sqlite_datareader = sqlite_cmd.ExecuteReader();
                        // The SQLiteDataReader allows us to run through each row per loop
                        while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                        {
                            // check columns in query to know what to transfer, 
                            OTL_Parameter p = new OTL_Parameter(Keuzelijsten, (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(3),
                                (string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(2), bool.Parse((string)sqlite_datareader.GetValue(4)));
                            // override default value of the parameter if name is typeURI, this will autofill this parameter field upon export
                            if (p.FriendlyName.Contains("typeURI"))
                            {
                                p.DefaultValue = OTLClass.uri;
                            }
                            OTLClass.AddParameter(p);
                        }
                        sqlite_datareader.Close();
                    }
                }
            }
            
            if(importRelations)
            {
                // RELATIONS 
                using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
                {
                    sqlite_cmd.CommandText = QueryHandler.Get(Enums.Query.Relations); ;
                    var sqlite_datareader = sqlite_cmd.ExecuteReader();
                    // The SQLiteDataReader allows us to run through each row per loop
                    while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                    {
                        // check columns in query to know what to transfer, 
                        OTL_RelationshipType temp = new OTL_RelationshipType((string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(0),
                            (string)sqlite_datareader.GetValue(2), (string)sqlite_datareader.GetValue(3));
                        OTL_RelationTypes.Add(temp);
                    }
                    sqlite_datareader.Close();
                }

                // RELATION ATTRIBUTES
                using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
                {
                    foreach (OTL_RelationshipType RelType in OTL_RelationTypes)
                    {
                        string tempquery = QueryHandler.Get(Enums.Query.Parameters).Replace("[OSLOCLASS]", RelType.relationshipURI);
                        sqlite_cmd.CommandText = tempquery;
                        var sqlite_datareader = sqlite_cmd.ExecuteReader();
                        // The SQLiteDataReader allows us to run through each row per loop
                        while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                        {
                            // check columns in query to know what to transfer, 
                            OTL_Parameter p = new OTL_Parameter(Keuzelijsten, (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(3),
                                (string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(2), bool.Parse((string)sqlite_datareader.GetValue(4)));
                            // override default value of the parameter if name is typeURI, this will autofill this parameter field upon export
                            if (p.FriendlyName.Contains("typeURI"))
                            {
                                p.DefaultValue = RelType.relationshipURI;
                            }
                            RelType.properties.Add(p);
                        }
                        sqlite_datareader.Close();
                    }
                }
            }

            // VERSION NUMBER
            using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
            {
                sqlite_cmd.CommandText = QueryHandler.Get(Enums.Query.Version); ;
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    OTLVersion = (string)sqlite_datareader.GetValue(0);
                }
                sqlite_datareader.Close();
            }
        }
    }
}
