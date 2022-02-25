﻿using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class ArtefactResultWindow : Form
    {
        private List<string> userSelection;
        public ArtefactResultWindow()
        {
            userSelection = new List<string>();
            InitializeComponent();
        }

        public void SetUserSelection(List<string> otlClasses)
        {
            userSelection = otlClasses;
        }

        private void ArtefactResultWindow_Load(object sender, EventArgs e)
        {
            List<OTL_ArtefactType> results = ApplicationHandler.GetArtefactResultData();
            List<OTL_ArtefactType> selected = new List<OTL_ArtefactType>();

            if(userSelection.Count > 0)
            {
                foreach (string item in userSelection)
                {
                    foreach (OTL_ArtefactType oTL_ArtefactType in results)
                    {
                        if (oTL_ArtefactType.URL.Contains(item))
                        {
                            selected.Add(oTL_ArtefactType);
                        }
                    }
                }
            } else
            {
                selected = results;
            }          
            dataGridView1.DataSource = selected.ToArray();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void buttonExportArtefact_Click(object sender, EventArgs e)
        {
            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = "Sla Data op^naar Excel of CSV";
            fdlg.Filter = "Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                List<OTL_ArtefactType> results = ApplicationHandler.GetArtefactResultData();
                List<OTL_ArtefactType> selected = new List<OTL_ArtefactType>();

                if (userSelection.Count > 0)
                {
                    foreach (string item in userSelection)
                    {
                        foreach (OTL_ArtefactType oTL_ArtefactType in results)
                        {
                            if (oTL_ArtefactType.URL.Contains(item))
                            {
                                selected.Add(oTL_ArtefactType);
                            }
                        }
                    }
                }
                else
                {
                    selected = results;
                }
                if (fdlg.FilterIndex == 1)
                {
                    await ApplicationHandler.exportXlsArtefact(fdlg.FileName, selected);
                }
                else
                {
                    await ApplicationHandler.exportCSVArtefact(fdlg.FileName, selected);
                }
            }                
        }

        private void buttonExit(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.ArtefactResult, null);
        }
    }
}
