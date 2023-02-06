using System;
using System.Diagnostics;
using System.Windows.Forms;
using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;

namespace OTLWizard
{
    public partial class ExportSubsetWindow : Form
    {
        private bool importDone = false;

        public ExportSubsetWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Text = Language.Get("exportsubsetwindowheader");
            label1.Text = Language.Get("description1");
            label2.Text = Language.Get("description2");
            linkLabel1.Text = Language.Get("linkdescription1");
            buttonSubset.Text = Language.Get("selectsubset");
            buttonImportClasses.Text = Language.Get("importclasses");
            label3.Text = Language.Get("description3");
            label7.Text = Language.Get("otlversion");
            label4.Text = Language.Get("generalsettings");
            label5.Text = Language.Get("exampledata");
            label8.Text = Language.Get("legacydata");
            label6.Text = Language.Get("coffee");
            buttonExportXLS.Text = Language.Get("exportsubset");
            checkAllClasses.Text = Language.Get("checkallclasses");
            checkKeuzelijsten.Text = Language.Get("checkkl");
            checkVoorbeelddata.Text = Language.Get("checkexampledata");
            checkAttributes.Text = Language.Get("checkattributes");
            checkWKT.Text = Language.Get("checkwkt");
            checkDeprecated.Text = Language.Get("checkdeprecated");
            buttonArtefact.Text = Language.Get("buttonartefact");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://opendata.apps.mow.vlaanderen.be/otltool/subset/ui/");
        }

        private void selecteerSubsetButton(object sender, EventArgs e)
        {
            ListAllClasses.Items.Clear();
            buttonExportXLS.Enabled = false;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectsubsetfiledlg");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxSubset.Text = fdlg.FileName;
                buttonImportClasses.Enabled = true;
                checkAllClasses.Enabled = true;
                checkKeuzelijsten.Enabled = true;
                ListAllClasses.Enabled = true;
                buttonImportClasses.Enabled = true;
                checkVoorbeelddata.Enabled = true;
                checkDeprecated.Enabled = false;
                checkWKT.Enabled = true;
            }
        }

        private async void Export(object sender, EventArgs e)
        {
            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = Language.Get("arexport");
            fdlg.FileName = "subset_otlver" + ApplicationHandler.GetOTLVersion() + "_export"; 
            fdlg.Filter = "Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv";
            fdlg.FilterIndex= 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                string[] temp = null;
                if(ListAllClasses.Enabled == true)
                {
                    temp = new string[ListAllClasses.SelectedIndices.Count];
                    int i = 0;
                    foreach(int ind in ListAllClasses.SelectedIndices)
                    {                      
                        temp[i] = ListAllClasses.Items[ind].ToString();
                        i++;
                    }
                }
                if(fdlg.FilterIndex == 1)
                {
                    await ApplicationHandler.ExportXlsSubset(fdlg.FileName, textBoxArtefact.Text, Decimal.ToInt32(numericUpDown1.Value), checkAttributes.Checked, !checkKeuzelijsten.Checked, checkVoorbeelddata.Checked, checkWKT.Checked, checkDeprecated.Checked, temp);
                }
                else
                {
                    await ApplicationHandler.ExportCSVSubset(fdlg.FileName, textBoxArtefact.Text, Decimal.ToInt32(numericUpDown1.Value), checkAttributes.Checked, !checkKeuzelijsten.Checked, checkVoorbeelddata.Checked, checkWKT.Checked, checkDeprecated.Checked, temp);
                }
            }
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void WebGoToGithub(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Informatievlaanderen/OSLOthema-wegenenverkeer/archive/refs/heads/master.zip");
        }

        private async void ImportClasses(object sender, EventArgs e)
        {
            
            var deprecated = await ApplicationHandler.ImportSubset(textBoxSubset.Text, true, !checkKeuzelijsten.Checked);
            checkDeprecated.Enabled = deprecated;
            ListAllClasses.Items.Clear();
            foreach (string klasse in ApplicationHandler.GetSubsetClassNames())
            {
                ListAllClasses.Items.Add(klasse);           
            }
            buttonExportXLS.Enabled = true;
            TextVersion.Text = ApplicationHandler.GetOTLVersion();
            importDone = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkAllClasses.Checked)
            {
                ListAllClasses.Enabled = false;
            } else
            {
                ListAllClasses.Enabled=true;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.SubsetMain, null);
        }

        private void ListAllClasses_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if(checkVoorbeelddata.Checked)
            {
                checkAttributes.Enabled = true;
                numericUpDown1.Enabled=true;
            } else
            {
                checkAttributes.Enabled = false;
                checkAttributes.Checked=false;
                numericUpDown1.Enabled = false;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            
        }

        private void checkWKT_CheckedChanged(object sender, EventArgs e)
        {
            if(checkWKT.Checked)
            {
                buttonArtefact.Enabled = true;
                buttonExportXLS.Enabled = false;
            
            } else
            {
                buttonArtefact.Enabled = false;
                if(importDone)
                    buttonExportXLS.Enabled = true;
            }
                
              
        }

        private void buttonArtefact_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectartfiledlg");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxArtefact.Text = fdlg.FileName;
                if (importDone)
                    buttonExportXLS.Enabled = true;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
