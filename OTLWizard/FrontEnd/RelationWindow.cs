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
        bool autoupdate = true;

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
            ((CharacterStyle)project1.Design.CharacterStyles.Caption).SizeInPoints = 7;
            ((CharacterStyle)project1.Design.CharacterStyles.Normal).SizeInPoints = 7;
            ((CharacterStyle)project1.Design.CharacterStyles.Heading1).SizeInPoints = 7;
            ((CharacterStyle)project1.Design.CharacterStyles.Subtitle).SizeInPoints = 7;
            project1.Repository.Update(project1.Design.CharacterStyles.Caption);

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

      

        internal async Task ImportUserSelectionAsync(Dictionary<string, string[]> optionalArgument)
        {
            var bfiles = false;
            var bsubset = false;
            if (optionalArgument.ContainsKey("files") && optionalArgument.ContainsKey("subset")) {
                bsubset = await ApplicationHandler.R_ImportSubsetAsync(optionalArgument["subset"]);
                bfiles = ApplicationHandler.R_ImportRealRelationData(optionalArgument["files"]);                
                if (bfiles && bsubset)
                {
                    updateUserImportView(ApplicationHandler.R_GetImportedEntities().ToArray());                                      
                }
                else
                {
                    ViewHandler.Show("One or more assets failed to load.", "error", MessageBoxIcon.Error);
                }
            } else
            {
                ViewHandler.Show("You did not provide required files", "error", MessageBoxIcon.Error);
            }           
        }

        private void display1_Load(object sender, EventArgs e)
        {

        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewHandler.Show(Enums.Views.RelationsImport, Enums.Views.isNull, null);
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

        // disable Autoupdate
        private void button3_Click(object sender, EventArgs e)
        {
            if(autoupdate)
            {
                button3.Text = "AutoUpdate Disabled";
                autoupdate = false;
            } else
            {
                button3.Text = "AutoUpdate Enabled";
                updateVisuals();
                autoupdate=true;
            }
        }

        private void statuslabel_Click(object sender, EventArgs e)
        {
            // nothing
        }

        // add relation
        private void button1_Click(object sender, EventArgs e)
        {
            // first make sure a selection is made, then execute the creation process
            
            if(listBox1.SelectedItem != null && listBox2.SelectedItem != null)
            {
                var uri1 = listBox1.SelectedItem.ToString().Split('|')[0].Trim();
                var uri2 = listBox2.SelectedItem.ToString().Replace("-->", "|").Replace("<", "").Split('|')[1].Trim();
                var reluri = ApplicationHandler.R_GetRelationURLFromName(listBox2.SelectedItem.ToString().Replace("-->", "|").Replace("<", "").Split('|')[0].Trim());           
                ApplicationHandler.R_CreateNewRealRelation(uri1, uri2, reluri);
                updateUserView();
                updateVisuals();
            } else
            {
                ViewHandler.Show("select some objects first", "make selection", MessageBoxIcon.Information);
            }
        }

        // remove relation
        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox3.SelectedItem != null)
            {
                var rel = listBox3.SelectedItem.ToString().Split('|')[2].Trim().Split(':')[1].Trim();
                ApplicationHandler.R_RemoveRealRelation(rel);
                updateUserView();
                updateVisuals();
            }
            else
            {
                ViewHandler.Show("select a relation first", "make selection", MessageBoxIcon.Information);
            }

        }

       

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           updateUserView();
        }

     
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void updateUserView()
        {
            var temp = ApplicationHandler.R_GetImportedEntity(listBox1.SelectedItem.ToString().Split('|')[0].Trim());
            // update properties of one object
            dataGridView1.DataSource = temp.Properties.ToArray();
            dataGridView1.Update();
            // update available relations          
            listBox2.Items.Clear();
            listBox2.Items.AddRange(ApplicationHandler.R_GetPossibleRelations(temp));
            // update created relations
            listBox3.Items.Clear();
            listBox3.Items.AddRange(ApplicationHandler.R_GetRealRelations());
        }

        private void updateVisuals()
        {
            if(autoupdate)
            {
                var tempdict = new Dictionary<string, Shape>();
                var drawables = ApplicationHandler.R_GetImportedEntities();
                var reldrawables = ApplicationHandler.R_GetRealRelationsObjects();



                Guid guid = Guid.NewGuid();
                diagram = new Diagram(guid.ToString());

                foreach (var drawing in drawables)
                {
                    foreach (var item in reldrawables)
                    {
                        if (item.doelID.Equals(drawing.AssetId) || item.bronID.Equals(drawing.AssetId))
                        {
                            Shape s = NShapeCreateNode(drawing.AssetId);
                            tempdict.Add(drawing.AssetId, s);
                            break;
                        }
                    }

                }
                foreach (var reldrawable in reldrawables)
                {

                    NShapeConnectNode(tempdict[reldrawable.doelID], tempdict[reldrawable.bronID], reldrawable.relationshipURI.Split('#')[1], ApplicationHandler.R_GetIsRelationshipDirectionalFromName(reldrawable.relationshipURI));
                }

                NShapeLayouter();
            } else
            {
                // do nothing
            }
            
        }

        private void updateUserImportView(OTL_Entity[] data)
        {
            listBox1.Items.Clear();
            foreach (var element in data)
            {
                var item = element.AssetId + " | " + element.Name;
                listBox1.Items.Add(item);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    ViewHandler.Show(Enums.Views.Home, Enums.Views.Relations, null);
                    break;
            }
        }
        
    }
}
