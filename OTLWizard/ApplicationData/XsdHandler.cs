using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;


namespace OTLWizard.ApplicationData
{
    public class XsdHandler
    {
        XmlDocument document;

        public List<OTL_ObjectType> oot = new List<OTL_ObjectType>();

        public XsdHandler() {
            document = new XmlDocument();
        }


        public bool Export(List<OTL_ObjectType> objects, string path)
        {

            XmlElement schema = generateXMLHeaders();

            foreach (OTL_ObjectType o in objects)
            {
                ApplicationHandler.SetArtefactForClass(o);
                OTL_SDXObject sdf = new OTL_SDXObject(document, o);
                sdf.AppendXml(schema);
            }
            try
            {
                document.Save(path);
                return true;
            } catch
            {
                return false;
            }
        }


        private XmlElement generateXMLHeaders()
        {
            // document version
            document.AppendChild(document.CreateXmlDeclaration("1.0", "UTF-8", null));
            // schema
            XmlElement schema = createXsElement("schema");
            schema.SetAttribute("xmlns:xs", "http://www.w3.org/2001/XMLSchema");
            schema.SetAttribute("targetNamespace", "http://fdo.osgeo.org/schemas/feature/Schema1");
            schema.SetAttribute("xmlns:fdo", "http://fdo.osgeo.org/schemas");
            schema.SetAttribute("xmlns:gml", "http://www.opengis.net/gml");
            schema.SetAttribute("xmlns:Schema1", "http://fdo.osgeo.org/schemas/feature/Schema1");
            schema.SetAttribute("elementFormDefault", "qualified");
            schema.SetAttribute("attributeFormDefault", "unqualified");
            
            // annotation and documentation
            XmlElement annotation = createXsElement(schema,"annotation");
            XmlElement documentation = createXsElement(annotation,"documentation");
            documentation.InnerText = "Default schema";

            return schema;
        }

        private XmlElement createXsElement(XmlElement root, string name)
        {
            return (XmlElement)root.AppendChild(document.CreateElement("xs", name, "http://www.w3.org/2001/XMLSchema"));
        }
        private XmlElement createXsElement(string name)
        {
            return (XmlElement)document.AppendChild(document.CreateElement("xs", name, "http://www.w3.org/2001/XMLSchema"));
        }

        public string[] GetXSDClasses(string path)
        {
            List<string> classes = new List<string>();
            document = new XmlDocument();
            XmlNodeList node;

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            document.Load(fs);
            // testing filters
            // setup name space manager for XSD
            var nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            node = document.SelectNodes("/xs:schema/xs:element[starts-with(@name,'OTL_')]", nsmgr);
            for (var i = 0; i <= node.Count - 1; i++)
            {
                var str = node[i].Attributes["name"].Value;
                classes.Add(str);
            }
            return classes.ToArray();
        }


        // test method unpacking
        public void Import()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlNodeList xmlnode;
            XmlNodeList xmlnode2;
            XmlNodeList xmlnode3;
            XmlNodeList xmlnode4;

            FileStream fs = new FileStream("data\\test.xsd", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            // testing filters
            // setup name space manager for XSD
            var nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            // the real deal
            xmlnode = xmldoc.SelectNodes("/xs:schema/xs:element[starts-with(@name,'OTL_')]", nsmgr);
            for (var i = 0; i <= xmlnode.Count - 1; i++)
            {
                var str = xmlnode[i].Attributes["name"].Value;
                Console.WriteLine("klasse: " + str);
                xmlnode2 = xmldoc.SelectNodes("/xs:schema/xs:complexType[starts-with(@name,'" + str + "')]", nsmgr);
                for (var j = 0; j <= xmlnode2.Count - 1; j++)
                {
                    var str2 = xmlnode2[j].Attributes["name"].Value;
                    Console.WriteLine("    attributenTypeLijst: " + str2);
                    xmlnode3 = xmldoc.SelectNodes("/xs:schema/xs:complexType[starts-with(@name,'" + str + "')]/xs:complexContent/xs:extension/xs:sequence/xs:element", nsmgr);
                    for (var k = 0; k <= xmlnode3.Count - 1; k++)
                    {
                        var str3 = xmlnode3[k].Attributes["name"].Value;
                        var str4 = "NO TYPE DEFINED";
                        xmlnode4 = xmldoc.SelectNodes("/xs:schema/xs:complexType[starts-with(@name,'" + str + "')]/xs:complexContent/xs:extension/xs:sequence/xs:element[starts-with(@name,'" + str3 + "')]/xs:simpleType/xs:restriction", nsmgr);
                        if (xmlnode4.Count > 0)
                            str4 = xmlnode4[0].Attributes["base"].Value;

                        Console.WriteLine("        attribuut: " + str3 + " - " + str4);
                    }
                }
            }


        }
    }
}
