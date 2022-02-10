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
        private ExportXLSWindow exportXLSWindow;
        private LoadingWindow loadingWindow;
        private HomeWindow homeWindow;
        private ExportArtefactWindow artefactWindow;

        public ApplicationManager()
        {
            exportXLSWindow = new ExportXLSWindow(this);
            loadingWindow = new LoadingWindow(this);
            homeWindow = new HomeWindow(this);
            artefactWindow = new ExportArtefactWindow(this);
            // de interface wordt opgestart via Main Window            
            Application.Run(homeWindow);
        }

        /// <summary>
        /// Interface Handle voor het importeren van de OTL database
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="klPath"></param>
        public async Task ImportSubset(string dbPath, string klPath)
        {
            showProgressBar("De OTL Subset wordt geimporteerd.");
            subsetConn = new SubsetImporter(dbPath, klPath, this);
            await Task.Run(() => { subsetConn.ImportSubset(); });
            hideProgressBar();
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
                showMessage("De volgende parameters en klasses uit de OTL worden niet meer gebruikt. Bij voorkeur kijkt u de subset eerst na.\nIndien u dit niet doet, zullen de ongeldige parameters ook worden geëxporteerd.\n\nKlassen (inclusief alle parameters):\n" + deprecatedclasses + "\n\nParameters (in een bestaande klasse):\n" +deprecatedparameters, "Kijk de subset na voor u verder gaat");
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
            showProgressBar("De template wordt aangemaakt.");
            TemplateExporter exp = new TemplateExporter(subsetConn.GetOTL_ObjectTypes());
            exp.SetClasses(classes);
            await Task.Run(() => { exp.ExportXls(exportPath, withDescriptions, withChecklistOptions); });
            hideProgressBar();
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

        public void showExportXLS(Form form)
        {
            form.Enabled = false;
            exportXLSWindow.Show();
        }

        public void showExportArtefact(Form form)
        {
            form.Enabled = false;
            artefactWindow.Show();
        }

        public void showHome(Form form)
        {
            // first reset all forms and databases upon refresh
            

            form.Hide();
            homeWindow.Enabled = true;
            homeWindow.Show();
            homeWindow.Select();
        }

        public void showProgressBar(string text)
        {
            loadingWindow.SetProgressLabelText(text);
            loadingWindow.Show();
        }

        public void hideProgressBar()
        {
            loadingWindow.Hide();
        }

        public void showMessage(string message, string header)
        {
            MessageBox.Show(message, header, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
