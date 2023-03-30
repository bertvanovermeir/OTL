using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class RelationImportDataWindow : Form
    {
        private Dictionary<string, string[]> data = new Dictionary<string, string[]>();
        private string sessionDirectory = @"c:\";

        public RelationImportDataWindow()
        {
            InitializeComponent();
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }

        private void RelationImportDataWindow_Load(object sender, EventArgs e)
        {
            this.Text = Language.Get("relimportwindow");
            label1.Text = Language.Get("relmsg2");
            label2.Text = Language.Get("relmsg");
            checkBox1.Text = Language.Get("downotl");
        }

        public void ResetInterface()
        {
            data = new Dictionary<string, string[]>();
            textBox1.Text = "";
            textBox2.Text = "";
            checkBox1.Checked = false;
        }

        // cancel
        private void button2_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.RelationsImport, null);
        }

        // apply
        private void button1_Click(object sender, EventArgs e)
        {
            if (data.ContainsKey("files") && data.ContainsKey("subset"))
            {
                ViewHandler.Show(Enums.Views.RelationsMain, Enums.Views.RelationsImport, data);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = true;
            fdlg.Title = Language.Get("SelectDataFiles");
            fdlg.InitialDirectory = sessionDirectory;
            fdlg.Filter = "Data Files (*.csv;*.xls;*.xlsx;*.sdf)|*csv;*xls;*.xlsx;*.sdf";
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = String.Join("\r\n", fdlg.FileNames);
                if (data.ContainsKey("files"))
                {
                    data["files"] = fdlg.FileNames;
                }
                else
                {
                    data.Add("files", fdlg.FileNames);
                }
                if (fdlg.FileNames.Length > 0)
                    sessionDirectory = System.IO.Path.GetDirectoryName(fdlg.FileNames[0]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = false;
            fdlg.Title = Language.Get("SelectSubset");
            fdlg.InitialDirectory = sessionDirectory;
            fdlg.Filter = "OTL db Files (*.db)|*.db";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = fdlg.FileName;
                if (data.ContainsKey("subset"))
                {
                    data["subset"] = new string[] { fdlg.FileName };
                }
                else
                {
                    data.Add("subset", new string[] { fdlg.FileName });
                }
                sessionDirectory = System.IO.Path.GetDirectoryName(fdlg.FileName);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // do nothing
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.Enabled = false;
                if (data.ContainsKey("subset"))
                {
                    data["subset"] = new string[] { "download" };
                }
                else
                {
                    data.Add("subset", new string[] { "download" });
                }
            }
            else
            {
                textBox2.Enabled = true;
                if (data.ContainsKey("subset"))
                {
                    if (data["subset"].Equals("download"))
                    {
                        data.Remove("subset");
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
