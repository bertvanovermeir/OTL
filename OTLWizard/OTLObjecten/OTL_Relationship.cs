using CsvHelper.Configuration.Attributes;
using OTLWizard.Helpers;
using System;

namespace OTLWizard.OTLObjecten
{
    public class OTL_Relationship
    {
        [Name("assetId.identificator")]
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
        public string doelName { get; set; }
        [Ignore]
        public string bronName { get; set; }
        [Ignore]
        public string DisplayName { get; set; }
        [Name("isActief")]
        public bool Activated { get; set; }

        [Ignore]
        public SerializableDictionary<string, string> Properties { get; set; }

        public OTL_Relationship()
        {
            Properties = new SerializableDictionary<string, string>();
            bronName = "";
            doelName = "";
        }

        public void GenerateDisplayName()
        {
            var bron = OTLUtils.SimplifyBase64Notation(bronID);
            var doel = OTLUtils.SimplifyBase64Notation(doelID);
            if (Boolean.Parse(Settings.Get("showassetnamewherepossible")))
            {
                if (!bronName.Equals(""))
                    bron = bronName;
                if (!doelName.Equals(""))
                    doel = doelName;
            }
            var relatieAssetId = OTLUtils.SimplifyBase64Notation(AssetId);

            if (Boolean.Parse(Settings.Get("hidereluricosmetic")))
            {
                relatieAssetId = "";
            }
            else
            {
                relatieAssetId = "(" + relatieAssetId + ") ";
            }


            if (this.isDirectional)
            {
                this.DisplayName = relatieAssetId + this.relationshipURI.Split('#')[1] + " | " + bron + " --> " + doel;

            }
            else
            {
                this.DisplayName = relatieAssetId + this.relationshipURI.Split('#')[1] + " | " + bron + " <--> " + doel;
            }
            // actief of niet
            if (Activated)
                this.DisplayName = this.DisplayName + " (Actief)";
            else
                this.DisplayName = this.DisplayName + " (Verwijderd)";

        }
    }
}
