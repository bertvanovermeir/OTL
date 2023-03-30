using System;
using System.Drawing;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class GeometryViewerWindow : Form
    {
        public GeometryViewerWindow()
        {
            InitializeComponent();
        }

        private void GeometryViewerWindow_Load(object sender, EventArgs e)
        {

        }

        private void Create_Geometry(object sender, PaintEventArgs e)
        {
            Pen pen1 = new Pen(Color.Black, 2);
            e.Graphics.DrawLine(pen1, this.Width / 2, 0, this.Width / 2, this.Height);
        }

        private void GeometryViewerWindow_Load_1(object sender, EventArgs e)
        {

        }
    }
}
