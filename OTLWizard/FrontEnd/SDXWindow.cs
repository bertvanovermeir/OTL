using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class SDXWindow : Form
    {
        public SDXWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.SDFMain, null);

        }

        private void radioButtonSDXModeNew_CheckedChanged(object sender, EventArgs e)
        {

            buttonSubset.Enabled = radioButtonSDXModeNew.Checked;
            buttonSDX.Enabled = !radioButtonSDXModeNew.Checked;
            buttonartefact.Enabled = radioButtonSDXModeNew.Checked;
            setStatusComponents(false);
        }

        private void radioButtonSDXModeEdit_CheckedChanged(object sender, EventArgs e)
        {
            buttonSubset.Enabled = !radioButtonSDXModeEdit.Checked;
            buttonSDX.Enabled = radioButtonSDXModeEdit.Checked;
            buttonartefact.Enabled = !radioButtonSDXModeEdit.Checked;
            setStatusComponents(false);
        }

        private void setStatusComponents(bool enabled)
        {
            textBoxSubset.Text = "";
            textBoxSDX.Text = "";
            textBoxArtefact.Text = "";

            buttonImportAll.Enabled = enabled;
            checkAllClasses.Enabled = enabled;
            ListAllClasses.Enabled = enabled;
            buttonExportSDX.Enabled = false;
            checkAllClasses.Enabled = false;
            ListAllClasses.Enabled = false;

        }

        private void buttonSubset_Click(object sender, EventArgs e)
        {
            ListAllClasses.Items.Clear();
            buttonExportSDX.Enabled = false;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectsubsetfiledlg");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                setStatusComponents(true);
                textBoxSubset.Text = fdlg.FileName;
            }
        }

        private void buttonSDX_Click(object sender, EventArgs e)
        {
            ListAllClasses.Items.Clear();
            buttonExportSDX.Enabled = false;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectsdxfiledialog");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "XSD Files (*.xsd)|*.xsd|XSD Files (*.xsd)|*.xsd";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                setStatusComponents(true);
                textBoxSDX.Text = fdlg.FileName;
            }
        }

        private void buttonExportSDX_Click(object sender, EventArgs e)
        {
            // execute the export
            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = Language.Get("sdxexport");
            fdlg.FileName = "xsd" + "_export";
            fdlg.Filter = "XSD files (*.xsd)|*.xsd|XSD files (*.xsd)|*.xsd";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                string[] temp = null;
                if (ListAllClasses.Enabled == true)
                {
                    temp = new string[ListAllClasses.SelectedIndices.Count];
                    int i = 0;
                    foreach (int ind in ListAllClasses.SelectedIndices)
                    {
                        temp[i] = ListAllClasses.Items[ind].ToString();
                        i++;
                    }
                }
                if (radioButtonSDXModeEdit.Checked)
                    ApplicationHandler.SDX_ExportSDX(fdlg.FileName, temp);
                if (radioButtonSDXModeNew.Checked)
                    ApplicationHandler.SDX_ExportSDX(fdlg.FileName, temp);
            }
        }

        private async void buttonImportAll_Click(object sender, EventArgs e)
        {
            if (radioButtonSDXModeEdit.Checked)
            {
                ApplicationHandler.SDX_ImportSDX(textBoxSDX.Text, true);
                ListAllClasses.Items.Clear();
                foreach (string klasse in ApplicationHandler.getXSDClassNames())
                {
                    ListAllClasses.Items.Add(klasse);
                }
            }
            else // mode new SDX
            {
                await ApplicationHandler.ImportSubset(textBoxSubset.Text, true, true);
                await ApplicationHandler.ImportArtefact(textBoxArtefact.Text);
                ListAllClasses.Items.Clear();
                foreach (string klasse in ApplicationHandler.GetSubsetClassNames())
                {
                    ListAllClasses.Items.Add(klasse);
                }
                TextVersion.Text = ApplicationHandler.GetOTLVersion();
                buttonExportSDX.Enabled = true;
            }
            checkAllClasses.Enabled = true;
            ListAllClasses.Enabled = true;
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

        private void buttonartefact_Click(object sender, EventArgs e)
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
                checkAllClasses.Enabled = true;
                ListAllClasses.Enabled = true;

            }
        }

        private void SDXWindow_Load(object sender, EventArgs e)
        {
            this.Text = Language.Get("sdfwizardheader");
            buttonartefact.Text = Language.Get("buttonartefacttext");
            buttonSDX.Text = Language.Get("buttonsdxtext");
            buttonSubset.Text = Language.Get("buttonsubsettext");
            buttonImportAll.Text = Language.Get("buttonimportall");
            buttonExportSDX.Text = Language.Get("buttonexportsdx");
            label1.Text = Language.Get("labelexplanationsdf");
            label4.Text = Language.Get("label4sdf");
            label7.Text = Language.Get("label7sdf");
            radioButtonSDXModeEdit.Text = Language.Get("radiobuttonsdxedit");
            radioButtonSDXModeNew.Text = Language.Get("radiobuttonsdxnew");
            label2.Text = Language.Get("label2sdf");
        }
    }
}
