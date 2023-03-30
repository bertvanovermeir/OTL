using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.MDS;
using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Color = Microsoft.Msagl.Drawing.Color;

namespace OTLWizard.FrontEnd
{
    public partial class SubsetViewerWindow : Form
    {
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        Microsoft.Msagl.Drawing.Graph graph;

        List<OTL_ObjectType> objects = new List<OTL_ObjectType>();
        List<OTL_RelationshipType> relationshipTypes = new List<OTL_RelationshipType>();

        bool drawAbstract = true;
        bool drawImplementatieElement = true;
        Color colorAbstractRelation = Microsoft.Msagl.Drawing.Color.Pink;
        Color colorImplementatieElementRelation = Microsoft.Msagl.Drawing.Color.Gray;
        int relationFontSize = 10;
        Shape abstractNodeShape = Shape.Ellipse;
        Color abstractNodeColor = Color.LightBlue;
        Shape implementatieelementNodeShape = Shape.Box;
        Color implementatieelementNodeColor = Color.LightGreen;
        Shape defaultNodeShape = Shape.Box;
        Color defaultNodeColor = Color.LightSalmon;
        int defaultFontSize = 12;

        public SubsetViewerWindow()
        {
            InitializeComponent();
            initColorComponents();
            initShapeComponents();
            SetBaseValues();
        }

        private void SetBaseValues()
        {
            checkBox1.Checked = drawAbstract;
            checkBox2.Checked = drawImplementatieElement;
            comboBox7.SelectedItem = "Pink";
            comboBox8.SelectedItem = "Gray";
            numericUpDown1.Value = 10;
            comboBox6.SelectedItem = "Ellipse";
            comboBox1.SelectedItem = "LightBlue";
            comboBox5.SelectedItem = "Box";
            comboBox2.SelectedItem = "LightGreen";
            comboBox4.SelectedItem = "Box";
            comboBox3.SelectedItem = "LightSalmon";
            numericUpDown2.Value = 12;
        }

        public void setOTLData()
        {
            objects = ApplicationHandler.VWR_GetSubset();
            relationshipTypes = ApplicationHandler.VWR_GetRelationTypes();
        }

        public void loadOTLData()
        {
            graph = new Microsoft.Msagl.Drawing.Graph("graph");

            foreach (OTL_ObjectType o in objects)
            {
                drawOTLClass(o);
            }

            relationshipTypes = makeRelationUnique();

            foreach (OTL_RelationshipType o in relationshipTypes)
            {
                // start drawing process
                if (o.isAbstract && drawAbstract)
                    drawRelation(o, true, false);
                else if (o.isImplementatieElement && drawImplementatieElement)
                    drawRelation(o, false, true);
                else if (!o.isAbstract && !o.isImplementatieElement)
                    drawRelation(o, false, false);
            }
        }

        public void initShapeComponents()
        {
            var shapeobj = typeof(Shape);
            var shapearr = shapeobj.GetFields().ToArray().Where(x => !x.Name.Equals("value__")).Select(o => o.Name);

            comboBox4.Items.AddRange(shapearr.ToArray());
            comboBox5.Items.AddRange(shapearr.ToArray());
            comboBox6.Items.AddRange(shapearr.ToArray());
        }

        public void initColorComponents()
        {
            var colorobj = typeof(Color);
            var colorarr = colorobj.GetProperties().ToArray().Where(x => x.Name.Length > 1).Select(o => o.Name);

            comboBox1.Items.AddRange(colorarr.ToArray());
            comboBox2.Items.AddRange(colorarr.ToArray());
            comboBox3.Items.AddRange(colorarr.ToArray());
            comboBox7.Items.AddRange(colorarr.ToArray());
            comboBox8.Items.AddRange(colorarr.ToArray());
        }

        private List<OTL_RelationshipType> makeRelationUnique()
        {
            bool found2 = false;
            List<OTL_RelationshipType> cleanedList = new List<OTL_RelationshipType>();
            foreach (OTL_RelationshipType a in relationshipTypes)
            {
                found2 = false;
                foreach (OTL_RelationshipType b in cleanedList)
                {
                    if (a.bronURI.Equals(b.doelURI) && a.doelURI.Equals(b.bronURI) && a.relationshipURI.Equals(b.relationshipURI))
                    {
                        // it already exists
                        found2 = true;
                    }
                }
                if (!found2)
                {
                    cleanedList.Add(a);
                }
            }
            return cleanedList;
        }

        private void updateLayouter()
        {
            //bind the graph to the viewer 
            graph.LayoutAlgorithmSettings = new MdsLayoutSettings();
            graph.LayoutAlgorithmSettings.PackingMethod = Microsoft.Msagl.Core.Layout.PackingMethod.Columns;
            graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = Microsoft.Msagl.Core.Routing.EdgeRoutingMode.Rectilinear;
            viewer.Graph = graph;
            viewer.Update();
        }


