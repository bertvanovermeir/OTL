using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OTLWizard.Helpers
{
    public static class DummyDataHandler
    {

        public static Random rand;
        private static List<OTL_ArtefactType> artefacts;

        public static void initRandom(List<OTL_ArtefactType> oTL_ArtefactTypes)
        {
            artefacts = oTL_ArtefactTypes;
            rand = new Random();
        }

        /// <summary>
        /// returns a dummy value to inject in CSV or XLS
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetDummyValue(OTL_Parameter p, OTL_ObjectType o)
        {
            var result = "";
            var prefix = "dummy_";
            var DataTypeString = p.DataTypeString;

            // create a dummy value
            if (DataTypeString.Contains("XMLSchema#") || DataTypeString.Contains("rdf-schema#") || DataTypeString.Contains("generiek#Getal") || DataTypeString.Contains("#Dte"))
            {
                string temp = DataTypeString.Split('#')[1].ToLower();
                switch (temp)
                {
                    case "anyuri":
                        result = (string)p.DefaultValue;
                        break;
                    case "getal":
                        result = (rand.NextDouble() * 10.0d).ToString("F2");
                        break;
                    case "integer":
                        result = rand.Next(0, 100).ToString();
                        break;
                    case "decimal":
                        result = (rand.NextDouble() * 10.0d).ToString("F2");
                        break;
                    case "datetime":
                        result = (rand.Next(2030, 2050).ToString() + "-"
                            + rand.Next(1, 12).ToString() + "-"
                            + rand.Next(1, 29).ToString());
                        result = result + ":00:00:00";
                        break;
                    case "date":
                        result = (rand.Next(2030, 2050).ToString() + "-"
                            + rand.Next(1, 12).ToString() + "-"
                            + rand.Next(1, 29).ToString());
                        break;
                    case "time":
                        result = "00:00:00";
                        break;
                    case "string":
                        result = prefix + p.FriendlyName + GetRandomString();
                        break;
                    case "boolean":
                        var num = rand.Next(1, p.DropdownValues.Count);
                        result = p.DropdownValues[num];
                        break;
                    case "literal":
                        result = (rand.NextDouble() * 100.0d).ToString("F2");
                        break;
                    default:
                        result = prefix + p.FriendlyName + GetRandomString();
                        break;
                }
            }
            // kwantWaarde
            else if (DataTypeString.Contains("#KwantWrdIn"))
            {
                result = (rand.NextDouble() * 100.0d).ToString("F2");
            }
            // Enums TTL (lists in acad)
            else if (DataTypeString.Contains("#Kl"))
            {
                var num = rand.Next(0, p.DropdownValues.Count);
                result = p.DropdownValues[num];
            }
            else if (DataTypeString.Contains("WKT"))
            {
                result = GetWKTRandomString(o);
            }
            else
            {
                result = prefix + p.FriendlyName + GetRandomString();
            }
            return result;
        }

        private static string GetRandomString()
        {

            // Choosing the size of string
            // Using Next() string
            int stringlen = rand.Next(4, 10);
            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            {

                // Generating a random number.
                randValue = rand.Next(0, 26);

                // Generating random character by converting
                // the random number into character.
                letter = Convert.ToChar(randValue + 65);

                // Appending the letter to string.
                str = str + letter;
            }
            return ("_" + str).ToLower();
        }

        private static string GetWKTRandomString(OTL_ObjectType o)
        {
            // not so random though, as the artefact will calculate if geometry is required.
            string temp = "";
            var geom = artefacts.Where(w => w.URL == o.uri).Select(q => q).FirstOrDefault();
            // kies poly, line of point


            if (geom != null && geom.geometrie.Length > 1)
            {
                string[] keuzelijst = geom.geometrie.Split(',');
                foreach (string keuze in keuzelijst)
                {
                    switch (keuze)
                    {
                        case "polygoon 3D":
                            temp += "POLYGON Z ((" + GetCoord() + "," + GetCoord() + "," + GetCoord() + "," + GetCoord() + "," + GetCoord() + "))|";
                            break;
                        case "lijn 3D":
                            temp += "LINESTRING Z (" + GetCoord() + "," + GetCoord() + ")|";
                            break;
                        case "punt 3D":
                            temp += "POINT Z (" + GetCoord() + ")|";
                            break;
                        default:
                            temp += "";
                            break;
                    }
                }
                if (temp.EndsWith("|"))
                    temp = temp.Substring(0, temp.Length - 1);
            }
            else
            {
                temp = "";
            }
            return temp;
        }

        private static string GetCoord()
        {
            // 3 coords
            double x = rand.Next(26000, 260000);
            double y = rand.Next(155000, 241000);
            double z = rand.Next(-20, 115);
            // 4 getallen na komma
            x = x + rand.NextDouble();
            y = y + rand.NextDouble();
            z = z + rand.NextDouble();

            return (x.ToString("F4") + " " + y.ToString("F4") + " " + z.ToString("F4")).Replace(',', '.');
        }
    }
}
