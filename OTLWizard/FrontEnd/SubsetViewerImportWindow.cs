using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class SubsetViewerImportWindow : Form
    {
        public SubsetViewerImportWindow()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectsubsetfiledlg");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = fdlg.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.SubsetViewerImport, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplicationHandler.VWR_ImportSubset(textBox2.Text);
        }

        private void SubsetViewerImportWindow_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.SubsetViewerImport, null);
        }
    }
}
