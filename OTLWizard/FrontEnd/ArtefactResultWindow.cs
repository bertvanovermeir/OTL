using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class ArtefactResultWindow : Form
    {
        private ApplicationManager app;
        public ArtefactResultWindow(ApplicationManager app)
        {
            this.app = app;
            InitializeComponent();
        }

        private void ArtefactResultWindow_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = app.GetArtefactResultData().ToArray();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonExportArtefact_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
