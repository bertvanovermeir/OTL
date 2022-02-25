using System;
using System.Diagnostics;
using System.Windows.Forms;
using OTLWizard.OTLObjecten;

namespace OTLWizard
{
    public partial class ExportSubsetWindow : Form
    {
        public ExportSubsetWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

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
            fdlg.Title = "Selecteer een subset";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxSubset.Text = fdlg.FileName;
                buttonImportClasses.Enabled = true;
                checkAllClasses.Enabled = true;
                checkAttributes.Enabled = true;
                checkKeuzelijsten.Enabled = true;
                ListAllClasses.Enabled = true;
                buttonImportClasses.Enabled = true;
            }
        }

        private async void Export(object sender, EventArgs e)
        {
            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = "Sla Data op naar Excel of CSV";
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
                    await ApplicationHandler.exportXlsSubset(fdlg.FileName, checkAttributes.Checked, !checkKeuzelijsten.Checked, temp);
                }
                else
                {
                    await ApplicationHandler.exportCSVSubset(fdlg.FileName, checkAttributes.Checked, !checkKeuzelijsten.Checked, temp);
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
            
            await ApplicationHandler.ImportSubset(textBoxSubset.Text, !checkKeuzelijsten.Checked);

            ListAllClasses.Items.Clear();
            foreach(string klasse in ApplicationHandler.GetSubsetClassNames())
            {
                ListAllClasses.Items.Add(klasse);
                
            }
            buttonExportXLS.Enabled = true;
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
    }
}
