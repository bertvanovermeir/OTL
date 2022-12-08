using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.MDS;
using Microsoft.Msagl.Miscellaneous;
using Microsoft.Msagl.Prototype.Ranking;
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
        int relationFontSize = 6;
        Shape abstractNodeShape = Shape.Ellipse;
        Color abstractNodeColor = Color.LightBlue;
        Shape implementatieelementNodeShape = Shape.Box;
        Color implementatieelementNodeColor = Color.LightGreen;
        Shape defaultNodeShape = Shape.Box;
        Color defaultNodeColor = Color.LightSalmon;
        int defaultFontSize = 8;

        public SubsetViewerWindow()
        {
            InitializeComponent();
        }

        public void setOTLData(OTL_DataContainer container)
        {
            objects = container.ObjectTypes;
            relationshipTypes = container.RelationshipTypes;
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


        private void SubsetViewerWindow_Load(object sender, EventArgs e)
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
                if ((o.bronURI.Contains("abstracten#") || o.doelURI.Contains("abstracten#")) && drawAbstract)
                    drawRelation(o, true, false);
                else if ((o.bronURI.Contains("implementatieelement#") || o.doelURI.Contains("implementatieelement#")) && drawImplementatieElement)
                    drawRelation(o, false, true);
                else
                    drawRelation(o, false, false);
            }

            //bind the graph to the viewer 
            graph.LayoutAlgorithmSettings = new MdsLayoutSettings();
            graph.LayoutAlgorithmSettings.PackingMethod = Microsoft.Msagl.Core.Layout.PackingMethod.Columns;
            graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = Microsoft.Msagl.Core.Routing.EdgeRoutingMode.Rectilinear;
            viewer.Graph = graph;
            //associate the viewer with the form 
            SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            Controls.Add(viewer);
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
            ViewHandler.Show(Enums.Views.SubsetViewerImport, Enums.Views.SubsetViewer, null);
        }
    }
}

    
