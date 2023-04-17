using OTLWizard.Helpers;
using OTLWizard.Helpers;
using OTLWizard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class DataConversionWindow : Form
    {
        public DataConversionWindow()
        {
            InitializeComponent();
            this.Text = Language.Get("converttitle");
            label1.Text = Language.Get("convertdescription");
            buttonSelect.Text = Language.Get("convertselectfile");
            button2.Text = Language.Get("convertconvert");


        }

        private void buttonSubset_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("converttitle");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "SDF Files (*.sdf)|*.sdf";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxSubset.Text = fdlg.FileName;
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // convert SDF to CSV
            String path = textBoxSubset.Text;
            if (SDFHandler.checkDependencies())
            {
                // set folder
                SaveFileDialog fdlg = new SaveFileDialog();
                fdlg.Title = Language.Get("converttitle");
                fdlg.FileName = "sdf_export";
                fdlg.Filter = "CSV files (*.csv)|*.csv";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                var localPath = "";

                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    localPath = fdlg.FileName;
                    localPath = localPath.ToLower().Replace(".csv", "");
                    
                    // convert
                    SDFHandler.GenerateCSVExportFiles(path, localPath);

                    ViewHandler.Show(Language.Get("convertok"), Language.Get("convertokheader"), System.Windows.Forms.MessageBoxIcon.Information);
                }
                else
                {
                    ViewHandler.Show(Language.Get("nofileselected"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                ViewHandler.Show(Language.Get("dependencymissing2"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Error);
                setFDODependency();
                ViewHandler.Show(Language.Get("restartprocess"), Language.Get("errorheader"), System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.DataConversion, null);
        }

        private static void setFDODependency()
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = true;
            fdlg.Title = Language.Get("SelectFDOCMDFilePath");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Executable Files (*.exe)|*.exe";
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                Settings.Update("sdfpath", fdlg.FileName);
                Settings.WriteSettings();
            }
        }
    }
}
