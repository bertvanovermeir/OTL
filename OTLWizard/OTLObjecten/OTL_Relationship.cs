using CsvHelper.Configuration.Attributes;
using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTLWizard.OTLObjecten
{
    public class OTL_Relationship
    {
        [Name("AssetId.identificator")]
        public string AssetId { get; set; }
        [Name("typeURI")]
        public string relationshipURI { get; set; }
        [Name("doelAssetId.identificator")]
        public string doelID { get; set; }
        [Name("bronAssetId.identificator")]
        public string bronID { get; set; }
        [Ignore]
        public bool isDirectional { get; set; }
        [Ignore]
        public string DisplayName { get; set; }
        [Name("isActief")]
        public bool Activated { get; set; }

        [Ignore]
        public SerializableDictionary<string, string> Properties { get; set; }

        public OTL_Relationship()
        {
            Properties = new SerializableDictionary<string, string>();
        }

        public void GenerateDisplayName()
        {
            var bron = bronID;
            var doel = doelID;
            var dit = AssetId;

            if (Boolean.Parse(Settings.Get("hidebase64cosmetic")))
            {
                // change the relationshipuris, if applicable, if not silently fail.
                // they look like this: 632f6526-7bdf-4d44-a289-a0a00a993793-b25kZXJkZWVsI0VpbmRzdHVr               
                try
                {
                    var col = bron.Split('-');
                    var replacer = col[col.Length - 1];
                    bron = bron.Replace("-" + replacer, "");
                    col = doel.Split('-');
                    replacer = col[col.Length - 1];
                    doel = doel.Replace("-" + replacer, "");
                    col = dit.Split('-');
                    replacer = col[col.Length - 1];
                    dit = dit.Replace("-" + replacer, "");
                }
                catch
                {
                    // silent error
                }
            }

            if (Boolean.Parse(Settings.Get("hidereluricosmetic")))
            {
                // change the relationshipuris, if applicable, if not silently fail.
                // they look like this: 632f6526-7bdf-4d44-a289-a0a00a993793-b25kZXJkZWVsI0VpbmRzdHVr               
                dit = "";
            } else
            {
                dit = "(" + dit + ") ";
            }


            if (this.isDirectional)
            {
                this.DisplayName = dit + this.relationshipURI.Split('#')[1] + " | " + bron + " --> " + doel;

            }
            else
            {
                this.DisplayName = dit + this.relationshipURI.Split('#')[1] + " | " + bron + " <--> " + doel;
            }
            // actief of niet
            if(Activated)
                this.DisplayName = this.DisplayName + " (Actief)";
            else
                this.DisplayName = this.DisplayName + " (Verwijderd)";

        }
    }
}
