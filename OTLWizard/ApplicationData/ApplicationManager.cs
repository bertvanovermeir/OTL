using OTLWizard.FrontEnd;
using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OTLWizard
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
            await Task.Run(() => { artefactConn.ImportArtefact(); });
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
            await Task.Run(() => { subsetConn.ImportSubset(); });
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
                OpenMessage("De volgende parameters en klasses uit de OTL worden niet meer gebruikt. Bij voorkeur kijkt u de subset eerst na.\nIndien u dit niet doet, zullen de ongeldige parameters ook worden geëxporteerd.\n\nKlassen (inclusief alle parameters):\n" + deprecatedclasses + "\n\nParameters (in een bestaande klasse):\n" +deprecatedparameters, "Kijk de subset na voor u verder gaat");
            }
        }

        /// <summary>
        /// Interface Handle voor het exporteren van de Template XLS
        /// </summary>
        /// <param name="exportPath"></param>
        /// <param name="withDescriptions"></param>
        /// <param name="withChecklistOptions"></param>
        /// <param name="classes"></param>
        public async Task exportXls(string exportPath, Boolean withDescriptions, Boolean withChecklistOptions, string[] classes)
        {
            openView(Enums.Views.Loading, Enums.Views.isNull, "De template wordt aangemaakt.");
            TemplateExporter exp = new TemplateExporter(subsetConn.GetOTL_ObjectTypes());
            exp.SetClasses(classes);
            await Task.Run(() => { exp.ExportXls(exportPath, withDescriptions, withChecklistOptions); });
            openView(Enums.Views.isNull, Enums.Views.Loading, null);
        }

        /// <summary>
        /// Interface Handle voor het vullen van de Listbox met alle mogelijke klassen die zich in de subset bevinden.
        /// </summary>
        /// <returns></returns>
        public List<string> GetOTLClassNames()
        {
            List<string> temp = new List<string>();
            foreach(OTL_ObjectType otl in subsetConn.GetOTL_ObjectTypes())
            {
                temp.Add(otl.otlName);
            }
            return temp;
        }

        public List<OTL_ArtefactType> GetArtefactResultData()
        {
            return artefactConn.GetOTLArtefactTypes();
        }

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

        public void OpenMessage(string message, string header)
        {
            MessageBox.Show(message, header, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
