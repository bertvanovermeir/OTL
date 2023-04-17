using OTLWizard.FrontEnd;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OTLWizard.Helpers
{
    public static class ViewHandler
    {
        private static ExportSubsetWindow exportXLSWindow = new ExportSubsetWindow();
        private static LoadingWindow loadingWindow = new LoadingWindow();
        public static HomeWindow homeWindow = new HomeWindow();
        private static ExportArtefactWindow artefactWindow = new ExportArtefactWindow();
        private static ArtefactResultWindow artefactResult = new ArtefactResultWindow();
        private static SettingsWindow settingsWindow = new SettingsWindow();
        private static RelationWindow relationWindow = new RelationWindow();
        private static RelationImportDataWindow relationImportDataWindow = new RelationImportDataWindow();
        private static RelationUserDefinedWindow relationUserDefinedWindow = new RelationUserDefinedWindow();
        private static RelationImportSummaryWindow relationImportSummaryWindow = new RelationImportSummaryWindow("importsummaryheader");
        private static RelationImportSummaryWindow dataComparisonSummaryWindow = new RelationImportSummaryWindow("comparesummaryheader");
        private static SDXWindow sdxWindow = new SDXWindow();
        private static TutorialWindow tutorialWindow = new TutorialWindow();
        private static SubsetViewerWindow subsetViewerWindow = new SubsetViewerWindow();
        private static GeometryViewerWindow geometryViewerWindow = new GeometryViewerWindow();
        private static DataConversionWindow dataConversionWindow = new DataConversionWindow();
        private static DataComparisonWindow dataComparisonWindow = new DataComparisonWindow();

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
                case Enums.Views.Settings:
                    settingsWindow.Hide();
                    break;
                case Enums.Views.RelationsMain:
                    relationWindow.Hide();
                    break;
                case Enums.Views.RelationsImport:
                    relationImportDataWindow.Hide();
                    break;
                case Enums.Views.RelationsUserDefined:
                    relationUserDefinedWindow.Hide();
                    break;
                case Enums.Views.RelationImportSummary:
                    relationImportSummaryWindow.Hide();
                    break;
                case Enums.Views.DataComparisonSummary:
                    dataComparisonSummaryWindow.Hide();
                    break;
                case Enums.Views.SDFMain:
                    sdxWindow.Hide();
                    break;
                case Enums.Views.SubsetViewer:
                    subsetViewerWindow.Hide();
                    break;
                case Enums.Views.GeometryViewer:
                    geometryViewerWindow.Hide();
                    break;
                case Enums.Views.DataConversion:
                    dataConversionWindow.Hide();
                    break;
                case Enums.Views.DataComparison:
                    dataComparisonWindow.Hide();
                    break;
                default:
                    break;
            }
            switch (toOpen)
            {
                case Enums.Views.Home:
                    exportXLSWindow = new ExportSubsetWindow();
                    loadingWindow = new LoadingWindow();
                    artefactWindow = new ExportArtefactWindow();
                    artefactResult = new ArtefactResultWindow();
                    settingsWindow = new SettingsWindow();
                    relationWindow = new RelationWindow();
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
                case Enums.Views.Settings:
                    settingsWindow.Show();
                    break;
                case Enums.Views.RelationImportSummary:
                    relationImportSummaryWindow.SetDataSource(optionalArgument);
                    relationImportSummaryWindow.ShowDialog();
                    break;
                case Enums.Views.DataComparisonSummary:
                    dataComparisonSummaryWindow.SetDataSource(optionalArgument);
                    dataComparisonSummaryWindow.ShowDialog();
                    break;
                case Enums.Views.RelationsMain:
                    if (optionalArgument != null)
                    {
                        _ = relationWindow.ImportUserSelectionAsync((Dictionary<string, string[]>)optionalArgument);
                    }
                    relationWindow.Show();
                    break;
                case Enums.Views.RelationsImport:
                    relationImportDataWindow.ResetInterface();
                    relationImportDataWindow.Show();
                    break;
                case Enums.Views.RelationsUserDefined:
                    relationUserDefinedWindow.Init((OTL_ConnectingEntityHandle)optionalArgument);
                    relationUserDefinedWindow.ShowDialog();
                    break;
                case Enums.Views.SDFMain:
                    sdxWindow.Show();
                    break;
                case Enums.Views.SubsetViewer:
                    subsetViewerWindow = new SubsetViewerWindow();
                    subsetViewerWindow.Show();
                    break;
                case Enums.Views.GeometryViewer:
                    geometryViewerWindow = new GeometryViewerWindow();
                    geometryViewerWindow.Show();
                    break;
                case Enums.Views.DataConversion:
                    dataConversionWindow = new DataConversionWindow();
                    dataConversionWindow.Show();
                    break;
                case Enums.Views.DataComparison:
                    dataComparisonWindow = new DataComparisonWindow();
                    dataComparisonWindow.Show();
                    break;
                default:
                    break;
            }
            // tutorial open if necessary
            openTutorial(toOpen);
        }

        private static void openTutorial(Enums.Views toOpen)
        {
            tutorialWindow = new TutorialWindow();
            tutorialWindow.SetTutorial(toOpen);
        }
    }
}
