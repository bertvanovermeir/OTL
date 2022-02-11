namespace OTLWizard
{
    partial class ExportArtefactWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportArtefactWindow));
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxSubset = new System.Windows.Forms.TextBox();
            this.buttonSubset = new System.Windows.Forms.Button();
            this.textBoxArtefact = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonExportArtefact = new System.Windows.Forms.Button();
            this.checkAllClasses = new System.Windows.Forms.CheckBox();
            this.ListAllClasses = new System.Windows.Forms.ListBox();
            this.buttonImportClasses = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(736, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 54);
            this.button1.TabIndex = 20;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.terug);
            // 
            // textBoxSubset
            // 
            this.textBoxSubset.Enabled = false;
            this.textBoxSubset.Location = new System.Drawing.Point(174, 70);
            this.textBoxSubset.Name = "textBoxSubset";
            this.textBoxSubset.Size = new System.Drawing.Size(614, 20);
            this.textBoxSubset.TabIndex = 22;
            // 
            // buttonSubset
            // 
            this.buttonSubset.Location = new System.Drawing.Point(15, 70);
            this.buttonSubset.Name = "buttonSubset";
            this.buttonSubset.Size = new System.Drawing.Size(153, 23);
            this.buttonSubset.TabIndex = 21;
            this.buttonSubset.Text = "Selecteer een subset...";
            this.buttonSubset.UseVisualStyleBackColor = true;
            this.buttonSubset.Click += new System.EventHandler(this.buttonSubset_Click);
            // 
            // textBoxArtefact
            // 
            this.textBoxArtefact.Enabled = false;
            this.textBoxArtefact.Location = new System.Drawing.Point(174, 96);
            this.textBoxArtefact.Name = "textBoxArtefact";
            this.textBoxArtefact.Size = new System.Drawing.Size(614, 20);
            this.textBoxArtefact.TabIndex = 24;
            this.textBoxArtefact.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "Selecteer het artefact";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonArtefact_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(515, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "OTL Artefact Manager controleert welk geometrie dient aangeleverd te worden voor " +
    "bepaalde OTL klassen.";
            // 
            // buttonExportArtefact
            // 
            this.buttonExportArtefact.Enabled = false;
            this.buttonExportArtefact.Location = new System.Drawing.Point(641, 415);
            this.buttonExportArtefact.Name = "buttonExportArtefact";
            this.buttonExportArtefact.Size = new System.Drawing.Size(147, 23);
            this.buttonExportArtefact.TabIndex = 26;
            this.buttonExportArtefact.Text = "Exporteer artefactinformatie";
            this.buttonExportArtefact.UseVisualStyleBackColor = true;
            this.buttonExportArtefact.Click += new System.EventHandler(this.buttonExportArtefact_Click);
            // 
            // checkAllClasses
            // 
            this.checkAllClasses.AutoSize = true;
            this.checkAllClasses.Enabled = false;
            this.checkAllClasses.Location = new System.Drawing.Point(396, 187);
            this.checkAllClasses.Name = "checkAllClasses";
            this.checkAllClasses.Size = new System.Drawing.Size(129, 17);
            this.checkAllClasses.TabIndex = 29;
            this.checkAllClasses.Text = "Selecteer alle klassen";
            this.checkAllClasses.UseVisualStyleBackColor = true;
            this.checkAllClasses.CheckedChanged += new System.EventHandler(this.checkAllClasses_CheckedChanged);
            // 
            // ListAllClasses
            // 
            this.ListAllClasses.Enabled = false;
            this.ListAllClasses.FormattingEnabled = true;
            this.ListAllClasses.Location = new System.Drawing.Point(15, 187);
            this.ListAllClasses.Name = "ListAllClasses";
            this.ListAllClasses.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.ListAllClasses.Size = new System.Drawing.Size(374, 251);
            this.ListAllClasses.TabIndex = 28;
            this.ListAllClasses.SelectedIndexChanged += new System.EventHandler(this.ListAllClasses_SelectedIndexChanged);
            // 
            // buttonImportClasses
            // 
            this.buttonImportClasses.Enabled = false;
            this.buttonImportClasses.Location = new System.Drawing.Point(15, 158);
            this.buttonImportClasses.Name = "buttonImportClasses";
            this.buttonImportClasses.Size = new System.Drawing.Size(153, 23);
            this.buttonImportClasses.TabIndex = 27;
            this.buttonImportClasses.Text = "Importeer klassen";
            this.buttonImportClasses.UseVisualStyleBackColor = true;
            this.buttonImportClasses.Click += new System.EventHandler(this.buttonImportClasses_Click);
            // 
            // ExportArtefactWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.checkAllClasses);
            this.Controls.Add(this.ListAllClasses);
            this.Controls.Add(this.buttonImportClasses);
            this.Controls.Add(this.buttonExportArtefact);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxArtefact);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxSubset);
            this.Controls.Add(this.buttonSubset);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExportArtefactWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Artefact Manager";
            this.Load += new System.EventHandler(this.ExportArtefactWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxSubset;
        private System.Windows.Forms.Button buttonSubset;
        private System.Windows.Forms.TextBox textBoxArtefact;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonExportArtefact;
        private System.Windows.Forms.CheckBox checkAllClasses;
        private System.Windows.Forms.ListBox ListAllClasses;
        private System.Windows.Forms.Button buttonImportClasses;
    }
}