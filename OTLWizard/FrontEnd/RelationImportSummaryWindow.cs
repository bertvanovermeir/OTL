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
    public partial class RelationImportSummaryWindow : Form
    {
        public RelationImportSummaryWindow()
        {
            InitializeComponent();
        }

        private void RelationImportSummaryWindow_Load(object sender, EventArgs e)
        {
            this.Text = Language.Get("importsummaryheader");
        }

        public void SetDataSource(object source)
        {
            dataGridView1.DataSource = source;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.RelationImportSummary, null);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
