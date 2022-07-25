using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
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
    public partial class RelationImportDataWindow : Form
    {
        private Dictionary<string,string[]> data = new Dictionary<string, string[]>();

        public RelationImportDataWindow()
        {
            InitializeComponent();
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }

        private void RelationImportDataWindow_Load(object sender, EventArgs e)
        {

        }

        public void ResetInterface()
        {
            data = new Dictionary<string,string[]>();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        // cancel
        private void button2_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.RelationsImport, null);
        }

        // apply
        private void button1_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Relations, Enums.Views.RelationsImport, data);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = true;
            fdlg.Title = Language.Get("SelectDataFiles");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "CSV Files (*.csv)|*.csv|JSON Files (*.json)|*.json|Excel Files (*.xlsx)|*.xlsx";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = String.Join("\r\n",fdlg.FileNames);
                data.Add("files", fdlg.FileNames);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = false;
            fdlg.Title = Language.Get("SelectSubset");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "OTL db Files (*.db)|*.db";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = fdlg.FileName;
                data.Add("subset", new string[] { fdlg.FileName });
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // do nothing
        }
    }
}
