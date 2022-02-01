using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OTLWizard
{
    /// <summary>
    /// Deze klasse omschrijft de import van de DB subset én van de TTL keuzelijstbestanden.
    /// </summary>
    class SubsetImporter
    {
        private SQLiteConnection sqlite_conn_OTL; // OTL objects
        private string skospath;
        public List<OTL_ObjectType> OTL_ObjectTypes;
        public List<OTL_RelationshipType> OTL_RelationTypes;

        /// <summary>
        /// create a new database connection. Should not be referenced more than once.
        /// </summary>
        /// <param name="path"></param>
        public SubsetImporter(string dbpath, string skospath)
        {
            // init
            OTL_ObjectTypes = new List<OTL_ObjectType>();
            OTL_RelationTypes = new List<OTL_RelationshipType>();
            // create a new database connection and set the skospath to the private variable
            sqlite_conn_OTL = new SQLiteConnection("Data Source = " + dbpath + "; Version = 3; Read Only = True;");
            this.skospath = skospath;
        }

        public bool ParseDatabase(bool objects, bool relations)
        {
            if(objects)
            {
                // OBJECT TYPES
                string query = "select * from OSLOClass as cla left join OSLORelaties as rel on rel.uri = cla.uri where rel.uri is NULL and cla.uri like '%onderdeel%' or cla.uri like '%installatie%'";
                // open the connection:
                sqlite_conn_OTL.Open();
                var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
                sqlite_cmd.CommandText = query;
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // check columns in query to know what to transfer, 
                    OTL_ObjectType temp = new OTL_ObjectType((string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(3), (string)sqlite_datareader.GetValue(2));
                    OTL_ObjectTypes.Add(temp);
                }
                sqlite_conn_OTL.Close();

                // PARAMETERS
                // this should be executed for each OTL class, therefore:
                foreach (OTL_ObjectType OTLClass in OTL_ObjectTypes)
                {
                    List<OTL_Parameter> tempParams = new List<OTL_Parameter>();
                    query = "select osloAttributen.name as MainAttributeName, osloAttributen.label_nl as MainAttributeFriendlyName, osloAttributen.definition_nl as MainAttributeDescription, osloAttributen.kardinaliteit_max as MainCardinality, osloAttributen.type as MainDataType, osloAttributenType1.name as SubAttributeName, osloAttributenType1.label_nl as SubAttributeFriendlyName, osloAttributenType1.definition_nl as SubAttributeDescription, osloAttributenType1.uri as URI, osloAttributenType1.kardinaliteit_max as SubCardinality, osloAttributenType1.type as SubDataType, osloAttributenType2.name as SubSubAttributeName, osloAttributenType2.label_nl as SubSubAttributeFriendlyName, osloAttributenType2.definition_nl as SubSubAttributeDescription, osloAttributenType2.uri as SubURI, osloAttributenType2.type as SubSubDataType, osloAttributenType3.name as UnionSubAttributeName, osloAttributenType3.type as UnionSubAttributeType, osloAttributenType4.name as UnionSubSubAttributeName, osloAttributenType4.type as UnionSubSubAttributeType , osloAttributenType3.definition_nl as UnionSubAttributeDescription, osloAttributenType4.definition_nl as UnionSubSubAttributeDescription " +
                   "from osloAttributen as osloAttributen left outer join osloDatatypeComplexAttributen as osloAttributenType1 on osloAttributen.type = osloAttributenType1.class_uri " +
                   "left outer join osloDatatypeComplexAttributen  as osloAttributenType2 on osloAttributenType1.type = osloAttributenType2.class_uri " +
                   "left outer join osloDatatypeUnionAttributen as osloAttributenType3 on osloAttributen.type = osloAttributenType3.class_uri " +
                   "left outer join osloDatatypeComplexAttributen  as osloAttributenType4 on osloAttributenType3.type = osloAttributenType4.class_uri where osloAttributen.class_uri like '%" + OTLClass.otlName + "%'";
                    // open the connection:
                    sqlite_conn_OTL.Open();
                    sqlite_cmd = sqlite_conn_OTL.CreateCommand();

                    sqlite_cmd.CommandText = query;
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    // The SQLiteDataReader allows us to run through each row per loop
                    while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                    {
                        // check columns in query to know what to transfer, 
                        OTL_Parameter p = new OTL_Parameter(
                            new List<object> { sqlite_datareader.GetValue(0), sqlite_datareader.GetValue(5), sqlite_datareader.GetValue(11), sqlite_datareader.GetValue(16), sqlite_datareader.GetValue(18) },
                            new List<object> { sqlite_datareader.GetValue(3), sqlite_datareader.GetValue(9), "1", "1", "1" }, // incorrect for now, i guess??
                            new List<object> { sqlite_datareader.GetValue(2), sqlite_datareader.GetValue(7), sqlite_datareader.GetValue(13), sqlite_datareader.GetValue(20), sqlite_datareader.GetValue(21) },
                            new List<object> { sqlite_datareader.GetValue(4), sqlite_datareader.GetValue(10), sqlite_datareader.GetValue(15), sqlite_datareader.GetValue(17), sqlite_datareader.GetValue(19) });
                        tempParams.Add(p);
                    }
                    sqlite_conn_OTL.Close();
                    // check data type again of every object in otlclass and change accordingly
                    for (int i = 0; i < tempParams.Count; i++)
                    {
                        OTL_Parameter parameter = tempParams[i];
                        // parse the datatype               
                        parameter = ParseDataType(parameter);
                        // override default value of the parameterif name is typeURI
                        if (parameter.friendlyName.Contains("typeURI"))
                        {
                            parameter.defaultValue = OTLClass.uri;
                        }
                        OTLClass.AddParameter(parameter);
                    }
                }               
            }

            if(relations)
            {
                string query = "select doel_uri,bron_uri,uri,richting from OSLORelaties";
                // open the connection:
                sqlite_conn_OTL.Open();
                var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
                sqlite_cmd.CommandText = query;
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // check columns in query to know what to transfer, 
                    OTL_RelationshipType temp = new OTL_RelationshipType((string)sqlite_datareader.GetValue(1), (string)sqlite_datareader.GetValue(0), (string)sqlite_datareader.GetValue(2), (string)sqlite_datareader.GetValue(3));
                    OTL_RelationTypes.Add(temp);
                }
                sqlite_conn_OTL.Close();
            }

            //
            return true;
        }
        
        /// <summary>
        /// parse each parameter for data type, to define default value and list if necessary
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private OTL_Parameter ParseDataType(OTL_Parameter p)
        {
            string DataType = p.dataTypeString;
            // primitives
            // very primitive
            if (DataType.Contains("XMLSchema#") || DataType.Contains("rdf-schema#") || DataType.Contains("generiek#Getal") || DataType.Contains("#Dte"))
            {
                string temp = DataType.Split('#')[1];
                switch (temp)
                {
                    case "Getal":
                        p.dataType = Enums.DataType.Real;
                        p.defaultValue = -99999.99d;
                        break;
                    case "Integer":
                        p.dataType = Enums.DataType.Integer;
                        p.defaultValue = 99999;
                        break;
                    case "Decimal":
                        p.dataType = Enums.DataType.Real;
                        p.defaultValue = -99999.99d;
                        break;
                    case "DateTime":
                        p.dataType = Enums.DataType.Text;
                        break;
                    case "Date":
                        p.dataType = Enums.DataType.Text;
                        break;
                    case "Time":
                        p.dataType = Enums.DataType.Text;
                        break;
                    case "String":
                        p.dataType = Enums.DataType.Text;
                        p.defaultValue = "-";
                        break;
                    case "Boolean":
                        p.dataType = Enums.DataType.List;
                        p.dropdownValues = new List<string> { "- ", "True", "False" };
                        p.defaultValue = "-";
                        break;
                    case "Literal":
                        p.dataType = Enums.DataType.Real;
                        p.defaultValue = -99999.99d;
                        break;
                    default:
                        p.dataType = Enums.DataType.Text;
                        p.defaultValue = "-";
                        break;
                }
            }
            // kwantWaarde
            else if (DataType.Contains("#KwantWrdIn"))
            {
                p.dataType = Enums.DataType.Real;
                p.defaultValue = -99999.99d;
            }
            // Unions (lists in acad)
            else if (DataType.Contains("#Dtu"))
            {
                // sql query needed here
                p.dataType = Enums.DataType.List;
                p.defaultValue = "-";
                p.dropdownValues = new List<string> { "-" };
                //query
                string temp = DataType.Split('#')[1];
                string query = "select name from oslodatatypeunionAttributen where class_uri like '%" + temp + "%'";
                // open the connection:
                sqlite_conn_OTL.Open();
                var sqlite_cmd = sqlite_conn_OTL.CreateCommand();
                sqlite_cmd.CommandText = query;
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                // The SQLiteDataReader allows us to run through each row per loop
                while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
                {
                    // check columns in query to know what to transfer, 
                    p.dropdownValues.Add((string)sqlite_datareader.GetValue(0));
                }
                sqlite_conn_OTL.Close();
            }
            // Enums TTL (lists in acad)
            else if (DataType.Contains("#Kl"))
            {
                //webrequest needed here or internal files
                p.dataType = Enums.DataType.List;
                p.dropdownValues = new List<string> { "-" };
                // query the files in attachment.
                // find the correct file in folder
                string filename = DataType.Split('#')[1] + ".ttl";
                if (File.Exists(skospath + "\\" + filename))
                {
                    string[] lines = File.ReadAllLines(skospath + "\\" + filename, System.Text.Encoding.UTF8);
                    foreach (string item in lines)
                    {
                        if (item.Contains("skos:Concept;"))
                        {
                            string listText = item.Split('>')[0];
                            string sublistText = listText.Split('/')[listText.Split('/').Length - 1];
                            p.dropdownValues.Add(sublistText);
                        }
                    }
                }
                p.defaultValue = "-";
            }
            else
            {
                p.dataType = Enums.DataType.Text;
                p.defaultValue = "-";
            }
            return p;
        }
    }
}
