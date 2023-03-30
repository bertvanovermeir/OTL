using OTLWizard.Helpers;
using OTLWizard.OTLObjecten;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class RelationUserDefinedWindow : Form
    {

        OTL_ConnectingEntityHandle h;

        public RelationUserDefinedWindow()
        {
            InitializeComponent();
        }



        private void RelationUserDefinedWindow_Load(object sender, EventArgs e)
        {
            this.Text = Language.Get("relationuserdefinedwindow");
        }


        internal void Init(OTL_ConnectingEntityHandle optionalArgument)
        {
            h = optionalArgument as OTL_ConnectingEntityHandle;
            textBox1.Text = h.bronId;
            var listoftypes = ApplicationHandler.R_GetAllRelationshipTypes().Select(x => x.relationshipName).Distinct();
            comboBox1.Items.AddRange(listoftypes.ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            ViewHandler.Show(Enums.Views.isNull, Enums.Views.RelationsUserDefined, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // first check if the ID already exists

            if (textBox2.Text == "" || comboBox1.Text == "")
            {

            }
            else
            {
                // creates a new relation
                h.doelId = textBox2.Text;
                h.relationName = comboBox1.Text;
                var temp = ApplicationHandler.R_GetAllRelationshipTypes().Where(x => x.relationshipName == h.relationName).Select(x => x).FirstOrDefault();
                h.isDirectional = temp.isDirectional;
                h.DisplayName = temp.DisplayName; //?? not sure if necessary
                h.typeuri = temp.relationshipURI;
                ApplicationHandler.R_CreateNewRealRelation(h);

                // creates a new asset
                var temp2 = ApplicationHandler.R_GetImportedEntities().Where(i => i.AssetId == textBox2.Text).FirstOrDefault();
                if (temp2 == null)
                {
                    // create user asset
                    ApplicationHandler.R_CreateUserAsset(h.doelId);
                }
                // reset
                comboBox1.Items.Clear();
                textBox2.Text = "";
                // return to main
                ViewHandler.Show(Enums.Views.isNull, Enums.Views.RelationsUserDefined, null);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
