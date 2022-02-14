using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using OTLWizard.OTLObjecten;

namespace OTLWizard.ApplicationData
{
    public class ArtefactImporter
    {
        private SQLiteConnection sqlite_conn_OTL;
        private ApplicationManager app;
        private List<OTL_ArtefactType> OTL_ArtefactTypes;

        public ArtefactImporter(string dbpath, ApplicationManager app)
        {
            this.app = app;
            OTL_ArtefactTypes = new List<OTL_ArtefactType>();
            sqlite_conn_OTL = new SQLiteConnection("Data Source = " + dbpath + "; Version = 3; Read Only = True;");
        }

        public void ImportArtefact()
        {
            string query="";
            // open the queries file (queries in known order in file)
            if (File.Exists(Directory.GetCurrentDirectory() + "\\queries.dat"))
            {
                string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\queries.dat", System.Text.Encoding.UTF8);
                query = lines[3];

            }
            else
            {
                // failure to open will generate a message for now, no further action
                app.OpenMessage("Could not retrieve Query information.", "Fatal Error",System.Windows.Forms.MessageBoxIcon.Warning);
            }
            // iterate over all OTL objects included in the subset
            // it is assumed this is already imported when executing this class (blocked by interface)
            var otlClasses = app.GetSubsetClassNames();
            foreach(string otlClass in otlClasses)
            {
                string tempquery = query.Replace("[OSLOCLASS]", otlClass);
                // open the connection:
                sqlite_conn_OTL.Open();
                var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
                sqlite_cmd.CommandText = tempquery;
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    string objectnaam = (string)sqlite_datareader.GetValue(0);
                    string geometrie = (string)sqlite_datareader.GetValue(1);
                    string overerving = (string)sqlite_datareader.GetValue(2);
                    string meetcriterium = (string)sqlite_datareader.GetValue(3);
                    string uitzonderingen = (string)sqlite_datareader.GetValue(4);
                    string overervingsgrens = (string)sqlite_datareader.GetValue(5);
                    string steekkaarten = (string)sqlite_datareader.GetValue(6);
                    string overervenvan = (string)sqlite_datareader.GetValue(7);
                    string viarelatie = (string)sqlite_datareader.GetValue(8);
                    string URL = (string)sqlite_datareader.GetValue(9);

                    // check columns in query to know what to transfer, 
                    OTL_ArtefactType artefact = new OTL_ArtefactType(objectnaam,geometrie,overerving,meetcriterium,uitzonderingen,overervingsgrens,steekkaarten,overervenvan,viarelatie,URL);
                    OTL_ArtefactTypes.Add(artefact);
                }
                sqlite_conn_OTL.Close();
            }
            // check if all classes from OTL are available to resolve Artefact
            bool found = false;
            foreach (OTL_ArtefactType art in OTL_ArtefactTypes)
            {
                
                foreach (string otlClass in otlClasses)
                {
                    if(otlClass.Equals(art.overervenvan))
                    {
                        found = true;
                        break;
                    }


                }
                if(found)
                {
                    art.opmerkingen = "ja";
                } else
                {
                    art.opmerkingen = "neen";
                }
                found = false;
            }

        }

        public List<OTL_ArtefactType> GetOTLArtefactTypes()
        {
            return OTL_ArtefactTypes;
        }
    }
}
