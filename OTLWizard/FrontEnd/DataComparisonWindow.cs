using OTLWizard.Helpers;
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
    public partial class DataComparisonWindow : Form
    {

        private Dictionary<string, string[]> data = new Dictionary<string, string[]>();
        private bool legeKolommen = false;
        private bool featid = false;

        public DataComparisonWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplicationHandler.C_ResetCompare();
            ViewHandler.Show(Enums.Views.Home, Enums.Views.DataComparison, null);
        }

        private void DataComparisonWindow_Load(object sender, EventArgs e)
        {
            this.Text = Language.Get("compareheader");
            checkBoxLegeKolommen.Text = Language.Get("noemptycolumns");
            checkBoxLegeKolommen.Checked = !legeKolommen;
            checkBoxFeatid.Checked = featid;
            checkBoxFeatid.Text = Language.Get("featid");
            label1.Text = Language.Get("comparedescription");
            labelStatus.Text = Language.Get("waitforuser");
            
        }

        private void buttonOrigineleBestanden_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = true;
            fdlg.Title = Language.Get("SelectDataFiles");            
            fdlg.Filter = "Data Files (*.csv;*.xls;*.xlsx;*.sdf)|*csv;*xls;*.xlsx;*.sdf";
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxOrigineleBestanden.Text = String.Join(";", fdlg.FileNames);
                if (data.ContainsKey("filesoriginal"))
                {
                    data["filesoriginal"] = fdlg.FileNames;
                }
                else
                {
                    data.Add("filesoriginal", fdlg.FileNames);
                }
                buttonNieuweBestanden.Enabled = true;
            } else
            {
                buttonNieuweBestanden.Enabled=false;
                buttonControleUitvoeren.Enabled = false;
                buttonExporteerBestanden.Enabled = false;
            }
        }

        private void buttonNieuweBestanden_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = true;
            fdlg.Title = Language.Get("SelectDataFiles");
            fdlg.Filter = "Data Files (*.csv;*.xls;*.xlsx;*.sdf)|*csv;*xls;*.xlsx;*.sdf";
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textboxNieuweBestanden.Text = String.Join(";", fdlg.FileNames);
                if (data.ContainsKey("filesnew"))
                {
                    data["filesnew"] = fdlg.FileNames;
                }
                else
                {
                    data.Add("filesnew", fdlg.FileNames);
                }
                buttonControleUitvoeren.Enabled=true;
            } else
            {
                buttonControleUitvoeren.Enabled = false;
                buttonExporteerBestanden.Enabled = false;
            }
        }

        private async void buttonControleUitvoeren_Click(object sender, EventArgs e)
        {
            await ApplicationHandler.C_ImportData(data["filesoriginal"], true);
            await ApplicationHandler.C_ImportData(data["filesnew"], false);
            var result = await ApplicationHandler.C_CompareData();
            if(result)
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.DataSource = ApplicationHandler.C_GetDataCompareResults();
                buttonExporteerBestanden.Enabled = true;
                labelStatus.Text = ApplicationHandler.C_GetStatusImport();
            }               
        }

        private void buttonExporteerBestanden_Click(object sender, EventArgs e)
        {
            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = Language.Get("arexport");
            fdlg.FileName = "cleanedOTLdata";
            fdlg.Filter = "CSV files (*.csv)|*.csv";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                ApplicationHandler.C_ExportData(fdlg.FileName, legeKolommen, featid);
                labelStatus.Text = "Bestanden geëxporteerd...";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void checkBoxLegeKolommen_CheckedChanged(object sender, EventArgs e)
        {
            legeKolommen = !checkBoxLegeKolommen.Checked;
        }

        private void checkBoxFeatid_CheckedChanged(object sender, EventArgs e)
        {
            featid = !checkBoxFeatid.Checked;
        }
    }
}
