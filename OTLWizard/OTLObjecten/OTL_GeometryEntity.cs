using System;
using System.Collections.Generic;

namespace OTLWizard.Helpers
{
    public class OTL_GeometryEntity
    {
        private OTL_Entity entity;
        private string color;
        private string strokesize;
        private string displayName;
        private string name;
        private string id;
        private List<string> wkt;

        private bool isMapReferencePoint = false;
        private bool isBackground = false;
        private bool isSource = false;
        private bool isTarget = false;
        private int serialNumber;

        private string bingGeometryString;
        private string bingLocationString;
        private string bingPushString;
        private List<string> bingLocations;
        private string bingMapReferencePointPointerString;

        public OTL_GeometryEntity(OTL_Entity entity)
        {
            this.entity = entity;
            this.displayName = entity.DisplayName;
            this.name = entity.Name;
            this.id = entity.AssetId;
            this.wkt = new List<string>();
            wkt.AddRange(entity.GlobalWKT.Values);
            bingLocations = new List<string>();
        }

        public void generateGeometryPointers()
        {
            // reset strings
            bingGeometryString = "";
            bingPushString = "";
            // parse geometry
            var label = id;
            var pinLabel = displayName;
            if (Boolean.Parse(Settings.Get("showassetnamewherepossible")) && entity.GetProperties().ContainsKey("naam"))
                label = entity.GetProperties()["naam"];
            if (!Boolean.Parse(Settings.Get("relationviewnolabels")) || isBackground)
                pinLabel = "";


            if (bingLocations.Count == 1)
            {
                var pt = "var pin" + serialNumber + " = new Microsoft.Maps.Pushpin(" + bingLocations[0] + ",{color: '" + color + "', title : '" + pinLabel + "' });";
                pt = pt + "pin" + serialNumber + ".metadata = {id: '" + id + "', title: '" + name + "',description: '" + label + "'};  Microsoft.Maps.Events.addHandler(pin" +
                    serialNumber + ", 'click', iPrimitiveClicked);";
                bingGeometryString += pt;
                var pt_push = "map.entities.push(pin" + serialNumber + ");";
                bingPushString += pt_push;

            }
            else if (bingLocations.Count > 1)
            {
                for (int i = 0; i < bingLocations.Count - 1; i++)
                {
                    var poly = "var polyVertices" + serialNumber + i + " = new Array(" + bingLocations[i] + "," + bingLocations[i + 1] + ");var poly" +
                        serialNumber + i + " = new Microsoft.Maps.Polyline(polyVertices" + serialNumber + i + ",{strokeColor: '" + color + "',strokeThickness: " +
                        strokesize + ", title : '" + pinLabel + "'});";
                    poly = poly + "poly" + serialNumber + i + ".metadata = {id: '" + id + "', title: '" + name + "',description: '" + label + "'};  Microsoft.Maps.Events.addHandler(poly" +
                        serialNumber + i + ", 'click', iPrimitiveClicked);";
                    bingGeometryString += poly;
                    var poly_push = "map.entities.push(poly" + serialNumber + i + ");";
                    bingPushString += poly_push;
                }
                // also add a TRANSPARENT pushpin to attach label to.
                var pt = "var pin" + serialNumber + " = new Microsoft.Maps.Pushpin(" + bingLocations[(bingLocations.Count / 2)]
                    + ",{icon: '<svg xmlns=\"https://www.w3.org/2000/svg\" width=\"1\" height=\"1\"></svg>', title : '" + pinLabel + "' });";
                bingGeometryString += pt;
                var pt_push = "map.entities.push(pin" + serialNumber + ");";
                bingPushString += pt_push;
            }
        }

        public void generateLocationPointers()
        {
            // parse location
            var iterator = 0;
            foreach (string coordinate in wkt)
            {
                var coord = coordinate.Trim();
                coord = coord.Replace(" ", ", ");
                var locationstring = "var loc" + serialNumber + iterator + " = new Microsoft.Maps.Location(" + coord + ");";
                bingLocationString += locationstring;
                bingLocations.Add("loc" + serialNumber + iterator);
                iterator++;
            }

            // set reference pointer in case used
            bingMapReferencePointPointerString = "loc" + serialNumber + (bingLocations.Count / 2);
        }

        public void SetSerialNumber(int serialNumber)
        {
            this.serialNumber = serialNumber;
        }

        public string GetBingLocationString()
        {
            return bingLocationString;
        }

        public string GetBingGeometryString()
        {
            return bingGeometryString;
        }

        public string GetBingPushString()
        {
            return bingPushString;
        }

        public string GetBingReferencePointerString()
        {
            return bingMapReferencePointPointerString;
        }

        public void SetAsBackGroundAsset()
        {
            resetDrawState();
            strokesize = "2";
            color = Settings.Get("relationviewbackgroundcolor");
            isBackground = true;
            isMapReferencePoint = false;
            generateGeometryPointers();
        }

        public void SetAsSourceAsset()
        {
            resetDrawState();
            strokesize = "4";
            color = Settings.Get("relationviewsourcecolor");
            isSource = true;
            isMapReferencePoint = true;
            generateGeometryPointers();
        }

        public void SetAsTargetAsset()
        {
            resetDrawState();
            strokesize = "4";
            color = Settings.Get("relationviewtargetcolor");
            isTarget = true;
            generateGeometryPointers();
        }

        public void SetAsMapReferencePoint()
        {
            isMapReferencePoint = true;
        }

        public bool IsMapReferencePoint()
        {
            return isMapReferencePoint;
        }

        public bool IsTargetAsset()
        {
            return isTarget;
        }

        public bool IsSourceAsset()
        {
            return isSource;
        }

        public bool IsBackgroundAsset()
        {
            return isBackground;
        }

        private void resetDrawState()
        {
            isTarget = false;
            isBackground = false;
            isSource = false;
        }

    }
}
