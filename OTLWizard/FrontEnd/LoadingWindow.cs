using System;
using System.Windows.Forms;
using OTLWizard.ApplicationData;

namespace OTLWizard
{
    public partial class LoadingWindow : Form
    {
        public LoadingWindow(ApplicationManager app)
        {
            InitializeComponent();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void progressLabel_Click(object sender, EventArgs e)
        {

        }

        public void SetProgressLabelText(string text)
        {
            progressLabel.Text = text;
        }

        private void progressBar1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
