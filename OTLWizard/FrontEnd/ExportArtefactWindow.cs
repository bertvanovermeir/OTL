﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OTLWizard
{
    public partial class ExportArtefactWindow : Form
    {
        private ApplicationManager app;
        public ExportArtefactWindow(ApplicationManager app)
        {
            this.app = app;
            InitializeComponent();
        }

        private void ExportArtefactWindow_Load(object sender, EventArgs e)
        {

        }

        private void terug(object sender, EventArgs e)
        {
            app.showHome(this);
        }

        private void buttonSubset_Click(object sender, EventArgs e)
        {
            ListAllClasses.Items.Clear();
            buttonExportArtefact.Enabled = false;
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Selecteer een subset";
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
            fdlg.Title = "Selecteer een artefactdatabase";
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
            await app.ImportArtefact(textBoxSubset.Text, textBoxArtefact.Text);
            foreach (string klasse in app.GetOTLClassNames())
            {
                ListAllClasses.Items.Add(klasse);

            }
            buttonExportArtefact.Enabled = true;
        }

        private void buttonExportArtefact_Click(object sender, EventArgs e)
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
            app.showArtefactResult();
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
    }
}
