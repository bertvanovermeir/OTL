using OTLWizard.ApplicationData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OTLWizard.OTLObjecten
{
    /// <summary>
    /// Belangrijkste klasse die de werking van het programma illustreert. 
    /// De interface legt connectie met de instatie van deze klasse voor de uitvoering van opdrachten (klikken op knoppen etc..)
    /// </summary>
    public static class ApplicationHandler
    {
        private static SubsetImporter subsetConn;
        private static ArtefactImporter artefactConn;
        private static string newestversion;
        private static string currentversion;

        public static void Start()
        {
            if (!CheckVersion())
                ViewHandler.Show("Een nieuwe versie is beschikaar. Het is aangeraden deze te installeren.", "Versiecontrole", MessageBoxIcon.Exclamation);
            ViewHandler.Start();
        }

        public static bool CheckVersion()
        {
            var downloadpath = System.IO.Path.GetTempPath() + "otlappversie\\";
            // create the folder if it does not exist
            Directory.CreateDirectory(downloadpath);
            // download the TTL file
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile("https://raw.githubusercontent.com/bertvanovermeir/OTL/master/OTLWizard/data/version.dat", downloadpath + "version.dat");
                }
                string[] lines = File.ReadAllLines(downloadpath + "version.dat", System.Text.Encoding.UTF8);
                foreach (string item in lines)
                {
                    newestversion = item;
                }
                string[] lines2 = File.ReadAllLines("version.dat", System.Text.Encoding.UTF8);
                foreach (string item in lines2)
                {
                    currentversion = item;
                }
                if(newestversion.Equals(currentversion))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static async Task ImportArtefact(string subsetPath, string artefactPath)
        {
            await ImportSubset(subsetPath);
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "Het Geometrie Artefact wordt geimporteerd.");            
            artefactConn = new ArtefactImporter(artefactPath);
            try
            {
                await Task.Run(() => { artefactConn.Import(GetSubsetClassNames()); });
            } catch
            {
                ViewHandler.Show("Het artefact kon niet worden geïmporteerd", "Algemene Fout", MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        public static string GetOTLVersion()
        {
            var temp = subsetConn.GetOTLVersion();
            if (temp == null)
                temp = "";
            return temp;
        }

        /// <summary>
        /// Interface Handle voor het importeren van de OTL database
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="klPath"></param>
        public static async Task<bool> ImportSubset(string dbPath, bool keuzelijsten = false)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "De OTL Subset wordt geimporteerd.");
            subsetConn = new SubsetImporter(dbPath, keuzelijsten);
            try
            {
                await Task.Run(() => { subsetConn.Import(); });
            } catch
            {
                ViewHandler.Show("De subset kon niet worden geïmporteerd. (Subset Versie < 2.0 of Corrupte Database)", "Algemene Fout", MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            return CheckDeprecated();          
        }

        /// <summary>
        /// check if any of these classes are deprecated
        /// this will generate a purely cosmetic message to the user. Action is in his hands.
        /// </summary>
        private static bool CheckDeprecated()
        {           
            string deprecatedclasses = "";
            string deprecatedparameters = "";
            bool showWarning = false;
            foreach (OTL_ObjectType otlObject in subsetConn.GetOTLObjectTypes())
            {
                if (otlObject.deprecated)
                {
                    deprecatedclasses = deprecatedclasses + otlObject.friendlyName + ", ";
                    showWarning = true;
                }
                foreach (OTL_Parameter p in otlObject.GetParameters())
                {
                    if (p.Deprecated && !otlObject.deprecated)
                    {
                        deprecatedparameters = deprecatedparameters + p.FriendlyName + " in " + otlObject.friendlyName + ", ";
                        showWarning = true;
                    }
                }
            }
            if (showWarning)
            {
                ViewHandler.Show("De volgende parameters en klasses uit de OTL worden niet meer gebruikt. Bij voorkeur kijkt u de subset eerst na." +
                    "\nIndien u dit niet doet, zullen de ongeldige parameters ook worden geëxporteerd.\n\nKlassen (inclusief alle parameters):\n"
                    + deprecatedclasses + "\n\nParameters (in een bestaande klasse):\n" + deprecatedparameters, "Kijk de subset na voor u verder gaat", MessageBoxIcon.Information);
            }
            return showWarning;
        }

        public static async Task ExportCSVSubset(string exportPath, Boolean withDescriptions, Boolean withChecklistOptions, Boolean dummyData, Boolean wkt, Boolean deprecated, string[] classes)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "De template wordt aangemaakt (CSV).");
            SubsetExporterCSV exp = new SubsetExporterCSV();
            bool successSubset = exp.SetOTLSubset(subsetConn.GetOTLObjectTypes());
            bool successSelection = exp.SetSelectedClassesByUser(classes);
            if (successSubset && successSelection)
            {
                var result = await Task.Run(() => exp.Export(exportPath, withDescriptions, dummyData, wkt, deprecated));
                if (!result)
                {
                    ViewHandler.Show("Kon het bestand niet opslaan, controleer of het in gebruik is.", "Fout bij opslaan", System.Windows.Forms.MessageBoxIcon.Error);
                }
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
                ViewHandler.Show("Export voltooid", "Template export", MessageBoxIcon.Information);
            }
            else
            {
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
                ViewHandler.Show("Er zijn geen klassen geselecteerd of de subset is leeg.", "Fout bij exporteren", System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Interface Handle voor het exporteren van de Template XLS
        /// </summary>
        /// <param name="exportPath"></param>
        /// <param name="withDescriptions"></param>
        /// <param name="withChecklistOptions"></param>
        /// <param name="classes"></param>
        public static async Task ExportXlsSubset(string exportPath, Boolean withDescriptions, Boolean withChecklistOptions, Boolean dummyData, Boolean wkt, Boolean deprecated, string[] classes)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "De template wordt aangemaakt (XLSX).");
            SubsetExporterXLS exp = new SubsetExporterXLS();
            bool successSubset = exp.SetOTLSubset(subsetConn.GetOTLObjectTypes());
            bool successSelection = exp.SetSelectedClassesByUser(classes);
            if (successSubset && successSelection)
            {               
                var result = await Task.Run(() => exp.Export(exportPath, withDescriptions, withChecklistOptions, dummyData, wkt, deprecated));
                if (!result)
                {
                    ViewHandler.Show("Kon het bestand niet opslaan, controleer of het in gebruik is.", "Fout bij opslaan", System.Windows.Forms.MessageBoxIcon.Error);
                }
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
                ViewHandler.Show("Export voltooid", "Template export", MessageBoxIcon.Information);
            }
            else
            {
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
                ViewHandler.Show("Er zijn geen klassen geselecteerd of de subset is leeg.", "Fout bij exporteren", System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static async Task ExportXlsArtefact(string exportPath, List<OTL_ArtefactType> artefacten)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "De artefactinformatie wordt geëxporteerd.");
            ArtefactExporterXLS exp = new ArtefactExporterXLS();
            var result = await Task.Run(() => exp.Export(exportPath, artefacten));
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            if (!result) 
            {
                ViewHandler.Show("Kon het bestand niet opslaan, controleer of het in gebruik is.", "Fout bij opslaan", System.Windows.Forms.MessageBoxIcon.Error);
            } else
            {
                ViewHandler.Show("Export voltooid", "Artefact export", MessageBoxIcon.Information);
            }
        }

        public static async Task ExportCSVArtefact(string exportPath, List<OTL_ArtefactType> artefacten)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "De artefactinformatie wordt geëxporteerd.");
            ArtefactExporterCSV exp = new ArtefactExporterCSV();
            var result = await Task.Run(() => exp.Export(exportPath, artefacten));
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            if (!result)
            {
                ViewHandler.Show("Kon het bestand niet opslaan, controleer of het in gebruik is.", "Fout bij opslaan", System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                ViewHandler.Show("Export voltooid", "Artefact export", MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Interface Handle voor het vullen van de Listbox met alle mogelijke klassen die zich in de subset bevinden.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetSubsetClassNames()
        {
            return subsetConn.GetOTLObjectTypes().Select(x => x.otlName);
        }

        /// <summary>
        /// Interface Handle voor het opvragen van alle ingeladen artefactdata, dit kan nog gefilterd worden
        /// in een later stadium met de user selection.
        /// </summary>
        /// <returns></returns>
        public static List<OTL_ArtefactType> GetArtefactResultData()
        {
            return artefactConn.GetOTLArtefactTypes();
        }

      
    }
}
