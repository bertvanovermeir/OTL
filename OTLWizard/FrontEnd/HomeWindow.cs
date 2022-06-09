using System;
using System.Diagnostics;
using System.Windows.Forms;
using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;

namespace OTLWizard
{
    public partial class HomeWindow : Form
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void HomeWindow_Load(object sender, EventArgs e)
        {
            Text = Language.Get("homewindowheader");
            label3.Text = Language.Get("homewindowheader") + " - " + Language.Get("changelanguage");
            button1.Text = Language.Get("home1");
            button3.Text = Language.Get("home3");
            button2.Text = Language.Get("home2");
            button4.Text = Language.Get("home4");
            button5.Text = Language.Get("home5");
            button6.Text = Language.Get("home6");
            label1.Text = Language.Get("welcome");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void xlsExport(object sender, EventArgs e)
        {

            ViewHandler.Show(Enums.Views.SubsetMain, Enums.Views.Home, null);
        }

        private void artefactExport(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.ArtefactMain, Enums.Views.Home, null);
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

        private void button6_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Settings, Enums.Views.Home, null);
        }
    }
}
