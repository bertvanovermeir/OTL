using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using OTLWizard.OTLObjecten;

namespace OTLWizard.ApplicationData
{
    /// <summary>
    /// Deze klasse omschrijft de import van de DB subset
    /// </summary>
    public class SubsetImporter
    {
        private readonly SQLiteConnection SqliteConnection;
        private readonly string KeuzelijstenPad;
        private List<OTL_ObjectType> OTL_ObjectTypes;
        private List<OTL_RelationshipType> OTL_RelationTypes;

        /// <summary>
        /// Maak een nieuwe databaseconnectie met een subset aan
        /// </summary>
        /// <param name="DatabasePad"></param>
        /// <param name="KeuzelijstenPad"></param>
        public SubsetImporter(string DatabasePad, string KeuzelijstenPad = null)
        {
            // initialize
            OTL_ObjectTypes = new List<OTL_ObjectType>();
            OTL_RelationTypes = new List<OTL_RelationshipType>();
            // create a new database connection and set the keuzelijstenpad to the private variable
            SqliteConnection = new SQLiteConnection("Data Source = " + DatabasePad + "; Version = 3; Read Only = True;");
            this.KeuzelijstenPad = KeuzelijstenPad;
        }

        /// <summary>
        /// return all OTL objecttypes in imported subset
        /// </summary>
        /// <returns></returns>
        public List<OTL_ObjectType> GetOTLObjectTypes()
        {
            return OTL_ObjectTypes;
        }

        /// <summary>
        /// return all OTL relations in imported subset
        /// </summary>
        /// <returns></returns>
        public List<OTL_RelationshipType> GetOTLRelationshipTypes()
        {
            return OTL_RelationTypes;
        }

        /// <summary>
        /// start parsing the database for OTL classes.
        /// </summary>
        public void Import()
        {
            //clear the lists to avoid repeating imports
            OTL_ObjectTypes = new List<OTL_ObjectType>();
            OTL_RelationTypes = new List<OTL_RelationshipType>();

            // open the connection
            SqliteConnection.Open();

            if (!File.Exists(SqliteConnection.FileName))
                throw new FileNotFoundException("Could not find the database file.");

            // OBJECT TYPES
            using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
            {
                sqlite_cmd.CommandText = QueryHelper.Get(Enums.Query.Objects);
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // check columns in query to know what to transfer, 
                    OTL_ObjectType temp = new OTL_ObjectType((string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(1),
                        (string)sqlite_datareader.GetValue(2), (string)sqlite_datareader.GetValue(3), bool.Parse((string)sqlite_datareader.GetValue(4)));
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
                    string tempquery = QueryHelper.Get(Enums.Query.Parameters).Replace("[OSLOCLASS]", OTLClass.otlName);
                    sqlite_cmd.CommandText = tempquery;
                    var sqlite_datareader = sqlite_cmd.ExecuteReader();
                    // The SQLiteDataReader allows us to run through each row per loop
                    while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                    {
                        // check columns in query to know what to transfer, 
                        OTL_Parameter p = new OTL_Parameter(KeuzelijstenPad, (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(3),
                            (string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(2), bool.Parse((string)sqlite_datareader.GetValue(4)));
                        // override default value of the parameter if name is typeURI, this will autofill this parameter field upon export
                        if (p.friendlyName.Contains("typeURI"))
                        {
                            p.defaultValue = OTLClass.uri;
                        }
                        OTLClass.AddParameter(p);
                    }
                    sqlite_datareader.Close();
                }
            }

            // RELATIONS (unused for now, but is imported anyway for future use)
            using (var sqlite_cmd = new SQLiteCommand(SqliteConnection))
            {
                sqlite_cmd.CommandText = QueryHelper.Get(Enums.Query.Relations); ;
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
        }
    }
}
