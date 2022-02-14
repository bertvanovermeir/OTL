using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OTLWizard.FrontEnd;
using OTLWizard.OTLObjecten;

namespace OTLWizard.ApplicationData
{
    /// <summary>
    /// Belangrijkste klasse die de werking van het programma illustreert. 
    /// De interface legt connectie met de instatie van deze klasse voor de uitvoering van opdrachten (klikken op knoppen etc..)
    /// </summary>
    public class ApplicationManager
    {
        private SubsetImporter subsetConn;
        private ArtefactImporter artefactConn;
        private ExportXLSWindow exportXLSWindow;
        private LoadingWindow loadingWindow;
        private HomeWindow homeWindow;
        private ExportArtefactWindow artefactWindow;
        private ArtefactResultWindow artefactResult;

        public ApplicationManager()
        {
            exportXLSWindow = new ExportXLSWindow(this);
            loadingWindow = new LoadingWindow(this);
            homeWindow = new HomeWindow(this);
            artefactWindow = new ExportArtefactWindow(this);
            artefactResult = new ArtefactResultWindow(this);
            // de interface wordt opgestart via Main Window            
            Application.Run(homeWindow);
        }

        public async Task ImportArtefact(string subsetPath, string artefactPath)
        {
            await ImportSubset(subsetPath, "");
            openView(Enums.Views.Loading, Enums.Views.isNull, "Het Geometrie Artefact wordt geimporteerd.");            
            artefactConn = new ArtefactImporter(artefactPath,this);
            try
            {
                await Task.Run(() => { artefactConn.ImportArtefact(); });
            } catch
            {
                OpenMessage("Het artefact kon niet worden geïmporteerd", "Algemene Fout", MessageBoxIcon.Error);
            }
            openView(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        /// <summary>
        /// Interface Handle voor het importeren van de OTL database
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="klPath"></param>
        public async Task ImportSubset(string dbPath, string klPath)
        {
            openView(Enums.Views.Loading, Enums.Views.isNull, "De OTL Subset wordt geimporteerd.");
            subsetConn = new SubsetImporter(dbPath, klPath, this);
            try
            {
                await Task.Run(() => { subsetConn.ImportSubset(); });
            } catch
            {
                OpenMessage("De subset kon niet worden geïmporteerd", "Algemene Fout", MessageBoxIcon.Error);
            }
            
            openView(Enums.Views.isNull, Enums.Views.Loading, null);
            // now check if any of these classes are deprecated
            string deprecatedclasses = "";
            string deprecatedparameters = "";
            bool showWarning = false;
            foreach(OTL_ObjectType o in subsetConn.GetOTL_ObjectTypes())
            {
                if(o.deprecated)
                {
                    deprecatedclasses = deprecatedclasses + o.friendlyName + ", ";
                    showWarning = true;
                }
                foreach(OTL_Parameter p in o.GetParameters())
                {
                    if(p.deprecated && !o.deprecated)
                    {
                        deprecatedparameters = deprecatedparameters + p.friendlyName + " in " + o.friendlyName + ", ";
                        showWarning=true;
                    }                   
                }
            }
            if (showWarning)
            {
                OpenMessage("De volgende parameters en klasses uit de OTL worden niet meer gebruikt. Bij voorkeur kijkt u de subset eerst na." +
                    "\nIndien u dit niet doet, zullen de ongeldige parameters ook worden geëxporteerd.\n\nKlassen (inclusief alle parameters):\n" 
                    + deprecatedclasses + "\n\nParameters (in een bestaande klasse):\n" +deprecatedparameters, "Kijk de subset na voor u verder gaat",MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Interface Handle voor het exporteren van de Template XLS
        /// </summary>
        /// <param name="exportPath"></param>
        /// <param name="withDescriptions"></param>
        /// <param name="withChecklistOptions"></param>
        /// <param name="classes"></param>
        public async Task exportXlsSubset(string exportPath, Boolean withDescriptions, Boolean withChecklistOptions, string[] classes)
        {
            openView(Enums.Views.Loading, Enums.Views.isNull, "De template wordt aangemaakt.");
            TemplateExporter exp = new TemplateExporter(subsetConn.GetOTL_ObjectTypes(), this);
            exp.SetClasses(classes);
            await Task.Run(() => { exp.ExportXls(exportPath, withDescriptions, withChecklistOptions); });
            openView(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        public async Task exportXlsArtefact(string exportPath, List<OTL_ArtefactType> artefacten)
        {
            openView(Enums.Views.Loading, Enums.Views.isNull, "De artefactinformatie wordt geëxporteerd.");
            ArtefactExporter exp = new ArtefactExporter(this);
            await Task.Run(() => { exp.ExportXLS(exportPath, artefacten); });
            openView(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        /// <summary>
        /// Interface Handle voor het vullen van de Listbox met alle mogelijke klassen die zich in de subset bevinden.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSubsetClassNames()
        {
            return subsetConn.GetOTL_ObjectTypes().Select(x => x.otlName);
        }

        /// <summary>
        /// Interface Handle voor het opvragen van alle ingeladen artefactdata, dit kan nog gefilterd worden
        /// in een later stadium met de user selection.
        /// </summary>
        /// <returns></returns>
        public List<OTL_ArtefactType> GetArtefactResultData()
        {
            return artefactConn.GetOTLArtefactTypes();
        }

        /// <summary>
        /// open een venster met bepaalde inhoud, het optionele argument laat toe verschillende soorten data tussen 
        /// vensters te delen (gebruikersselectie etc...)
        /// </summary>
        /// <param name="toOpen"></param>
        /// <param name="toClose"></param>
        /// <param name="optionalArgument"></param>
        public void openView(Enums.Views toOpen, Enums.Views toClose, Object optionalArgument)
        {           
            switch (toClose)
            {
                case Enums.Views.Home:
                    homeWindow.Enabled = false;
                    break;
                case Enums.Views.Loading:
                    loadingWindow.Hide();
                    break;
                case Enums.Views.ArtefactMain:
                    artefactWindow.Hide();
                    break;
                case Enums.Views.ArtefactResult:
                    artefactResult.Hide();
                    break;
                case Enums.Views.SubsetMain:
                    exportXLSWindow.Hide();
                    break;
                default:
                    break;
            }
            switch (toOpen)
            {
                case Enums.Views.Home:
                    exportXLSWindow = new ExportXLSWindow(this);
                    loadingWindow = new LoadingWindow(this);
                    artefactWindow = new ExportArtefactWindow(this);
                    artefactResult = new ArtefactResultWindow(this);
                    homeWindow.Enabled = true;
                    homeWindow.Show();
                    homeWindow.Select();
                    break;
                case Enums.Views.Loading:
                    loadingWindow.SetProgressLabelText((string)optionalArgument);
                    loadingWindow.Show();
                    break;
                case Enums.Views.ArtefactMain:
                    artefactWindow.Show();
                    artefactWindow.Select();
                    break;
                case Enums.Views.ArtefactResult:
                    artefactResult.SetUserSelection((List<string>)optionalArgument);
                    artefactResult.Show();
                    artefactResult.Select();
                    break;
                case Enums.Views.SubsetMain:
                    exportXLSWindow.Show();
                    exportXLSWindow.Select();
                    break;
                default:
                    break;
            }
        }

        public void OpenMessage(string message, string header, MessageBoxIcon icon)
        {
            MessageBox.Show(message, header, MessageBoxButtons.OK, icon);
        }
    }
}