        private void SubsetViewerWindow_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            groupBox2.Enabled = false;
            button3.Enabled = false;
            // text
            this.Text = Language.Get("subsetviewerheader");



            graph = new Microsoft.Msagl.Drawing.Graph("graph");
            graph.AddNode(Language.Get("laadeenproject"));

            //bind the graph to the viewer 
            graph.LayoutAlgorithmSettings = new MdsLayoutSettings();
            graph.LayoutAlgorithmSettings.PackingMethod = Microsoft.Msagl.Core.Layout.PackingMethod.Columns;
            graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = Microsoft.Msagl.Core.Routing.EdgeRoutingMode.Rectilinear;
            viewer.Graph = graph;
            //associate the viewer with the form 
            SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(viewer);
            ResumeLayout();
        }

        private void drawOTLClass(OTL_ObjectType otl)
        {


            var node = graph.AddNode(otl.uri);
            node.LabelText = otl.otlName;
            node.Label.FontSize = defaultFontSize;
            setGraphicalRepresentation(node, defaultNodeColor, defaultNodeShape);
        }



        private void drawRelation(OTL_RelationshipType otl, bool isAbstract, bool isImplementatieElement)
        {
            // warning: this method auto generates abstract and implementatieelement objects
            var relation = graph.AddEdge(otl.bronURI, otl.doelURI);
            relation.LabelText = otl.relationshipName;
            relation.Label.FontSize = relationFontSize;
            if (!otl.isDirectional)
            {
                relation.Attr.ArrowheadAtSource = ArrowStyle.None;
                relation.Attr.ArrowheadAtTarget = ArrowStyle.None;
            }
            if (isAbstract)
            {
                relation.Attr.Color = colorAbstractRelation;
                checkLabel(otl.doelURI, true);
                checkLabel(otl.bronURI, true);
            }
            if (isImplementatieElement)
            {
                relation.Attr.Color = colorImplementatieElementRelation;
                checkLabel(otl.doelURI, false);
                checkLabel(otl.bronURI, false);
            }
        }

        private void checkLabel(string id, bool isAbstract)
        {
            // check if labels are correctly generated for special (abstract classes)
            var bron = graph.FindNode(id);
            if (bron.LabelText.Contains("#"))
            {
                bron.LabelText = bron.LabelText.Split('#')[1];
                bron.Label.FontSize = defaultFontSize;
                if (isAbstract)
                    setGraphicalRepresentation(bron, abstractNodeColor, abstractNodeShape);
                if (!isAbstract)
                    setGraphicalRepresentation(bron, implementatieelementNodeColor, implementatieelementNodeShape);
            }
        }

        private void setGraphicalRepresentation(Node node, Microsoft.Msagl.Drawing.Color fill, Shape shape)
        {
            node.Attr.FillColor = fill;
            node.Attr.Shape = shape;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.SubsetViewer, null);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var colors = typeof(Color).GetProperties();
            var shapes = typeof(Shape).GetFields();

            drawAbstract = checkBox1.Checked;
            drawImplementatieElement = checkBox2.Checked;
            colorAbstractRelation = (Color)colors.Where(c => c.Name.Equals(comboBox7.SelectedItem)).FirstOrDefault().GetValue(null);
            colorImplementatieElementRelation = (Color)colors.Where(c => c.Name.Equals(comboBox8.SelectedItem)).FirstOrDefault().GetValue(null);
            relationFontSize = Decimal.ToInt32(numericUpDown1.Value);
            abstractNodeShape = (Shape)shapes.Where(c => c.Name.Equals(comboBox6.SelectedItem)).FirstOrDefault().GetValue(null);
            abstractNodeColor = (Color)colors.Where(c => c.Name.Equals(comboBox1.SelectedItem)).FirstOrDefault().GetValue(null);
            implementatieelementNodeShape = (Shape)shapes.Where(c => c.Name.Equals(comboBox5.SelectedItem)).FirstOrDefault().GetValue(null);
            implementatieelementNodeColor = (Color)colors.Where(c => c.Name.Equals(comboBox2.SelectedItem)).FirstOrDefault().GetValue(null);
            defaultNodeShape = (Shape)shapes.Where(c => c.Name.Equals(comboBox4.SelectedItem)).FirstOrDefault().GetValue(null);
            defaultNodeColor = (Color)colors.Where(c => c.Name.Equals(comboBox3.SelectedItem)).FirstOrDefault().GetValue(null);
            defaultFontSize = Decimal.ToInt32(numericUpDown2.Value);

            setOTLData();
            loadOTLData();
            updateLayouter();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectsubsetfiledlg");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "Database Files (*.db)|*.db|Database Files (*.db)|*.db";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fdlg.FileName;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ApplicationHandler.VWR_ImportSubset(textBox1.Text);
            button2.Enabled = true;
            groupBox2.Enabled = true;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}


