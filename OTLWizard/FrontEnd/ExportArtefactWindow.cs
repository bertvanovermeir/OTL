using System;
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

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void buttonImportClasses_Click(object sender, EventArgs e)
        {

        }

        private void buttonExportArtefact_Click(object sender, EventArgs e)
        {

        }

        private void checkAllClasses_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
