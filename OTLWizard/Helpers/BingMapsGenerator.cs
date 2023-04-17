using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace OTLWizard.Helpers
{
    public class BingMapsGenerator
    {
        private int height;
        private int width;
        private string htmlfile = "";

        public BingMapsGenerator(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        public void generateMainWebPage(List<OTL_GeometryEntity> selection, bool viewIsRoads, string zoomlevel)
        {
            var locationString = "";
            var geometryString = "";
            var pushString = "";
            var mapReferencePointerString = "";

            foreach (OTL_GeometryEntity entity in selection)
            {
                locationString += entity.GetBingLocationString();
                geometryString += entity.GetBingGeometryString();
                pushString += entity.GetBingPushString();
                if(entity.IsMapReferencePoint())
                    mapReferencePointerString = entity.GetBingReferencePointerString();
            }
            if(mapReferencePointerString.Equals("") && selection.Count > 0)
            {
                zoomlevel = "12";
                mapReferencePointerString = selection[selection.Count/2].GetBingReferencePointerString();
            }

            // All HTML generated, combine into HTML web page.
            // <OTL_GEO> and <OTL_PUSH>
            var view = "mapTypeId: Microsoft.Maps.MapTypeId.roads";
            if (!viewIsRoads)
                view = "mapTypeId: Microsoft.Maps.MapTypeId.aerial";

            var baseHTML = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
                "<html><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8'> <script type='text/javascript' src='http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0'></script> " +
                "<script type='text/javascript'>var sourceEntity=null;var targetEntity=null;var map=null; var infobox; " +
                "function GetMap(){ map = new Microsoft.Maps.Map(document.getElementById('mapDiv')," +
                "{credentials:'XgztV5wYl1ttbABnqX3J~lDyFWEBAZv_pAzojPBF1xw~AmlPcScLW0qNwjZ2b7NUaFL6wALuy8WVUMq25YYCF8cHYR46TnTQkOKvkyXutFLD' ," + 
                view + "}); infobox = new Microsoft.Maps.Infobox(map.getCenter(), { visible:false}); infobox.setMap(map); <OTL_GEO> map.setView({center:" +
                mapReferencePointerString + ",zoom:" + zoomlevel + "}); <OTL_PUSH>} function iPrimitiveClicked(e) {" +
                " if (e.target.metadata) " +
                "{infobox.setOptions({location: e.location,title: e.target.metadata.title,description: e.target.metadata.description,visible: true, " +
                "actions: [{label: 'bron', eventHandler: function (f) {console.log('source:' + e.target.metadata.id);" +
                "var i = 0, entity;while (i < map.entities.getLength()) {entity = map.entities.get(i);if(targetEntity == null){targetEntity = e.target;}if(e.target.metadata.id != targetEntity.metadata.id){entity.setOptions({ color: '" + Settings.Get("relationviewbackgroundcolor") + "', strokeColor: '" + Settings.Get("relationviewbackgroundcolor") + "'});}" + "i += 1;}" +
                " e.target.setOptions({ color: '" + Settings.Get("relationviewsourcecolor") + "', strokeColor: '" + Settings.Get("relationviewsourcecolor") + "'});sourceEntity=e.target;" +
                "}}, " +
                "{label: 'doel', eventHandler: function (f) {console.log('target:' + e.target.metadata.id);" +
                "var i = 0, entity;while (i < map.entities.getLength()) {entity = map.entities.get(i);if(sourceEntity == null){sourceEntity = e.target;}if(e.target.metadata.id != sourceEntity.metadata.id){entity.setOptions({ color: '" + Settings.Get("relationviewbackgroundcolor") + "', strokeColor: '" + Settings.Get("relationviewbackgroundcolor") + "'});}" + "i += 1;}" +
                " e.target.setOptions({ color: '" + Settings.Get("relationviewtargetcolor") + "', strokeColor: '" + Settings.Get("relationviewtargetcolor") + "'});targetEntity=e.target;" +
                "}}]});}} </script>" +
                "</head><body onload='GetMap();'><div id='mapDiv' style='position:relative; width:" + width + "px; height:" + height + "px;'></div></body></html>";

            baseHTML = baseHTML.Replace("<OTL_GEO>", locationString + geometryString);
            baseHTML = baseHTML.Replace("<OTL_PUSH>", pushString);

            var path = System.IO.Path.GetTempPath() + "otldataviewer\\";

            Directory.CreateDirectory(path);

            path = path + "web.html";

            if (!baseHTML.Contains("var loc"))
            {
                baseHTML = "<h3 style='text-align: center;'><strong>" + Language.Get("clickassettobegin") + "</strong></h3><p><strong><img style='display: block; margin-left: auto; margin-right: auto;' src='https://d33wubrfki0l68.cloudfront.net/1f2d17dfef0f2a0c057bec7d9da581e9de7ec877/70d1f/static/2-6c44f6b506230bd0d0e5978e2e5c6f0d.gif' alt='' width='150' height='85' /></strong></p>";
            }
            try
            {
                File.WriteAllText(path, baseHTML);
            } catch
            {
                // was still in use by different thread.
            }
            

            htmlfile = path;
        }

        public string getHtmlFilePath()
        {
            return htmlfile;
        }
    }
}
