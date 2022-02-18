using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OTLWizard.Helpers;

namespace OTLWizard.Helpers
{
    /// <summary>
    /// Belangrijkste klasse die de werking van het programma illustreert. 
    /// De interface legt connectie met de instatie van deze klasse voor de uitvoering van opdrachten (klikken op knoppen etc..)
    /// </summary>
    public static class ApplicationHandler
    {
        private static SubsetImporter subsetConn;
        private static ArtefactImporter artefactConn;

        public static void Start()
        {
            ViewHandler.Start();
        }

        public static async Task ImportArtefact(string subsetPath, string artefactPath)
        {
            await ImportSubset(subsetPath,null);
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "Het Geometrie Artefact wordt geimporteerd.");            
            artefactConn = new ArtefactImporter(artefactPath);
            try
            {
                await Task.Run(() => { artefactConn.ImportArtefact(); });
            } catch
            {
                ViewHandler.Show("Het artefact kon niet worden geïmporteerd", "Algemene Fout", MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        /// <summary>
        /// Interface Handle voor het importeren van de OTL database
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="klPath"></param>
        public static async Task ImportSubset(string dbPath, string klPath)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "De OTL Subset wordt geimporteerd.");
            subsetConn = new SubsetImporter(dbPath, klPath);
            try
            {
                await Task.Run(() => { subsetConn.Import(); });
            } catch
            {
                ViewHandler.Show("De subset kon niet worden geïmporteerd", "Algemene Fout", MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
            CheckDeprecated();          
        }

        /// <summary>
        /// check if any of these classes are deprecated
        /// this will generate a purely cosmetic message to the user. Action is in his hands.
        /// </summary>
        private static void CheckDeprecated()
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
                    + deprecatedclasses + "\n\nParameters (in een bestaande klasse):\n" + deprecatedparameters, "Kijk de subset na voor u verder gaat", MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Interface Handle voor het exporteren van de Template XLS
        /// </summary>
        /// <param name="exportPath"></param>
        /// <param name="withDescriptions"></param>
        /// <param name="withChecklistOptions"></param>
        /// <param name="classes"></param>
        public static async Task exportXlsSubset(string exportPath, Boolean withDescriptions, Boolean withChecklistOptions, string[] classes)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "De template wordt aangemaakt.");
            TemplateExporter exp = new TemplateExporter(subsetConn.GetOTLObjectTypes());
            exp.SetClasses(classes);
            var result = await Task.Run(() => exp.ExportXls(exportPath, withDescriptions, withChecklistOptions));
            if(!result)
            {
                ViewHandler.Show("Kon het bestand niet opslaan, controleer of het in gebruik is.", "Fout bij opslaan", System.Windows.Forms.MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        public static async Task exportXlsArtefact(string exportPath, List<OTL_ArtefactType> artefacten)
        {
            ViewHandler.Show(Enums.Views.Loading, Enums.Views.isNull, "De artefactinformatie wordt geëxporteerd.");
            ArtefactExporter exp = new ArtefactExporter();
            var result = await Task.Run(() => exp.ExportXLS(exportPath, artefacten));
            if (!result) 
            {
                ViewHandler.Show("Kon het bestand niet opslaan, controleer of het in gebruik is.", "Fout bij opslaan", System.Windows.Forms.MessageBoxIcon.Error);
            }
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.Loading, null);
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
