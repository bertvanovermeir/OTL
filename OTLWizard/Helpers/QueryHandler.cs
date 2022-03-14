using System.IO;

namespace OTLWizard.OTLObjecten
{

    public static class QueryHandler
    {
        /// <summary>
        /// retrieves an SQL query from the query "db". 
        /// </summary>
        /// <param name="Query"></param>
        /// <returns>string if found, NULL if not found.</returns>
        public static string Get(Enums.Query Query)
        {
            string strQuery = null;
            // open the queries file (queries in known order in file)
            if (File.Exists(Directory.GetCurrentDirectory() + "\\queries.dat"))
            {
                string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\queries.dat", System.Text.Encoding.UTF8);
                switch (Query)
                {
                    case Enums.Query.Objects:
                        strQuery = lines[0];
                        break;
                    case Enums.Query.Parameters:
                        strQuery = lines[1];
                        break;
                    case Enums.Query.Relations:
                        strQuery = lines[2];
                        break;
                    case Enums.Query.Artefact:
                        strQuery = lines[3];
                        break;
                    case Enums.Query.Version:
                        strQuery = lines[4];
                        break;
                }
            }
            return strQuery;
        }
    }
}
