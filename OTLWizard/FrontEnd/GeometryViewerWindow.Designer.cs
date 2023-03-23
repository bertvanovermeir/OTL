namespace OTLWizard.FrontEnd
{
    partial class GeometryViewerWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // GeometryViewerWindow
            // 
            this.ClientSize = new System.Drawing.Size(1055, 564);
            this.Name = "GeometryViewerWindow";
            this.Load += new System.EventHandler(this.GeometryViewerWindow_Load_1);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Create_Geometry);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}