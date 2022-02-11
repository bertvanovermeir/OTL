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
    public partial class HomeWindow : Form
    {
        private ApplicationManager app;
        public HomeWindow(ApplicationManager app)
        {
            this.app = app;
            InitializeComponent();
        }

        private void HomeWindow_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void xlsExport(object sender, EventArgs e)
        {
            app.openView(Enums.Views.SubsetMain, Enums.Views.Home, null);
        }

        private void artefactExport(object sender, EventArgs e)
        {
            app.openView(Enums.Views.ArtefactMain, Enums.Views.Home, null);
        }
    }
}
