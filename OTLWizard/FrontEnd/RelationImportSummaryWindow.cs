using OTLWizard.Helpers;
using OTLWizard.Helpers;
using System;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class RelationImportSummaryWindow : Form
    {
        private string usecase;

        public RelationImportSummaryWindow(string usecase)
        {
            this.usecase = usecase;
            InitializeComponent();
        }

        private void RelationImportSummaryWindow_Load(object sender, EventArgs e)
        {
            this.Text = Language.Get(usecase);
        }

        public void SetDataSource(object source)
        {
            dataGridView1.DataSource = source;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(usecase.Equals("importsummaryheader"))
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.RelationImportSummary, null);
            else if(usecase.Equals("comparesummaryheader"))
            {
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.RelationImportSummary, null);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
