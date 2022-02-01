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
        private SubsetImporter conn;
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

        /// <summary>
        /// Interface Handle voor het importeren van de OTL database
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="klPath"></param>
        public async Task ImportSubset(string dbPath, string klPath)
        {
            showProgressBar("De OTL Subset wordt geimporteerd.");
            conn = new SubsetImporter(dbPath, klPath);
            await Task.Run(() => { conn.ParseDatabase(true, true); });
            hideProgressBar();
        }

        public void showMessage(string message, string header)
        {
            MessageBox.Show(message, header, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            TemplateExporter exp = new TemplateExporter(conn.OTL_ObjectTypes);
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
            foreach(OTL_ObjectType otl in conn.OTL_ObjectTypes)
            {
                temp.Add(otl.otlName);
            }
            return temp;
        }

        
    }
}
