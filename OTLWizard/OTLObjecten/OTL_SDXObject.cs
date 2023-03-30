using OTLWizard.Helpers;
using System.Collections.Generic;
using System.Xml;
using static OTLWizard.OTLObjecten.Enums;

namespace OTLWizard.OTLObjecten
{
    public class OTL_SDXObject
    {
        // element type
        private string element_name;
        private string element_type;
        private string element_abstract;
        private string element_substitutionGroup;
        private string key_name;
        private string selector_xpath;
        private string field_xpath;
        private XmlDocument document;
        // complex type
        private string complextype_name;
        private string complextype_abstract;
        private string complextype_fdo_geometryName;
        private string annotation_documentation_innertext;
        private string complexContent_extension_base;

        private readonly OTL_ObjectType o;

        // list with attributes

        private List<OTL_SDXAttribute> attributes;

        // used for saving XML format for this sole class
        public OTL_SDXObject(XmlDocument document, OTL_ObjectType o)
        {
            // xml generation
            this.document = document;
            this.o = o;

            // object attributes
            this.element_name = "OTL_" + o.otlName;
            element_type = "Schema1:" + element_name + "Type";
            element_abstract = "false";
            element_substitutionGroup = "gml:_Feature";
            key_name = element_name + "Key";
            selector_xpath = ".//" + element_name;
            field_xpath = "FeatId";

            // complex attributes
            complextype_name = element_name + "Type";
            complextype_abstract = "false";
            complextype_fdo_geometryName = "Geometry"; // to check if it fails in case of no geometry
            annotation_documentation_innertext = o.description + " (export by OTL Wizard)";
            complexContent_extension_base = "gml:AbstractFeatureType";

            // attributes & add default attributes
            attributes = new List<OTL_SDXAttribute>();
            // Featid
            OTL_SDXAttribute featid = new OTL_SDXAttribute(document, "FeatId", "Default identity property", null, null, Enums.SDFAttributeTypes.FeatId);
            attributes.Add(featid);
            // Geometry if any
            if (o.geometryRepresentation != null)
            {
                var geometryTypes = "";
                foreach (string geom in o.geometryRepresentation)
                {
                    var geomtrimmed = geom.Trim();
                    // choose from point multipoint linestring multilinestring curvestring multicurvestring polygon multipolygon curvepolygon multicurvepolygo
                    switch (geomtrimmed)
                    {
                        case "punt 3D":
                            geometryTypes += "point multipoint ";
                            break;
                        case "lijn 3D":
                            geometryTypes += "linestring multilinestring ";
                            break;
                        case "polygoon 3D":
                            geometryTypes += "polygon multipolygon ";
                            break;
                        case "geen geometrie":
                            // do nothing
                            break;
                        default:
                            // do nothing
                            break;
                    }

                }
                OTL_SDXAttribute geometry = new OTL_SDXAttribute(document, "Geometry", "Default geometry property", null, null, Enums.SDFAttributeTypes.Geometry, geometryTypes);
                attributes.Add(geometry);
            }
            // other attributes
            foreach (OTL_Parameter p in o.GetParameters())
            {
                var tempType = ParameterHandler.GetDataTypeSDF(p.DataTypeString);
                var dval = "";
                if (p.DotNotatie.ToLower().Equals(Settings.Get("otlclassuri").ToLower()))
                    dval = (string)p.DefaultValue;
                if (p.DropdownValues != null)
                    AddAttribute(p.DotNotatie, p.Description, dval, p.DropdownValues.ToArray(), tempType);
                else
                    AddAttribute(p.DotNotatie, p.Description, dval, null, tempType);
            }

        }

        private void AddAttribute(string name, string description, string defaultvalue, string[] keuzelijstopties, SDFAttributeTypes type)
        {
            if (type == Enums.SDFAttributeTypes.List)
                defaultvalue = "-";
            OTL_SDXAttribute temp = new OTL_SDXAttribute(document, name, description, defaultvalue, keuzelijstopties, type);
            attributes.Add(temp);
        }

        public void AppendXml(XmlElement appendTo)
        {
            // element and attributes
            XmlElement element = createXsElement(appendTo, "element");
            element.SetAttribute("name", element_name);
            element.SetAttribute("type", element_type);
            element.SetAttribute("abstract", element_abstract);
            element.SetAttribute("substitutionGroup", element_substitutionGroup);

            // key child and below
            XmlElement key = createXsElement(element, "key");
            key.SetAttribute("name", key_name);
            createXsElement(key, "selector").SetAttribute("xpath", selector_xpath);
            createXsElement(key, "field").SetAttribute("xpath", field_xpath);

            // complextype generation for all attributes in class
            XmlElement complex = createXsElement(appendTo, "complexType");
            complex.SetAttribute("name", complextype_name);
            complex.SetAttribute("abstract", complextype_abstract);
            if (o.geometryRepresentation != null) // in case no geometry was found
                complex.SetAttribute("geometryName", "http://fdo.osgeo.org/schemas", complextype_fdo_geometryName);
            XmlElement annotation = createXsElement(complex, "annotation");
            XmlElement documentation = createXsElement(annotation, "documentation");
            documentation.InnerText = annotation_documentation_innertext;

            XmlElement content = createXsElement(complex, "complexContent");
            XmlElement extension = createXsElement(content, "extension");
            extension.SetAttribute("base", complexContent_extension_base);

            XmlElement sequence = createXsElement(extension, "sequence");

            // attributes go here
            foreach (OTL_SDXAttribute attri in attributes)
            {
                attri.AppendXml(sequence);
            }
        }

        private XmlElement createXsElement(XmlElement root, string name)
        {
            return (XmlElement)root.AppendChild(document.CreateElement("xs", name, "http://www.w3.org/2001/XMLSchema"));
        }
    }
}
