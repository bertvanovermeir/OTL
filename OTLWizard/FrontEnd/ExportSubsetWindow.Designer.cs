namespace OTLWizard
{
    partial class ExportSubsetWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportSubsetWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.buttonSubset = new System.Windows.Forms.Button();
            this.textBoxSubset = new System.Windows.Forms.TextBox();
            this.buttonExportXLS = new System.Windows.Forms.Button();
            this.buttonImportClasses = new System.Windows.Forms.Button();
            this.ListAllClasses = new System.Windows.Forms.ListBox();
            this.checkAllClasses = new System.Windows.Forms.CheckBox();
            this.checkAttributes = new System.Windows.Forms.CheckBox();
            this.checkKeuzelijsten = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.voorbeelddata = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(450, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "OTL Template generator maakt templates in Excel van een geimporteerde Object Type" +
    " Library";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Maak eerst een subset met ";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(146, 32);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(122, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "de subset tool van AWV";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // buttonSubset
            // 
            this.buttonSubset.Location = new System.Drawing.Point(15, 78);
            this.buttonSubset.Name = "buttonSubset";
            this.buttonSubset.Size = new System.Drawing.Size(153, 23);
            this.buttonSubset.TabIndex = 4;
            this.buttonSubset.Text = "Selecteer een subset...";
            this.buttonSubset.UseVisualStyleBackColor = true;
            this.buttonSubset.Click += new System.EventHandler(this.selecteerSubsetButton);
            // 
            // textBoxSubset
            // 
            this.textBoxSubset.Enabled = false;
            this.textBoxSubset.Location = new System.Drawing.Point(174, 80);
            this.textBoxSubset.Name = "textBoxSubset";
            this.textBoxSubset.Size = new System.Drawing.Size(614, 20);
            this.textBoxSubset.TabIndex = 5;
            this.textBoxSubset.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // buttonExportXLS
            // 
            this.buttonExportXLS.Enabled = false;
            this.buttonExportXLS.Location = new System.Drawing.Point(618, 415);
            this.buttonExportXLS.Name = "buttonExportXLS";
            this.buttonExportXLS.Size = new System.Drawing.Size(170, 23);
            this.buttonExportXLS.TabIndex = 6;
            this.buttonExportXLS.Text = "Exporteer subset naar XLS/CSV";
            this.buttonExportXLS.UseVisualStyleBackColor = true;
            this.buttonExportXLS.Click += new System.EventHandler(this.Export);
            // 
            // buttonImportClasses
            // 
            this.buttonImportClasses.Enabled = false;
            this.buttonImportClasses.Location = new System.Drawing.Point(15, 107);
            this.buttonImportClasses.Name = "buttonImportClasses";
            this.buttonImportClasses.Size = new System.Drawing.Size(153, 23);
            this.buttonImportClasses.TabIndex = 11;
            this.buttonImportClasses.Text = "Importeer klassen";
            this.buttonImportClasses.UseVisualStyleBackColor = true;
            this.buttonImportClasses.Click += new System.EventHandler(this.ImportClasses);
            // 
            // ListAllClasses
            // 
            this.ListAllClasses.Enabled = false;
            this.ListAllClasses.FormattingEnabled = true;
            this.ListAllClasses.Location = new System.Drawing.Point(15, 136);
            this.ListAllClasses.Name = "ListAllClasses";
            this.ListAllClasses.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.ListAllClasses.Size = new System.Drawing.Size(374, 303);
            this.ListAllClasses.TabIndex = 12;
            this.ListAllClasses.SelectedIndexChanged += new System.EventHandler(this.ListAllClasses_SelectedIndexChanged);
            // 
            // checkAllClasses
            // 
            this.checkAllClasses.AutoSize = true;
            this.checkAllClasses.Enabled = false;
            this.checkAllClasses.Location = new System.Drawing.Point(395, 136);
            this.checkAllClasses.Name = "checkAllClasses";
            this.checkAllClasses.Size = new System.Drawing.Size(129, 17);
            this.checkAllClasses.TabIndex = 13;
            this.checkAllClasses.Text = "Selecteer alle klassen";
            this.checkAllClasses.UseVisualStyleBackColor = true;
            this.checkAllClasses.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkAttributes
            // 
            this.checkAttributes.AutoSize = true;
            this.checkAttributes.Enabled = false;
            this.checkAttributes.Location = new System.Drawing.Point(395, 159);
            this.checkAttributes.Name = "checkAttributes";
            this.checkAttributes.Size = new System.Drawing.Size(232, 17);
            this.checkAttributes.TabIndex = 14;
            this.checkAttributes.Text = "Exporteer een omschrijving van elk attribuut";
            this.checkAttributes.UseVisualStyleBackColor = true;
            this.checkAttributes.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkKeuzelijsten
            // 
            this.checkKeuzelijsten.AutoSize = true;
            this.checkKeuzelijsten.Enabled = false;
            this.checkKeuzelijsten.Location = new System.Drawing.Point(395, 182);
            this.checkKeuzelijsten.Name = "checkKeuzelijsten";
            this.checkKeuzelijsten.Size = new System.Drawing.Size(179, 17);
            this.checkKeuzelijsten.TabIndex = 15;
            this.checkKeuzelijsten.Text = "Geen keuzelijstopties aanmaken";
            this.checkKeuzelijsten.UseVisualStyleBackColor = true;
            this.checkKeuzelijsten.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(622, 399);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Dit kan enige tijd in beslag nemen";
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(736, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 54);
            this.button1.TabIndex = 19;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(177, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(336, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Het gebruik van keuzelijsten vereist een werkende internetverbinding.";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // voorbeelddata
            // 
            this.voorbeelddata.AutoSize = true;
            this.voorbeelddata.Enabled = false;
            this.voorbeelddata.Location = new System.Drawing.Point(395, 205);
            this.voorbeelddata.Name = "voorbeelddata";
            this.voorbeelddata.Size = new System.Drawing.Size(146, 17);
            this.voorbeelddata.TabIndex = 21;
            this.voorbeelddata.Text = "Voorbeelddata genereren";
            this.voorbeelddata.UseVisualStyleBackColor = true;
            this.voorbeelddata.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // ExportSubsetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.voorbeelddata);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkKeuzelijsten);
            this.Controls.Add(this.checkAttributes);
            this.Controls.Add(this.checkAllClasses);
            this.Controls.Add(this.ListAllClasses);
            this.Controls.Add(this.buttonImportClasses);
            this.Controls.Add(this.buttonExportXLS);
            this.Controls.Add(this.textBoxSubset);
            this.Controls.Add(this.buttonSubset);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ExportSubsetWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export OTL template";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button buttonSubset;
        private System.Windows.Forms.TextBox textBoxSubset;
        private System.Windows.Forms.Button buttonExportXLS;
        private System.Windows.Forms.Button buttonImportClasses;
        private System.Windows.Forms.ListBox ListAllClasses;
        private System.Windows.Forms.CheckBox checkAllClasses;
        private System.Windows.Forms.CheckBox checkAttributes;
        private System.Windows.Forms.CheckBox checkKeuzelijsten;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox voorbeelddata;
    }
}

