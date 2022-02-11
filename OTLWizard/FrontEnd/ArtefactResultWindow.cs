using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class ArtefactResultWindow : Form
    {
        private ApplicationManager app;
        private List<string> userSelection;
        public ArtefactResultWindow(ApplicationManager app)
        {
            this.app = app;
            userSelection = new List<string>();
            InitializeComponent();
        }

        public void SetUserSelection(List<string> otlClasses)
        {
            userSelection = otlClasses;
        }

        private void ArtefactResultWindow_Load(object sender, EventArgs e)
        {
            List<OTL_ArtefactType> results = app.GetArtefactResultData();
            List<OTL_ArtefactType> selected = new List<OTL_ArtefactType>();

            if(userSelection.Count > 0)
            {
                foreach (string item in userSelection)
                {
                    foreach (OTL_ArtefactType oTL_ArtefactType in results)
                    {
                        if (oTL_ArtefactType.objectnaam.Equals(item))
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

        private void buttonExportArtefact_Click(object sender, EventArgs e)
        {
            //
        }

        private void buttonExit(object sender, EventArgs e)
        {
            app.openView(Enums.Views.Home, Enums.Views.ArtefactResult, null);
        }
    }
}
