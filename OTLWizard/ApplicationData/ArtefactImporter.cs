using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.ApplicationData
{
    public class ArtefactImporter
    {
        private SQLiteConnection sqlite_conn_OTL;
        public List<OTL_ArtefactType> OTL_ArtefactTypes;

        public ArtefactImporter(string dbpath)
        {
            sqlite_conn_OTL = new SQLiteConnection("Data Source = " + dbpath + "; Version = 3; Read Only = True;");
        }

        public void ParseDatabase()
        {
            string query = "select * from *";
            // open the connection:
            sqlite_conn_OTL.Open();
            var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
            sqlite_cmd.CommandText = query;
            var sqlite_datareader = sqlite_cmd.ExecuteReader();
            // The SQLiteDataReader allows us to run through each row per loop
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // check columns in query to know what to transfer, 
                OTL_ArtefactType artefact = new OTL_ArtefactType();
                // OTL_ObjectType temp = new OTL_ObjectType((string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(3), (string)sqlite_datareader.GetValue(2));
                OTL_ArtefactTypes.Add(artefact);
            }
            sqlite_conn_OTL.Close();
        }
    }
}
