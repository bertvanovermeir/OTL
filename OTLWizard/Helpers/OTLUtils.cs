using OTLWizard.OTLObjecten;
using Programmerare.CrsTransformations;
using Programmerare.CrsTransformations.CompositeTransformations;
using Programmerare.CrsTransformations.Coordinate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.Helpers
{
    public static class OTLUtils
    {

        private static int epsg3173 = Programmerare.CrsConstants.ConstantsByAreaNameNumber.v10_036.EpsgNumber.BELGIUM__ONSHORE__BD72__BELGIAN_LAMBERT_72__31370;
        private static int epsgWgs84 = Programmerare.CrsConstants.ConstantsByAreaNameNumber.v10_036.EpsgNumber.WORLD__WGS_84__4326;
        private static ICrsTransformationAdapter crsTransformationAdapter = CrsTransformationAdapterCompositeFactory.Create().CreateCrsTransformationMedian();


        /// <summary>
        /// This will simplify the base64 notation but only if that parameter is set by the user and if it actually can execute.
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public static string SimplifyBase64Notation(string assetId)
        {
            string result = assetId;

            if (Boolean.Parse(Settings.Get("hidebase64cosmetic")))
            {
                // change the relationshipuris, if applicable, if not silently fail.
                // they look like this: 632f6526-7bdf-4d44-a289-a0a00a993793-b25kZXJkZWVsI0VpbmRzdHVr               
                try
                {
                    var col = assetId.Split('-');
                    if (col.Length > 4)
                    {
                        var replacer = col[col.Length - 1];
                        result = assetId.Replace("-" + replacer, "");
                    }
                }
                catch
                {
                    // silent error
                }
            }
            return result;
        }

        private static CrsCoordinate TransformCoordinates(double x, double y)
        {
            CrsCoordinate lambert = CrsCoordinateFactory.LatLon(y,x, epsg3173);
            CrsTransformationResult ResultSweRef = crsTransformationAdapter.Transform(lambert, epsgWgs84);

            if (ResultSweRef.IsSuccess)
            {
                return ResultSweRef.OutputCoordinate;
                
            } else
            {
                return null;
            }
        }

        public static List<string> GenerateMapLocation(OTL_Entity ent)
        {
            List<string> locationpoints = new List<string>();
            if (ent.Properties.ContainsKey("geometry"))
            {

                var WKT = ent.Properties["geometry"].ToLower();
                WKT = WKT.Replace(")", "").Replace("(", "").Replace("z", "").Replace("xy", "").Replace("polygon", "").Replace("linestring", "").Replace("point", "").Replace("multi","").Trim();
                var temp = WKT.Split(',');
                foreach (var item in temp)
                {
                    try
                    {
                        var coords = item.Trim().Replace(".", ",").Split(' ');
                        // opvragen coordinaat
                        CrsCoordinate coord = OTLUtils.TransformCoordinates(Double.Parse(coords[0]), Double.Parse(coords[1]));
                        if (coord != null)
                        {
                            string xs = coord.X.ToString().Replace(',', '.');
                            string ys = coord.Y.ToString().Replace(',', '.');
                            // add to locations
                            locationpoints.Add(ys + " " + xs);
                        }
                    } catch { //no location added for this asset due to uncatched error
                              }
                              
                }
            }
            else
            {
                // locationpoints will be empty
            }
            return locationpoints;
        }
    }
}
