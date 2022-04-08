using System;
using System.Linq;
using System.Windows.Forms;
using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;

namespace OTLWizard
{
    public partial class ExportArtefactWindow : Form
    {
        public ExportArtefactWindow()
        {
            InitializeComponent();
        }

        private void ExportArtefactWindow_Load(object sender, EventArgs e)
        {
            Text = Language.Get("exportartefactwindowheader");
            buttonSubset.Text = Language.Get("selectsubset");
            button2.Text = Language.Get("selectartefact");
            buttonImportClasses.Text = Language.Get("importclasses");
            label1.Text = Language.Get("eadescription");
            checkAllClasses.Text = Language.Get("checkallclasses");
            buttonExportArtefact.Text = Language.Get("exportartefact");
        }

        private void terug(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.ArtefactMain, null);
        }

        private void buttonSubset_Click(object sender, EventArgs e)
        {
            ListAllClasses.Items.Clear();
            buttonExportArtefact.Enabled = false;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectsubsetfiledlg");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxSubset.Text = fdlg.FileName;
                checkAllClasses.Enabled = true;
                ListAllClasses.Enabled = true;
            }
        }

        private void buttonArtefact_Click(object sender, EventArgs e)
        {
            ListAllClasses.Items.Clear();
            buttonExportArtefact.Enabled = false;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectartfiledlg");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxArtefact.Text = fdlg.FileName;
                buttonImportClasses.Enabled = true;
                checkAllClasses.Enabled = true;
                ListAllClasses.Enabled = true;
            }
        }

        private async void buttonImportClasses_Click(object sender, EventArgs e)
        {
            await ApplicationHandler.ImportArtefact(textBoxSubset.Text, textBoxArtefact.Text);
            ListAllClasses.Items.Clear();
            foreach (string klasse in ApplicationHandler.GetSubsetClassNames())
            {
                ListAllClasses.Items.Add(klasse);

            }
            buttonExportArtefact.Enabled = true;
        }

        private void buttonExportArtefact_Click(object sender, EventArgs e)
        {
            string[] temp = null;
            if(checkAllClasses.Checked)
            {
                temp = new string[0];
            } else
            {
                if(ListAllClasses.SelectedIndices.Count == 0)
                {
                    // temp will be null, application will halt further execution
                } else
                {
                    temp = new string[ListAllClasses.SelectedIndices.Count];
                    int i = 0;
                    foreach (int ind in ListAllClasses.SelectedIndices)
                    {

                        temp[i] = ListAllClasses.Items[ind].ToString();
                        i++;
                    }
                }               
            }
            if(temp != null)
            {
                // open the result window and pass the results of the user selection in the optionalArgument parameter
                ViewHandler.Show(Enums.Views.ArtefactResult, Enums.Views.ArtefactMain, temp.ToList<string>());
            } else
            {
                ViewHandler.Show(Language.Get("noselection"), Language.Get("errorheader"), MessageBoxIcon.Error);
            }           
        }

        private void checkAllClasses_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAllClasses.Checked)
            {
                ListAllClasses.Enabled = false;
            }
            else
            {
                ListAllClasses.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ListAllClasses_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
