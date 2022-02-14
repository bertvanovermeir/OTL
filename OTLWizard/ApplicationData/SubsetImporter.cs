using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using OTLWizard.OTLObjecten;

namespace OTLWizard.ApplicationData
{
    /// <summary>
    /// Deze klasse omschrijft de import van de DB subset én van de TTL keuzelijstbestanden.
    /// </summary>
    class SubsetImporter
    {
        private SQLiteConnection sqlite_conn_OTL; // OTL objects
        private string klPath;
        private ApplicationManager app;
        private List<OTL_ObjectType> OTL_ObjectTypes;
        private List<OTL_RelationshipType> OTL_RelationTypes;
        

        /// <summary>
        /// create a new database connection. Should not be referenced more than once.
        /// </summary>
        /// <param name="path"></param>
        public SubsetImporter(string dbpath, string klPath, ApplicationManager app)
        {
            // init
            OTL_ObjectTypes = new List<OTL_ObjectType>();
            OTL_RelationTypes = new List<OTL_RelationshipType>();
            // create a new database connection and set the skospath to the private variable
            sqlite_conn_OTL = new SQLiteConnection("Data Source = " + dbpath + "; Version = 3; Read Only = True;");
            this.klPath = klPath;
            this.app = app;
        }

        /// <summary>
        /// return all OTL objecttypes in imported subset
        /// </summary>
        /// <returns></returns>
        public List<OTL_ObjectType> GetOTL_ObjectTypes()
        {
            return OTL_ObjectTypes;
        }

        /// <summary>
        /// return all OTL relations in imported subset
        /// </summary>
        /// <returns></returns>
        public List<OTL_RelationshipType> GetOTL_RelationshipTypes()
        {
            return OTL_RelationTypes;
        }

        /// <summary>
        /// start parsing the database for OTL classes.
        /// </summary>
        public void ImportSubset()
        {
            //clear the lists
            OTL_ObjectTypes = new List<OTL_ObjectType>();
            OTL_RelationTypes = new List<OTL_RelationshipType>();

            // 3 possible queries to run in order to import subset
            string queryOTLObjects = ""; // import otl objects
            string queryOTLParameters = ""; // import otl parameters per object
            string queryOTLRelations = ""; // import otl relations

            // open the queries file (queries in known order in file)
            if (File.Exists(Directory.GetCurrentDirectory() + "\\queries.dat"))
            {
                string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\queries.dat", System.Text.Encoding.UTF8);
                queryOTLObjects = lines[0];
                queryOTLParameters = lines[1];
                queryOTLRelations = lines[2];
            }
            else
            {
                // failure to open will generate a message for now, no further action
                app.OpenMessage("Could not retrieve Query information.", "Fatal Error", System.Windows.Forms.MessageBoxIcon.Warning);
            }

            // OBJECT TYPES
            // open the connection
            sqlite_conn_OTL.Open();
            var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
            sqlite_cmd.CommandText = queryOTLObjects;
            var sqlite_datareader = sqlite_cmd.ExecuteReader();
            // The SQLiteDataReader allows us to run through each row per loop
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // check columns in query to know what to transfer, 
                OTL_ObjectType temp = new OTL_ObjectType((string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(2), (string)sqlite_datareader.GetValue(3),bool.Parse((string)sqlite_datareader.GetValue(4)));
                OTL_ObjectTypes.Add(temp);
            }
            sqlite_conn_OTL.Close();

            // PARAMETERS
            // this should be executed for each OTL class, therefore:
            foreach (OTL_ObjectType OTLClass in OTL_ObjectTypes)
            {
                // open the connection:
                sqlite_conn_OTL.Open();
                sqlite_cmd = sqlite_conn_OTL.CreateCommand();
                string tempquery = queryOTLParameters.Replace("[OSLOCLASS]", OTLClass.otlName);
                sqlite_cmd.CommandText = tempquery;
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // check columns in query to know what to transfer, 
                    OTL_Parameter p = new OTL_Parameter(klPath, (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(3), (string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(2), bool.Parse((string)sqlite_datareader.GetValue(4)));
                    // override default value of the parameter if name is typeURI, this will autofill this parameter field upon export
                    if (p.friendlyName.Contains("typeURI"))
                    {
                        p.defaultValue = OTLClass.uri;
                    }
                    OTLClass.AddParameter(p);
                }
                sqlite_conn_OTL.Close();
            }

            // RELATIONS (unused for now, but is imported anyway for future use)
            // open the connection:
            sqlite_conn_OTL.Open();
            sqlite_cmd = sqlite_conn_OTL.CreateCommand();
            sqlite_cmd.CommandText = queryOTLRelations;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            // The SQLiteDataReader allows us to run through each row per loop
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // check columns in query to know what to transfer, 
                OTL_RelationshipType temp = new OTL_RelationshipType((string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(2), (string)sqlite_datareader.GetValue(3));
                OTL_RelationTypes.Add(temp);
            }
            sqlite_conn_OTL.Close();
        }
    }
}
