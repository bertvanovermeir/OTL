using OTLWizard.Helpers;
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
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            Text = Language.Get("settingswindowheader");
            label1.Text = Language.Get("labelobjecttype");
            label2.Text = Language.Get("labelsetlanguage");
            label3.Text = Language.Get("labelsetkl");
            buttonOK.Text = Language.Get("buttonok");
            buttonCancel.Text = Language.Get("buttoncancel");
            textBox1.Text = Settings.Get("klpath");
            var lang = Settings.Get("language");
            var types = Settings.Get("types").Split('|');
            // check language
            if(lang.Equals("nl"))
            {
                radioButton1.Checked = true;
            } else if(lang.Equals("en"))
            {
                radioButton2.Checked = true;
            }
            // check types
            foreach(string ty in types)
            {
                switch (ty)
                {
                    case "onderdeel":
                        checkBox1.Checked = true;
                        break;
                    case "implementatieelement":
                        checkBox3.Checked = true;
                        break;
                    case "levenscyclus":
                        checkBox4.Checked = true;
                        break;
                    case "installatie":
                        checkBox2.Checked = true;
                        break;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Settings.Update("klpath", textBox1.Text);
            if(radioButton1.Checked)
            {
                Settings.Update("language", "nl");
            }
            else
            {
                Settings.Update("language", "en");
            }

            var types = "";
            if(checkBox1.Checked)
            {

            }
            if (checkBox2.Checked)
            {

            }
            if (checkBox3.Checked)
            {

            }
            if (checkBox4.Checked)
            {

            }
            Settings.Update("types", types);

            Settings.WriteSettings();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
