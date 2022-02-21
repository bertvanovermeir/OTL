using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace OTLWizard.OTLObjecten
{
    public class ArtefactImporter
    {
        private SQLiteConnection sqlite_conn_OTL;
        private List<OTL_ArtefactType> OTL_ArtefactTypes;

        public ArtefactImporter(string dbpath)
        {
            OTL_ArtefactTypes = new List<OTL_ArtefactType>();
            sqlite_conn_OTL = new SQLiteConnection("Data Source = " + dbpath + "; Version = 3; Read Only = True;");
        }

        public void Import(IEnumerable<string> otlClasses)
        {
            // iterate over all OTL objects included in the subset
            // it is assumed this is already imported when executing this class (blocked by interface)
            foreach(string otlClass in otlClasses)
            {
                string tempquery = QueryHandler.Get(Enums.Query.Artefact).Replace("[OSLOCLASS]", otlClass);
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
            foreach (OTL_ArtefactType art in OTL_ArtefactTypes)
            {
                var otlClass = otlClasses.FirstOrDefault(c => c.Equals(art.overervenvan));
                art.opmerkingen = otlClass == null ? "neen" : "ja";
            }

        }

        public List<OTL_ArtefactType> GetOTLArtefactTypes()
        {
            return OTL_ArtefactTypes;
        }
    }
}
