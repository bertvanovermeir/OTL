using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.MDS;
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
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        Microsoft.Msagl.Drawing.Graph graph;

        public RelationWindow()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CreateNode(OTL_Entity otl)
        {
            var node = graph.AddNode(otl.AssetId);
            node.LabelText = "<" + otl.AssetId + ">\n" + otl.Name;
            node.Label.FontSize = 3;
            setGraphicalRepresentation(node, Microsoft.Msagl.Drawing.Color.Gray, Microsoft.Msagl.Drawing.Shape.Box);
        }

        private void setGraphicalRepresentation(Node node, Microsoft.Msagl.Drawing.Color fill, Microsoft.Msagl.Drawing.Shape shape)
        {
            node.Attr.FillColor = fill;
            node.Attr.Shape = shape;
        }

        private void RelationWindow_Load(object sender, EventArgs e)
        {
            // MSAGL
            Layouter();
            // headings
            groupBox1.Text = Language.Get("gb1");
            groupBox2.Text = Language.Get("gb2");
            groupBox3.Text = Language.Get("gb3");
            groupBox4.Text = Language.Get("gb4");
            button1.Text = Language.Get("addrel");
            button2.Text = Language.Get("remrel");
            button4.Text = Language.Get("remrelsoft");
            checkBox1.Text = Language.Get("chckrel");
            statuslabel.Text = Language.Get("statuslabel");
            this.Text = Language.Get("relationwindowheader");
            fileToolStripMenuItem.Text = Language.Get("mfile");
            manualToolStripMenuItem.Text = Language.Get("mmanual");
            aboutToolStripMenuItem.Text = Language.Get("mhelp");
            aboutToolStripMenuItem1.Text = Language.Get("mabout");
            saveToolStripMenuItem.Text = Language.Get("saveproj");
            openToolStripMenuItem.Text = Language.Get("openproj");
            importToolStripMenuItem.Text = Language.Get("impfle");
            exportToolStripMenuItem.Text = Language.Get("expfle");
            newProjectToolStripMenuItem.Text = Language.Get("closerelmgr");
            hideBase64NotationDAVIEImportToolStripMenuItem.Text = Language.Get("hidebase64");
            hideBase64NotationDAVIEImportToolStripMenuItem.Checked = Boolean.Parse(Settings.Get("hidebase64cosmetic"));
            hideRelationshipURIToolStripMenuItem.Text = Language.Get("hidereluri");
            hideRelationshipURIToolStripMenuItem.Checked = Boolean.Parse(Settings.Get("hidereluricosmetic"));
            otherSettingsToolStripMenuItem.Text = Language.Get("othersettings");
            optionsToolStripMenuItem.Text = Language.Get("mtitleextra");
        }

        private void updateLayouter()
        {
            viewer.Graph = graph;
            viewer.Update();
        }

        private void Layouter()
        {
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
            groupBox4.Controls.Add(viewer);
            ResumeLayout();
        }

        private void ConnectNode(OTL_Relationship otl)
        {
            var relation = graph.AddEdge(otl.bronID, otl.doelID);
            relation.LabelText = otl.relationshipURI.Split('#')[1];
            relation.Label.FontSize = 3;
            if (!otl.isDirectional)
            {
                relation.Attr.ArrowheadAtSource = ArrowStyle.None;
                relation.Attr.ArrowheadAtTarget = ArrowStyle.None;
            }
        }

        public async Task ImportUserSelectionAsync(Dictionary<string, string[]> optionalArgument)
        {
            if (optionalArgument.ContainsKey("files") && optionalArgument.ContainsKey("subset"))
            {
                var result = await ApplicationHandler.R_ImportSubsetAsync(optionalArgument["subset"]);
                if(result)
                {
                    await ApplicationHandler.R_ImportRealRelationDataAsync(optionalArgument["files"]);
                    UI_UpdateImportedEntities(ApplicationHandler.R_GetImportedEntities().ToArray());
                    updateUserRelations();
                    updateVisuals();
                }                
            }
            else
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
            fdlg.Title = Language.Get("savestate");
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
            ViewHandler.Show(Enums.Views.Home, Enums.Views.RelationsMain, null);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewHandler.Show("Development by Bert Van Overmeir for AWV (2023). For more information about custom development contact bert.vanovermeir@gmail.com", "About", MessageBoxIcon.Information);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = Language.Get("selectsavestate");
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "XMLR Files (*.xmlr)|*.xmlr";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                ApplicationHandler.R_LoadRelationState(fdlg.FileName);
                UI_UpdateImportedEntities(ApplicationHandler.R_GetImportedEntities().ToArray());
                updateVisuals();
            }
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        // disable Autoupdate
        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void statuslabel_Click(object sender, EventArgs e)
        {
            // nothing
        }

        // add relation
        private void buttonAddRelation(object sender, EventArgs e)
        {
            // first make sure a selection is made, then execute the creation process
            if (ListImportedEntities.SelectedItem != null && ListRelationsPerEntity.SelectedItems != null)
            {
                // if the clicked option is "other" then goto different method
                foreach (OTL_ConnectingEntityHandle item in ListRelationsPerEntity.SelectedItems)
                {
                    if (item.DisplayName == Language.Get("userdefinedrelation"))
                    {
                        ViewHandler.Show(Enums.Views.RelationsUserDefined, Enums.Views.isNull, item);
                        var ind = ListImportedEntities.SelectedIndex;
                        ListImportedEntities.DataSource = ApplicationHandler.R_GetImportedEntities().ToArray();
                        if(ind+1 < ListImportedEntities.Items.Count)
                        {
                            ListImportedEntities.SetSelected(ind + 1, true);
                        }
                    }
                    else
                    {
                        ApplicationHandler.R_CreateNewRealRelation(item);
                    }
                }
                updateUserView();
                updateUserRelations();
                updateVisuals();
                updateStatusText(Language.Get("st_added") + ListRelationsPerEntity.SelectedItems.Count);
            }
            else
            {
                updateStatusText(Language.Get("st_add"));
            }
        }

        // remove relation
        private void buttonRemoveRelation(object sender, EventArgs e)
        {
            if (ListCreatedRelations.SelectedItems != null)
            {
                foreach (var item in ListCreatedRelations.SelectedItems)
                {
                    OTL_Relationship rel = (OTL_Relationship)item;
                    ApplicationHandler.R_RemoveRealRelation(rel.AssetId, false);

                }
                updateStatusText(Language.Get("st_removed") + ListCreatedRelations.SelectedItems.Count);
                updateUserView();
                updateUserRelations();
                updateVisuals();
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
            var temp = (OTL_Entity)ListImportedEntities.SelectedItem;
            var val = temp.AssetId;
            if (Boolean.Parse(Settings.Get("hidebase64cosmetic")))
            {
                // change the relationshipuris, if applicable, if not silently fail.
                // they look like this: 632f6526-7bdf-4d44-a289-a0a00a993793-b25kZXJkZWVsI0VpbmRzdHVr               
                try
                {
                    var col = val.Split('-');
                    var replacer = col[col.Length - 1];
                    val = val.Replace("-" + replacer, "");
                }
                catch
                {
                    // silent error
                }
            }
            textBox3.Text = val;

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void updateUserView()
        {
            OTL_Entity source = (OTL_Entity)ListImportedEntities.SelectedItem;
            // update properties of one object
            ListPropertiesPerEntity.DataSource = source.Properties.OrderBy(x => x.Key).ToArray();
            ListPropertiesPerEntity.Update();
            // update available relations          
            ListRelationsPerEntity.DisplayMember = "DisplayName";
            ListRelationsPerEntity.ValueMember = "DisplayName";
            var items = ApplicationHandler.R_GetPossibleRelations(source);
            ListRelationsPerEntity.DataSource = items;
            if (items.Count == 1)
            {
                updateStatusText(Language.Get("norelationsfoundforobject"));
            }
            else
            {
                updateStatusText(Language.Get("relationsfound") + (items.Count - 1));
            }
        }

        private void updateUserViewRelationProperties()
        {
            OTL_Relationship source = (OTL_Relationship)ListCreatedRelations.SelectedItem;
            source.Properties.OrderBy(x => x.Key).ToList();
            ListPropertiesRelation.DataSource = source.Properties.OrderBy(x => x.Key).ToArray();
            ListPropertiesRelation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ListPropertiesRelation.Update();
            ListPropertiesRelation.Refresh();
        }

        private void updateUserViewRelationTargetProperties()
        {
            OTL_ConnectingEntityHandle otl_handle = (OTL_ConnectingEntityHandle)ListRelationsPerEntity.SelectedItem;
            if (otl_handle.DisplayName == Language.Get("userdefinedrelation"))
            {
                // this is not a real relation
                ListPropertiesTarget.DataSource = null;
                ListPropertiesTarget.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ListPropertiesTarget.Update();
                ListPropertiesTarget.Refresh();
            } else
            {
                ListPropertiesTarget.DataSource = ApplicationHandler.R_GetEntityForID(otl_handle.doelId).Properties.OrderBy(x => x.Key).ToArray();
                ListPropertiesTarget.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ListPropertiesTarget.Update();
                ListPropertiesTarget.Refresh();
            }

                
        }

        private void updateUserRelations()
        {
            if (ListCreatedRelations.Items != null)
            {
                ListCreatedRelations.DisplayMember = "DisplayName";
                ListCreatedRelations.ValueMember = "DisplayName";
                if (textBox3.Text != "")
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
            var validUpdate = true;

            if(ApplicationHandler.R_GetImportedEntities().Count == 0)
            {
                validUpdate = false;
            }

            // check if all or selection only
            if (validUpdate)
            {
                graph = new Microsoft.Msagl.Drawing.Graph("graph");
                var drawables = ApplicationHandler.R_GetImportedEntities();
                var reldrawables = ApplicationHandler.R_GetRealRelationsObjects();

                if (checkBox1.Checked)
                {
                    if (textBox2.Text != "")
                    {
                        var zoekterm = textBox3.Text.ToLower();
                        var filteredFiles = ApplicationHandler.R_GetRealRelations().Where(x => x.DisplayName.ToLower().Contains(zoekterm));
                        reldrawables = filteredFiles.Where(x => x.Activated == true).ToArray();
                    }
                    
                        // only selected without foreign objects
                        foreach (var drawing in drawables)
                        {
                            foreach (var item in reldrawables)
                            {
                                if (item.doelID.Equals(drawing.AssetId) || item.bronID.Equals(drawing.AssetId))
                                {
                                    CreateNode(drawing);
                                    break;
                                }
                            }
                        }
                    

                }
                else
                {
                    // all
                    foreach (var drawing in drawables)
                    {
                        CreateNode(drawing);
                    }
                }
                foreach (var reldrawable in reldrawables)
                {
                    if (reldrawable.Activated)
                    {
                        ConnectNode(reldrawable);
                    }
                }
                updateLayouter();
            }
            else
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
                    ViewHandler.Show(Enums.Views.Home, Enums.Views.RelationsMain, null);
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
            if (ListImportedEntities.Items != null)
            {
                if (textBox1.Text != "")
                {
                    var zoekterm = textBox1.Text.ToLower();
                    var filteredFiles = ApplicationHandler.R_GetImportedEntities().Where(x => x.DisplayName.ToLower().Contains(zoekterm)).ToArray();
                    ListImportedEntities.DataSource = filteredFiles;
                }
                else
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
            updateUserViewRelationTargetProperties();
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
            if (ListCreatedRelations.SelectedItems != null)
            {
                foreach (var entity in ListCreatedRelations.SelectedItems)
                {
                    OTL_Relationship rel = (OTL_Relationship)entity;
                    ApplicationHandler.R_RemoveRealRelation(rel.AssetId, true);

                }
                updateStatusText(Language.Get("st_removed") + ListCreatedRelations.SelectedItems.Count);
                updateUserView();
                updateUserRelations();
                updateVisuals();
            }
            else
            {
                updateStatusText(Language.Get("st_remove"));
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            updateUserRelations();
            if (checkBox1.Checked)
            {
                updateVisuals();
            }
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

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            updateLayouter();
        }
        //base64 main enabler
        private void hideBase64NotationDAVIEImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(hideBase64NotationDAVIEImportToolStripMenuItem.Checked)
            {
                hideBase64NotationDAVIEImportToolStripMenuItem.Checked = false;
            } else
            {
                hideBase64NotationDAVIEImportToolStripMenuItem.Checked = true;
            }
            Settings.Update("hidebase64cosmetic", hideBase64NotationDAVIEImportToolStripMenuItem.Checked.ToString().ToLower());
            Settings.WriteSettings();
            ViewHandler.Show(Language.Get("restartrequiredsetting"), Language.Get("restartrequiredsettingtitle"), MessageBoxIcon.Information);
        }

        private void hideRelationshipURIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(hideRelationshipURIToolStripMenuItem.Checked)
            {
                hideRelationshipURIToolStripMenuItem.Checked = false;
            } else
            {
                hideRelationshipURIToolStripMenuItem.Checked = true;
            }
            Settings.Update("hidereluricosmetic", hideRelationshipURIToolStripMenuItem.Checked.ToString().ToLower());
            Settings.WriteSettings();
            ViewHandler.Show(Language.Get("restartrequiredsetting"), Language.Get("restartrequiredsettingtitle"), MessageBoxIcon.Information);
        }

        private void otherSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var localPath = System.IO.Path.GetTempPath() + "otlsettingsv6\\";
            Process.Start("notepad.exe", localPath + "settings.txt");
        }
    }
}
