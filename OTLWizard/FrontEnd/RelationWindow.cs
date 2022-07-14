using Dataweb.NShape;
using Dataweb.NShape.Advanced;
using Dataweb.NShape.GeneralShapes;
using Dataweb.NShape.Layouters;
using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class RelationWindow : Form
    {

        Diagram diagram;

        public RelationWindow()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RelationWindow_Load(object sender, EventArgs e)
        {
            project1.Name = "Visual";
            project1.AddLibrary(typeof(Ellipse).Assembly, false);
            var temp = project1.ShapeTypes;
            project1.AutoLoadLibraries = true;
            project1.Create();            
            display1.ActiveTool = new SelectionTool();
        }

        private void NShapeLayouter()
        {
            RepulsionLayouter layouter = new RepulsionLayouter(project1);
            // Set the repulsion force and its range
            layouter.SpringRate = 8;
            layouter.Repulsion = 3;
            layouter.RepulsionRange = 500;
            // Set the friction and the mass of the shapes
            layouter.Friction = 0;
            layouter.Mass = 50;
            // Set all shapes 
            layouter.AllShapes = display1.Diagram.Shapes;
            // Set shapes that should be layouted
            layouter.Shapes = display1.Diagram.Shapes;
            //
            // Now prepare and execute the layouter
            layouter.Prepare();
            layouter.Execute(10);
            // Fit the result into the diagram bounds
            layouter.Fit(50, 50, display1.Diagram.Width - 100, display1.Diagram.Height - 100);
            cachedRepository1.InsertAll(diagram);           
        }

        private Shape NShapeCreateNode(string NodeText)
        {
            RectangleBase shape;
            
            shape = (RectangleBase)project1.ShapeTypes["Ellipse"].CreateInstance();
            shape.Width = 100;
            shape.Height = 60;
            // Set position of the shape (diagram coordinates)
            shape.X = 100;
            shape.Y = 100;
            // Set text of the shape
            shape.SetCaptionText(0, NodeText);
            // Add shape to the diagram
            diagram.Shapes.Add(shape);
            display1.Diagram = diagram;          
            return shape;
        }

        private void NShapeConnectNode(Shape referredShape, Shape referringShape, string relationName, bool isDirectional)
        {
            // Create a line shape for connecting the two shapes
            Polyline arrow = (Polyline)project1.ShapeTypes["Polyline"].CreateInstance();
            // Add shape to the diagram
            diagram.Shapes.Add(arrow);
            // Connect one of the line shape's endings (first vertex) to the referring shape's reference point
            arrow.Connect(ControlPointId.FirstVertex, referringShape, ControlPointId.Reference);
            // Connect the other of the line shape's endings (last vertex) to the referred shape
            arrow.Connect(ControlPointId.LastVertex, referredShape, ControlPointId.Reference);
            Guid guid = Guid.NewGuid();
            CapStyle style1 = new CapStyle(guid.ToString());
            style1.CapShape = CapShape.ClosedArrow;
            style1.CapSize = 25;
            style1.ColorStyle = new ColorStyle();           
            if(isDirectional)
            {               
                if(relationName.Equals("LigtOp"))
                    style1.ColorStyle = project1.Design.ColorStyles.Green;
                if (relationName.Equals("Voedt"))
                    style1.ColorStyle = project1.Design.ColorStyles.Blue;
                if (relationName.Equals("Bevestiging"))
                    style1.ColorStyle = project1.Design.ColorStyles.Red;
                if (relationName.Equals("HoortBij"))
                    style1.ColorStyle = project1.Design.ColorStyles.White;
                if (relationName.Equals("SluitAanOp"))
                    style1.ColorStyle = project1.Design.ColorStyles.Gray;
                if (relationName.Equals("Sturing"))
                    style1.ColorStyle = project1.Design.ColorStyles.Yellow;
                if (relationName.Equals("HeeftAanvullendeGeometrie"))
                    style1.ColorStyle = project1.Design.ColorStyles.LightGreen;
                if (relationName.Equals("HeeftBetrokkene"))
                    style1.ColorStyle = project1.Design.ColorStyles.LightBlue;
                if (relationName.Equals("HeeftBeheer"))
                    style1.ColorStyle = project1.Design.ColorStyles.LightYellow;
                arrow.EndCapStyle = style1;
            }
            project1.Design.CapStyles.Add(style1, project1.Design.CreatePreviewStyle(style1));
        }



        private void display1_Load(object sender, EventArgs e)
        {

        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Multiselect = true;
            fdlg.Title = Language.Get("SelectDataFiles");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "CSV Files (*.csv)|*.csv|JSON Files (*.json)|*.json|Excel Files (*.xlsx)|*.xlsx";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                ApplicationHandler.R_ImportRealRelationData(fdlg.FileNames);
                listBox1.Items.AddRange(ApplicationHandler.R_GetImportedEntities().ToArray());
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = Language.Get("SaveState");
            fdlg.FileName = "NAMEHERE";
            fdlg.Filter = "CSV files (*.csv)|*.csv";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                ApplicationHandler.R_ExportRealRelationData(fdlg.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo: check if file was already saved this session, then dont execute the savefiledialog

            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = Language.Get("SaveState");
            fdlg.FileName = "NAMEHERE";
            fdlg.Filter = "XMLR files (*.xmlr)|*.xmlr";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                ApplicationHandler.R_SaveRelationState(fdlg.FileName);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.Home, Enums.Views.Relations, null);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewHandler.Show("Developed by BVO for AWV", "About", MessageBoxIcon.Information);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("SelectSaveState");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "XMLR Files (*.xmlr)|*.xmlr";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                ApplicationHandler.R_LoadRelationState(fdlg.FileName);
            }
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Guid guid = Guid.NewGuid();
            diagram = new Diagram(guid.ToString());
            Shape s2 = NShapeCreateNode("OTLklasse1");
            Shape s1 = NShapeCreateNode("OTLklasse2");
            Shape s3 = NShapeCreateNode("OTLklasse3");
            Shape s4 = NShapeCreateNode("OTLklasse4");
            Shape s5 = NShapeCreateNode("OTLklasse5");
            NShapeConnectNode(s1, s2, "Voedt", true);
            NShapeConnectNode(s2, s3, "HeeftBetrokkene", true);
            NShapeConnectNode(s2, s3, "Sturing", true);
            NShapeLayouter();
        }

        private void statuslabel_Click(object sender, EventArgs e)
        {
            // nothing
        }

        // add relation
        private void button1_Click(object sender, EventArgs e)
        {
            ApplicationHandler.R_CreateNewRealRelation("", "", "");
        }

        // remove relation
        private void button2_Click(object sender, EventArgs e)
        {
            ApplicationHandler.R_RemoveRealRelation("");
        }

        // set otl version
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
