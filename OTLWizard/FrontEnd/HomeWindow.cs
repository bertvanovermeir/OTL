using System;
using System.Diagnostics;
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("https://wegenenverkeer.data.vlaanderen.be/");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("https://apps.mow.vlaanderen.be/davie-aanlevering/");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("https://sites.google.com/mow.vlaanderen.be/davie-aanlevering/startpagina?authuser=0");
        }
    }
}
