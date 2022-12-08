namespace OTLWizard.FrontEnd
{
    partial class SDXWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SDXWindow));
            this.textBoxSubset = new System.Windows.Forms.TextBox();
            this.buttonSubset = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.TextVersion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkAllClasses = new System.Windows.Forms.CheckBox();
            this.ListAllClasses = new System.Windows.Forms.ListBox();
            this.buttonImportAll = new System.Windows.Forms.Button();
            this.buttonExportSDX = new System.Windows.Forms.Button();
            this.textBoxSDX = new System.Windows.Forms.TextBox();
            this.buttonSDX = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonSDXModeNew = new System.Windows.Forms.RadioButton();
            this.radioButtonSDXModeEdit = new System.Windows.Forms.RadioButton();
            this.buttonartefact = new System.Windows.Forms.Button();
            this.textBoxArtefact = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxSubset
            // 
            this.textBoxSubset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSubset.Enabled = false;
            this.textBoxSubset.Location = new System.Drawing.Point(189, 110);
            this.textBoxSubset.Name = "textBoxSubset";
            this.textBoxSubset.Size = new System.Drawing.Size(641, 20);
            this.textBoxSubset.TabIndex = 45;
            // 
            // buttonSubset
            // 
            this.buttonSubset.Enabled = false;
            this.buttonSubset.Location = new System.Drawing.Point(15, 109);
            this.buttonSubset.Name = "buttonSubset";
            this.buttonSubset.Size = new System.Drawing.Size(168, 24);
            this.buttonSubset.TabIndex = 44;
            this.buttonSubset.Text = "Selecteer een Subset";
            this.buttonSubset.UseVisualStyleBackColor = true;
            this.buttonSubset.Click += new System.EventHandler(this.buttonSubset_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Location = new System.Drawing.Point(701, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "OTL Versie";
            // 
            // TextVersion
            // 
            this.TextVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextVersion.Enabled = false;
            this.TextVersion.Location = new System.Drawing.Point(767, 169);
            this.TextVersion.Name = "TextVersion";
            this.TextVersion.Size = new System.Drawing.Size(63, 20);
            this.TextVersion.TabIndex = 42;
            this.TextVersion.Text = "0.0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(395, 229);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Algemene instellingen";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(778, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 54);
            this.button1.TabIndex = 40;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkAllClasses
            // 
            this.checkAllClasses.AutoSize = true;
            this.checkAllClasses.Enabled = false;
            this.checkAllClasses.Location = new System.Drawing.Point(412, 248);
            this.checkAllClasses.Name = "checkAllClasses";
            this.checkAllClasses.Size = new System.Drawing.Size(129, 17);
            this.checkAllClasses.TabIndex = 38;
            this.checkAllClasses.Text = "Selecteer alle klassen";
            this.checkAllClasses.UseVisualStyleBackColor = true;
            this.checkAllClasses.CheckedChanged += new System.EventHandler(this.checkAllClasses_CheckedChanged);
            // 
            // ListAllClasses
            // 
            this.ListAllClasses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ListAllClasses.Enabled = false;
            this.ListAllClasses.FormattingEnabled = true;
            this.ListAllClasses.Location = new System.Drawing.Point(15, 229);
            this.ListAllClasses.Name = "ListAllClasses";
            this.ListAllClasses.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.ListAllClasses.Size = new System.Drawing.Size(374, 277);
            this.ListAllClasses.TabIndex = 37;
            // 
            // buttonImportAll
            // 
            this.buttonImportAll.Enabled = false;
            this.buttonImportAll.Location = new System.Drawing.Point(15, 200);
            this.buttonImportAll.Name = "buttonImportAll";
            this.buttonImportAll.Size = new System.Drawing.Size(153, 23);
            this.buttonImportAll.TabIndex = 36;
            this.buttonImportAll.Text = "Importeer bestanden";
            this.buttonImportAll.UseVisualStyleBackColor = true;
            this.buttonImportAll.Click += new System.EventHandler(this.buttonImportAll_Click);
            // 
            // buttonExportSDX
            // 
            this.buttonExportSDX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExportSDX.Enabled = false;
            this.buttonExportSDX.Location = new System.Drawing.Point(660, 489);
            this.buttonExportSDX.Name = "buttonExportSDX";
            this.buttonExportSDX.Size = new System.Drawing.Size(170, 23);
            this.buttonExportSDX.TabIndex = 35;
            this.buttonExportSDX.Text = "Exporteer XSD bestand";
            this.buttonExportSDX.UseVisualStyleBackColor = true;
            this.buttonExportSDX.Click += new System.EventHandler(this.buttonExportSDX_Click);
            // 
            // textBoxSDX
            // 
            this.textBoxSDX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSDX.Enabled = false;
            this.textBoxSDX.Location = new System.Drawing.Point(189, 82);
            this.textBoxSDX.Name = "textBoxSDX";
            this.textBoxSDX.Size = new System.Drawing.Size(641, 20);
            this.textBoxSDX.TabIndex = 34;
            // 
            // buttonSDX
            // 
            this.buttonSDX.Enabled = false;
            this.buttonSDX.Location = new System.Drawing.Point(15, 80);
            this.buttonSDX.Name = "buttonSDX";
            this.buttonSDX.Size = new System.Drawing.Size(168, 23);
            this.buttonSDX.TabIndex = 33;
            this.buttonSDX.Text = "Selecteer een XSD bestand";
            this.buttonSDX.UseVisualStyleBackColor = true;
            this.buttonSDX.Click += new System.EventHandler(this.buttonSDX_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(630, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Selecteer een XSD bestand of vertrek vanuit een subset om een OTL conforme templa" +
    "te aan te maken. Maak hieronder een keuze.";
            // 
            // radioButtonSDXModeNew
            // 
            this.radioButtonSDXModeNew.AutoSize = true;
            this.radioButtonSDXModeNew.Location = new System.Drawing.Point(15, 47);
            this.radioButtonSDXModeNew.Name = "radioButtonSDXModeNew";
            this.radioButtonSDXModeNew.Size = new System.Drawing.Size(174, 17);
            this.radioButtonSDXModeNew.TabIndex = 46;
            this.radioButtonSDXModeNew.TabStop = true;
            this.radioButtonSDXModeNew.Text = "Nieuw XSD bestand aanmaken";
            this.radioButtonSDXModeNew.UseVisualStyleBackColor = true;
            this.radioButtonSDXModeNew.CheckedChanged += new System.EventHandler(this.radioButtonSDXModeNew_CheckedChanged);
            // 
            // radioButtonSDXModeEdit
            // 
            this.radioButtonSDXModeEdit.AutoSize = true;
            this.radioButtonSDXModeEdit.Location = new System.Drawing.Point(195, 47);
            this.radioButtonSDXModeEdit.Name = "radioButtonSDXModeEdit";
            this.radioButtonSDXModeEdit.Size = new System.Drawing.Size(186, 17);
            this.radioButtonSDXModeEdit.TabIndex = 47;
            this.radioButtonSDXModeEdit.TabStop = true;
            this.radioButtonSDXModeEdit.Text = "Bestaand XSD bestand bewerken";
            this.radioButtonSDXModeEdit.UseVisualStyleBackColor = true;
            this.radioButtonSDXModeEdit.CheckedChanged += new System.EventHandler(this.radioButtonSDXModeEdit_CheckedChanged);
            // 
            // buttonartefact
            // 
            this.buttonartefact.Enabled = false;
            this.buttonartefact.Location = new System.Drawing.Point(15, 140);
            this.buttonartefact.Name = "buttonartefact";
            this.buttonartefact.Size = new System.Drawing.Size(168, 24);
            this.buttonartefact.TabIndex = 48;
            this.buttonartefact.Text = "Selecteer het geometrie artefact";
            this.buttonartefact.UseVisualStyleBackColor = true;
            this.buttonartefact.Click += new System.EventHandler(this.buttonartefact_Click);
            // 
            // textBoxArtefact
            // 
            this.textBoxArtefact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxArtefact.Enabled = false;
            this.textBoxArtefact.Location = new System.Drawing.Point(189, 143);
            this.textBoxArtefact.Name = "textBoxArtefact";
            this.textBoxArtefact.Size = new System.Drawing.Size(641, 20);
            this.textBoxArtefact.TabIndex = 49;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(174, 205);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(346, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Een internetverbinding is noodzakelijk voor de keuzelijsten te importeren";
            // 
            // SDXWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 522);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxArtefact);
            this.Controls.Add(this.buttonartefact);
            this.Controls.Add(this.radioButtonSDXModeEdit);
            this.Controls.Add(this.radioButtonSDXModeNew);
            this.Controls.Add(this.textBoxSubset);
            this.Controls.Add(this.buttonSubset);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TextVersion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkAllClasses);
            this.Controls.Add(this.ListAllClasses);
            this.Controls.Add(this.buttonImportAll);
            this.Controls.Add(this.buttonExportSDX);
            this.Controls.Add(this.textBoxSDX);
            this.Controls.Add(this.buttonSDX);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SDXWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SDXWindow";
            this.Load += new System.EventHandler(this.SDXWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSubset;
        private System.Windows.Forms.Button buttonSubset;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TextVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkAllClasses;
        private System.Windows.Forms.ListBox ListAllClasses;
        private System.Windows.Forms.Button buttonImportAll;
        private System.Windows.Forms.Button buttonExportSDX;
        private System.Windows.Forms.TextBox textBoxSDX;
        private System.Windows.Forms.Button buttonSDX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonSDXModeNew;
        private System.Windows.Forms.RadioButton radioButtonSDXModeEdit;
        private System.Windows.Forms.Button buttonartefact;
        private System.Windows.Forms.TextBox textBoxArtefact;
        private System.Windows.Forms.Label label2;
    }
}