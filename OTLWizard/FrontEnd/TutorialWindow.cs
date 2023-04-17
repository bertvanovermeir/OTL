using OTLWizard.Helpers;
using OTLWizard.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OTLWizard.FrontEnd
{
    public partial class TutorialWindow : Form
    {

        private Enums.Views currentTutorial;
        private readonly int[] subsettool = { 1, 2, 3, 4, 5 };
        private readonly int[] artefacttool = { 1, 2, 3 };
        private readonly int[] relationtool = { 1, 2, 3 };
        private readonly int[] sdftool = { 1, 2, 3, 4 };
        private int[] currentPages;
        private int currentPage;
        public TutorialWindow()
        {
            InitializeComponent();
        }

        private void TutorialWindow_Load(object sender, EventArgs e)
        {
            this.Text = Language.Get("tutorialheader");
            buttonClose.Text = Language.Get("close");
            buttonNext.Text = Language.Get("next");
            checkBox1.Text = Language.Get("dontshowthisagain");
            animation.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public void SetTutorial(Enums.Views optionalArgument)
        {
            bool doTutorial = false;
            currentTutorial = optionalArgument;
            switch (optionalArgument)
            {
                case Enums.Views.SubsetMain:
                    currentPages = subsettool;
                    doTutorial = true;
                    break;
                case Enums.Views.ArtefactMain:
                    currentPages = artefacttool;
                    doTutorial = true;
                    break;
                case Enums.Views.RelationsMain:
                    currentPages = relationtool;
                    doTutorial = true;
                    break;
                case Enums.Views.SDFMain:
                    currentPages = sdftool;
                    doTutorial = true;
                    break;
            }
            // check if you have to show the tutorial
            var show = Settings.Get("showtutorial" + currentTutorial);

            if (doTutorial && show.Equals("1"))
            {
                setWindowValues(0);
                this.Show();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            if (currentPage > currentPages.Length - 1)
            {
                currentPage = 0;
                finishTutorial();
            }
            else
                setWindowValues(currentPage);
        }

        private void setWindowValues(int page)
        {
            animation.Image = Image.FromFile("Data\\animations\\" + currentTutorial + currentPages[page] + ".gif");
            helpText.Text = Language.Get("tutorialmsg" + currentTutorial + currentPages[page]);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            finishTutorial();
        }

        private void finishTutorial()
        {
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Settings.Update("showtutorial" + currentTutorial, "0");
                Settings.WriteSettings();
            }
            else
            {
                Settings.Update("showtutorial" + currentTutorial, "1");
                Settings.WriteSettings();
            }
        }
    }
}
