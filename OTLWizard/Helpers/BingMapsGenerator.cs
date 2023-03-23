using OTLWizard.OTLObjecten;
using Programmerare.CrsTransformations.Coordinate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.Helpers
{
    public class BingMapsGenerator
    {
        private List<string> HTML_locations;
        private List<string> HTML_mapPush;
        private List<string> HTML_pin;
        private List<string> HTML_line;
        private int height;
        private int width;
        private string locationid = "loc00";

        public BingMapsGenerator(int height, int width)
        {
            HTML_locations = new List<string>();
            HTML_mapPush = new List<string>();
            HTML_pin = new List<string>();
            HTML_line = new List<string>();
            this.height = height;
            this.width = width;
        }

        public string generateWebPage(List<OTL_GeometryEntity> selection, bool viewIsRoads, string zoomlevel)
        {
            int iterator_wkt = 0;
            int iterator_coord = 0;

            foreach (OTL_GeometryEntity entity in selection)
            {
                var singleObject = entity.GetEntity().GlobalWKT.Values;
                List<string> locationsForThisObject = new List<string>();
                

                foreach(string so in singleObject)
                {
                    var tmp = so.Trim();
                    tmp = tmp.Replace(" ", ", ");
                    var locationstring = "var loc" + iterator_wkt + iterator_coord + " = new Microsoft.Maps.Location(" + tmp + ");";                   
                    HTML_locations.Add(locationstring);
                    locationsForThisObject.Add("loc" + iterator_wkt + iterator_coord);
                    iterator_coord++;
                }

                if (entity.IsMapReferencePoint())
                {
                    locationid = "loc" + iterator_wkt + "" + (locationsForThisObject.Count/2);
                }

                iterator_coord = 0;
                var label = entity.GetName();
                var e_naam = entity.GetEntity().Name;
                var e_id = entity.GetEntity().AssetId;
                if (Boolean.Parse(Settings.Get("showassetnamewherepossible")) && entity.GetEntity().Properties.ContainsKey("naam"))
                    e_id = entity.GetEntity().Properties["naam"];
                var strokesize = "2";
                if (entity.getForeground())
                    strokesize = "4";
            


                // check type
                if (locationsForThisObject.Count == 2)
                {
                    var line = "var lineVertices" + iterator_wkt + " = new Array(" + locationsForThisObject[0] + "," + locationsForThisObject[1] + ");var line" + iterator_wkt + " = new Microsoft.Maps.Polyline(lineVertices" + iterator_wkt + ",{strokeColor: '" + entity.GetColor() + "' ,strokeThickness: " + strokesize + ", title : '" + entity.GetName() + "' });";
                    line = line + "line" + iterator_wkt +  ".metadata = {title: '" + e_naam + "',description: '" + e_id + "'};  Microsoft.Maps.Events.addHandler(line" + iterator_wkt + ", 'click', iPrimitiveClicked);";
                    HTML_line.Add(line);
                    var line_push = "map.entities.push(line" + iterator_wkt + ");";
                    HTML_mapPush.Add(line_push);
                    // also add a TRANSPARENT pushpin to attach label to.
                    var pt = "var pin" + iterator_wkt + " = new Microsoft.Maps.Pushpin(" + locationsForThisObject[0] + ",{icon: '<svg xmlns=\"https://www.w3.org/2000/svg\" width=\"1\" height=\"1\"></svg>', title : '" + label + "' });";
                    HTML_pin.Add(pt);
                    var pt_push = "map.entities.push(pin" + iterator_wkt + ");";
                    HTML_mapPush.Add(pt_push);

                } else if(locationsForThisObject.Count == 1)
                {
                    var pt = "var pin" + iterator_wkt + " = new Microsoft.Maps.Pushpin(" + locationsForThisObject[0] + ",{color: '" + entity.GetColor() + "', title : '" + label + "' });";
                    pt = pt + "pin" + iterator_wkt + ".metadata = {title: '" + e_naam + "',description: '" + e_id + "'};  Microsoft.Maps.Events.addHandler(pin" + iterator_wkt + ", 'click', iPrimitiveClicked);";
                    HTML_pin.Add(pt);
                    var pt_push = "map.entities.push(pin" + iterator_wkt + ");";
                    HTML_mapPush.Add(pt_push);

                } else if(locationsForThisObject.Count > 2)
                {
                    for (int i = 0; i < locationsForThisObject.Count - 1; i++)
                    {
                        var poly = "var polyVertices" + iterator_wkt + i + " = new Array(" + locationsForThisObject[i] + "," + locationsForThisObject[i + 1] + ");var poly" + iterator_wkt + i + " = new Microsoft.Maps.Polyline(polyVertices" + iterator_wkt + i + ",{strokeColor: '" + entity.GetColor() + "',strokeThickness: " + strokesize + ", title : '" + entity.GetName() + "'});";
                        poly = poly + "poly" + iterator_wkt + i + ".metadata = {title: '" + e_naam + "',description: '" + e_id + "'};  Microsoft.Maps.Events.addHandler(poly" + iterator_wkt + i + ", 'click', iPrimitiveClicked);";
                        HTML_line.Add(poly);
                        var poly_push = "map.entities.push(poly" + iterator_wkt + i + ");";
                        HTML_mapPush.Add(poly_push);
                    }        
                    // also add a TRANSPARENT pushpin to attach label to.
                    var pt =  "var pin" + iterator_wkt + " = new Microsoft.Maps.Pushpin(" + locationsForThisObject[(locationsForThisObject.Count/2)] + ",{icon: '<svg xmlns=\"https://www.w3.org/2000/svg\" width=\"1\" height=\"1\"></svg>', title : '" + label + "' });";
                    HTML_pin.Add(pt);
                    var pt_push = "map.entities.push(pin" + iterator_wkt + ");";
                    HTML_mapPush.Add(pt_push);
                }
                iterator_wkt++;
            }

            // All HTML generated, combine into HTML web page.
            // <OTL_GEO> and <OTL_PUSH>
            var view = "mapTypeId: Microsoft.Maps.MapTypeId.roads";
            if (!viewIsRoads)
                view = "mapTypeId: Microsoft.Maps.MapTypeId.aerial";
            
            var baseHTML = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8'> <script type='text/javascript' src='http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0'></script> <script type='text/javascript'>var map=null; var infobox; function GetMap(){ map = new Microsoft.Maps.Map(document.getElementById('mapDiv'),{credentials:'XgztV5wYl1ttbABnqX3J~lDyFWEBAZv_pAzojPBF1xw~AmlPcScLW0qNwjZ2b7NUaFL6wALuy8WVUMq25YYCF8cHYR46TnTQkOKvkyXutFLD' ," + view + "}); infobox = new Microsoft.Maps.Infobox(map.getCenter(), { visible:false}); infobox.setMap(map); <OTL_GEO> map.setView({center:" + locationid + ",zoom:" + zoomlevel + "}); <OTL_PUSH>} function iPrimitiveClicked(e) {if (e.target.metadata) {infobox.setOptions({location: e.location,title: e.target.metadata.title,description: e.target.metadata.description,visible: true});}} </script></head><body onload='GetMap();'><div id='mapDiv' style='position:relative; width:" + width + "px; height:" + height + "px;'></div></body></html>";

            // locations
            var locationstring2 = "";
            foreach(string loc in HTML_locations)
            {
                locationstring2 = locationstring2 + loc;
            }

            // geometry
            var geometrystring = "";           
            foreach(string geo in HTML_line)
            {
                geometrystring = geometrystring + geo;
            }
            foreach (string geo in HTML_pin)
            {
                geometrystring = geometrystring + geo;
            }
            // push
            var pushstring = "";
            foreach (var ps in HTML_mapPush)
            {
                pushstring = pushstring + ps;
            }

            

            baseHTML = baseHTML.Replace("<OTL_GEO>", locationstring2 + geometrystring);
            baseHTML = baseHTML.Replace("<OTL_PUSH>", pushstring);

            var path = System.IO.Path.GetTempPath() + "otldataviewer\\";

            Directory.CreateDirectory(path);

            path = path + "web.html";

            if(!baseHTML.Contains("var loc"))
            {
                baseHTML = "<h3 style='text-align: center;'><strong>" + Language.Get("clickassettobegin") + "</strong></h3><p><strong><img style='display: block; margin-left: auto; margin-right: auto;' src='https://d33wubrfki0l68.cloudfront.net/1f2d17dfef0f2a0c057bec7d9da581e9de7ec877/70d1f/static/2-6c44f6b506230bd0d0e5978e2e5c6f0d.gif' alt='' width='150' height='85' /></strong></p>";
            }

            File.WriteAllText(path, baseHTML);

            return path;
        }
    }
}
