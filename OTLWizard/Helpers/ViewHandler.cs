using OTLWizard.FrontEnd;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OTLWizard.Helpers
{
    public static class ViewHandler
    {      
        private static ExportXLSWindow exportXLSWindow = new ExportXLSWindow();
        private static LoadingWindow loadingWindow = new LoadingWindow();
        public static HomeWindow homeWindow = new HomeWindow();
        private static ExportArtefactWindow artefactWindow = new ExportArtefactWindow();
        private static ArtefactResultWindow artefactResult = new ArtefactResultWindow();

        public static void Show(string message, string header, MessageBoxIcon icon)
        {
            MessageBox.Show(message, header, MessageBoxButtons.OK, icon);
        }

        public static void Start()
        {
            Application.Run(homeWindow);
        }

        public static void Show(Enums.Views toOpen, Enums.Views toClose, Object optionalArgument)
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
                    exportXLSWindow = new ExportXLSWindow();
                    loadingWindow = new LoadingWindow();
                    artefactWindow = new ExportArtefactWindow();
                    artefactResult = new ArtefactResultWindow();
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
    }
}
