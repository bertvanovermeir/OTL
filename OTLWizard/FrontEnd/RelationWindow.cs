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

            // headings
            groupBox1.Text = Language.Get("gb1");
            groupBox2.Text = Language.Get("gb2");
            groupBox3.Text = Language.Get("gb3");
            groupBox4.Text = Language.Get("gb4");
            button1.Text = Language.Get("addrel");
            button2.Text = Language.Get("remrel");
            button3.Text = Language.Get("aupdy");
            button4.Text = Language.Get("remrelsoft");
            statuslabel.Text = Language.Get("statuslabel");
            this.Text = Language.Get("relationwindowheader");
            fileToolStripMenuItem.Text = Language.Get("mfile");
            manualToolStripMenuItem.Text = Language.Get("mmanual");
            aboutToolStripMenuItem.Text = Language.Get("mtitleextra");
            aboutToolStripMenuItem1.Text = Language.Get("mabout");
        }

        private void NShapeLayouter()
        {   try
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
                display1.Diagram.Width = 2500;
                display1.Diagram.Height = 2500;
                layouter.Fit(50, 50, display1.Diagram.Width - 100, display1.Diagram.Height - 100);
                cachedRepository1.InsertAll(diagram);
            } catch
            {
                // no shapes were defined.
            }               
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
            style1.CapSize = 20;
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

      

        public async Task ImportUserSelectionAsync(Dictionary<string, string[]> optionalArgument)
        {
            if (optionalArgument.ContainsKey("files") && optionalArgument.ContainsKey("subset")) {
                await ApplicationHandler.R_ImportSubsetAsync(optionalArgument["subset"]);
                await ApplicationHandler.R_ImportRealRelationDataAsync(optionalArgument["files"]);                
                UI_UpdateImportedEntities(ApplicationHandler.R_GetImportedEntities().ToArray());
                updateUserRelations();
                if (ApplicationHandler.R_GetImportedEntities().Count > 30)
                {
                    ViewHandler.Show(Language.Get("maxelemwarn"), Language.Get("maxelemwarnhead"), MessageBoxIcon.Exclamation);
                    autoupdate = false;
                }
            } else
            {
                ViewHandler.Show(Language.Get("missingfiles"), "Oops", MessageBoxIcon.Error);
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
            fdlg.FileName = "relations";
            fdlg.Filter = "CSV files (*.csv)|*.csv";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                ApplicationHandler.R_ExportRealRelationData(fdlg.FileName);
                updateStatusText(ListCreatedRelations.Items.Count + Language.Get("st_export"));
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo: check if file was already saved this session, then dont execute the savefiledialog

            SaveFileDialog fdlg = new SaveFileDialog();
            fdlg.Title = Language.Get("SaveState");
            fdlg.FileName = "relations";
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
            ViewHandler.Show("Development by Bert Van Overmeir for AWV (2023). For more information about custom development contact bert.vanovermeir@gmail.com", "About", MessageBoxIcon.Information);
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
                UI_UpdateImportedEntities(ApplicationHandler.R_GetImportedEntities().ToArray());
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
                button3.Text = Language.Get("aupdn");
                autoupdate = false;
            } else
            {
                button3.Text = Language.Get("aupdy");
                updateVisuals();
                autoupdate=true;
            }
        }

        private void statuslabel_Click(object sender, EventArgs e)
        {
            // nothing
        }

        // add relation
        private void buttonAddRelation(object sender, EventArgs e)
        {
            // first make sure a selection is made, then execute the creation process
            if(ListImportedEntities.SelectedItem != null && ListRelationsPerEntity.SelectedItems != null)
            {
                // if the clicked option is "other" then goto different method
                foreach (OTL_ConnectingEntityHandle item in ListRelationsPerEntity.SelectedItems)
                {
                    if(item.DisplayName == Language.Get("userdefinedrelation"))
                    {
                        ViewHandler.Show(Enums.Views.RelationsUserDefined, Enums.Views.isNull, item);
                        ListImportedEntities.DataSource = ApplicationHandler.R_GetImportedEntities().ToArray();
                    }
                    else
                    {
                        ApplicationHandler.R_CreateNewRealRelation(item);
                    }
                }
                updateUserView();
                updateUserRelations();
                updateVisuals();
                updateStatusText(Language.Get("st_added") + ListRelationsPerEntity.SelectedItems.ToString());

            } else
            {
                updateStatusText(Language.Get("st_add"));
            }
        }

        // remove relation
        private void buttonRemoveRelation(object sender, EventArgs e)
        {
            if(ListCreatedRelations.SelectedItem != null)
            {
                OTL_Relationship rel = (OTL_Relationship) ListCreatedRelations.SelectedItem;
                ApplicationHandler.R_RemoveRealRelation(rel.AssetId, false);
                updateUserView();
                updateUserRelations();
                updateVisuals();
                updateStatusText(Language.Get("st_removed") + rel.AssetId);
            }
            else
            {
                updateStatusText(Language.Get("st_remove"));

            }

        }

       

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateUserView();
            updateUserRelations();
            var temp = (OTL_Entity) ListImportedEntities.SelectedItem;
            textBox3.Text = temp.AssetId;
        }

     
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void updateUserView()
        {
            OTL_Entity source = (OTL_Entity) ListImportedEntities.SelectedItem;
            // update properties of one object
            ListPropertiesPerEntity.DataSource = source.Properties.ToArray();
            ListPropertiesPerEntity.Update();
            // update available relations          
            ListRelationsPerEntity.DisplayMember = "DisplayName";
            ListRelationsPerEntity.ValueMember = "DisplayName";
            ListRelationsPerEntity.DataSource = ApplicationHandler.R_GetPossibleRelations(source);
                   
        }

        private void updateUserViewRelationProperties()
        {
            OTL_Relationship source = (OTL_Relationship) ListCreatedRelations.SelectedItem;
            ListPropertiesRelation.DataSource = source.Properties.ToArray();
            ListPropertiesRelation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ListPropertiesRelation.Update();
            ListPropertiesRelation.Refresh();
        }

        private void updateUserRelations()
        {
            if (ListCreatedRelations.Items != null)
            {
                ListCreatedRelations.DisplayMember = "DisplayName";
                ListCreatedRelations.ValueMember = "DisplayName";
                if (textBox2.Text != "")
                {
                    var zoekterm = textBox3.Text.ToLower();
                    var filteredFiles = ApplicationHandler.R_GetRealRelations().Where(x => x.DisplayName.ToLower().Contains(zoekterm)).ToArray();
                    ListCreatedRelations.DataSource = filteredFiles;
                }
                else
                {
                    ListCreatedRelations.DataSource = ApplicationHandler.R_GetRealRelations();
                }
            }
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
                            Shape s = NShapeCreateNode(drawing.AssetId);
                            tempdict.Add(drawing.AssetId, s);
                }
                foreach (var reldrawable in reldrawables)
                {
                    NShapeConnectNode(tempdict[reldrawable.doelID], tempdict[reldrawable.bronID], reldrawable.relationshipURI.Split('#')[1], reldrawable.isDirectional);
                }
                NShapeLayouter();
            } else
            {
                // do nothing
            }
            
        }

        private void UI_UpdateImportedEntities(OTL_Entity[] data)
        {
            ListImportedEntities.DisplayMember = "DisplayName";
            ListImportedEntities.ValueMember = "AssetId";
            ListImportedEntities.DataSource = data;
            updateStatusText(ListImportedEntities.Items.Count + Language.Get("st_import"));
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, Language.Get("suretoclose"), Language.Get("suretocloseheader"), MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    ApplicationHandler.R_DestroyOnClose();
                    ViewHandler.Show(Enums.Views.Home, Enums.Views.Relations, null);
                    break;
            }
        }

        private void updateStatusText(string text)
        {
            statuslabel.Text = text;
        }

        // searchfield entities
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(ListImportedEntities.Items != null)
            {
                if(textBox1.Text != "")
                {
                    var zoekterm = textBox1.Text.ToLower();
                    var filteredFiles = ApplicationHandler.R_GetImportedEntities().Where(x => x.DisplayName.ToLower().Contains(zoekterm)).ToArray();
                    ListImportedEntities.DataSource = filteredFiles;
                } else
                {
                    ListImportedEntities.DataSource = ApplicationHandler.R_GetImportedEntities().ToArray();
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (ListRelationsPerEntity.Items != null)
            {
                OTL_Entity source = (OTL_Entity)ListImportedEntities.SelectedItem;

                if (textBox2.Text != "")
                {
                    var zoekterm = textBox2.Text.ToLower();
                    var filteredFiles = ApplicationHandler.R_GetPossibleRelations(source).Where(x => x.DisplayName.ToLower().Contains(zoekterm)).ToArray();
                    ListRelationsPerEntity.DataSource = filteredFiles;
                }
                else
                {
                    ListRelationsPerEntity.DataSource = ApplicationHandler.R_GetPossibleRelations(source);
                }
            }
        }

        private void ListRelationsPerEntity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/bertvanovermeir/OTL");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ListCreatedRelations_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateUserViewRelationProperties();
        }

        // softremove
        private void button4_Click(object sender, EventArgs e)
        {
            if (ListCreatedRelations.SelectedItem != null)
            {
                OTL_Relationship rel = (OTL_Relationship)ListCreatedRelations.SelectedItem;
                ApplicationHandler.R_RemoveRealRelation(rel.AssetId, true);
                updateUserView();
                updateUserRelations();
                updateVisuals();
                updateStatusText(Language.Get("st_removed") + rel.AssetId);
            }
            else
            {
                updateStatusText(Language.Get("st_remove"));

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            updateUserRelations();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/bertvanovermeir");
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
