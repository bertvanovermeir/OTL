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
