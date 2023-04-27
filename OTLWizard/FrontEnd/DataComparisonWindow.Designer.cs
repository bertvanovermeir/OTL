namespace OTLWizard.FrontEnd
{
    partial class DataComparisonWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataComparisonWindow));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textboxNieuweBestanden = new System.Windows.Forms.TextBox();
            this.buttonNieuweBestanden = new System.Windows.Forms.Button();
            this.buttonControleUitvoeren = new System.Windows.Forms.Button();
            this.textBoxOrigineleBestanden = new System.Windows.Forms.TextBox();
            this.buttonOrigineleBestanden = new System.Windows.Forms.Button();
            this.buttonExporteerBestanden = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBoxLegeKolommen = new System.Windows.Forms.CheckBox();
            this.checkBoxFeatid = new System.Windows.Forms.CheckBox();
            this.labelStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(1059, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 54);
            this.button1.TabIndex = 20;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1040, 34);
            this.label1.TabIndex = 21;
            this.label1.Text = "label1";
            // 
            // textboxNieuweBestanden
            // 
            this.textboxNieuweBestanden.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxNieuweBestanden.Location = new System.Drawing.Point(186, 79);
            this.textboxNieuweBestanden.Name = "textboxNieuweBestanden";
            this.textboxNieuweBestanden.Size = new System.Drawing.Size(641, 20);
            this.textboxNieuweBestanden.TabIndex = 36;
            // 
            // buttonNieuweBestanden
            // 
            this.buttonNieuweBestanden.Enabled = false;
            this.buttonNieuweBestanden.Location = new System.Drawing.Point(12, 78);
            this.buttonNieuweBestanden.Name = "buttonNieuweBestanden";
            this.buttonNieuweBestanden.Size = new System.Drawing.Size(168, 24);
            this.buttonNieuweBestanden.TabIndex = 35;
            this.buttonNieuweBestanden.Text = "Selecteer nieuwe bestanden";
            this.buttonNieuweBestanden.UseVisualStyleBackColor = true;
            this.buttonNieuweBestanden.Click += new System.EventHandler(this.buttonNieuweBestanden_Click);
            // 
            // buttonControleUitvoeren
            // 
            this.buttonControleUitvoeren.Enabled = false;
            this.buttonControleUitvoeren.Location = new System.Drawing.Point(12, 108);
            this.buttonControleUitvoeren.Name = "buttonControleUitvoeren";
            this.buttonControleUitvoeren.Size = new System.Drawing.Size(168, 23);
            this.buttonControleUitvoeren.TabIndex = 33;
            this.buttonControleUitvoeren.Text = "Controle uitvoeren";
            this.buttonControleUitvoeren.UseVisualStyleBackColor = true;
            this.buttonControleUitvoeren.Click += new System.EventHandler(this.buttonControleUitvoeren_Click);
            // 
            // textBoxOrigineleBestanden
            // 
            this.textBoxOrigineleBestanden.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOrigineleBestanden.Location = new System.Drawing.Point(186, 51);
            this.textBoxOrigineleBestanden.Name = "textBoxOrigineleBestanden";
            this.textBoxOrigineleBestanden.Size = new System.Drawing.Size(641, 20);
            this.textBoxOrigineleBestanden.TabIndex = 32;
            // 
            // buttonOrigineleBestanden
            // 
            this.buttonOrigineleBestanden.Location = new System.Drawing.Point(12, 49);
            this.buttonOrigineleBestanden.Name = "buttonOrigineleBestanden";
            this.buttonOrigineleBestanden.Size = new System.Drawing.Size(168, 23);
            this.buttonOrigineleBestanden.TabIndex = 31;
            this.buttonOrigineleBestanden.Text = "Selecteer originele bestanden";
            this.buttonOrigineleBestanden.UseVisualStyleBackColor = true;
            this.buttonOrigineleBestanden.Click += new System.EventHandler(this.buttonOrigineleBestanden_Click);
            // 
            // buttonExporteerBestanden
            // 
            this.buttonExporteerBestanden.Enabled = false;
            this.buttonExporteerBestanden.Location = new System.Drawing.Point(186, 108);
            this.buttonExporteerBestanden.Name = "buttonExporteerBestanden";
            this.buttonExporteerBestanden.Size = new System.Drawing.Size(160, 23);
            this.buttonExporteerBestanden.TabIndex = 37;
            this.buttonExporteerBestanden.Text = "Exporteer bestanden";
            this.buttonExporteerBestanden.UseVisualStyleBackColor = true;
            this.buttonExporteerBestanden.Click += new System.EventHandler(this.buttonExporteerBestanden_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 138);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(1098, 457);
            this.dataGridView1.TabIndex = 38;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // checkBoxLegeKolommen
            // 
            this.checkBoxLegeKolommen.AutoSize = true;
            this.checkBoxLegeKolommen.Location = new System.Drawing.Point(353, 113);
            this.checkBoxLegeKolommen.Name = "checkBoxLegeKolommen";
            this.checkBoxLegeKolommen.Size = new System.Drawing.Size(80, 17);
            this.checkBoxLegeKolommen.TabIndex = 39;
            this.checkBoxLegeKolommen.Text = "checkBox1";
            this.checkBoxLegeKolommen.UseVisualStyleBackColor = true;
            this.checkBoxLegeKolommen.CheckedChanged += new System.EventHandler(this.checkBoxLegeKolommen_CheckedChanged);
            // 
            // checkBoxFeatid
            // 
            this.checkBoxFeatid.AutoSize = true;
            this.checkBoxFeatid.Location = new System.Drawing.Point(586, 112);
            this.checkBoxFeatid.Margin = new System.Windows.Forms.Padding(150, 3, 3, 3);
            this.checkBoxFeatid.Name = "checkBoxFeatid";
            this.checkBoxFeatid.Size = new System.Drawing.Size(80, 17);
            this.checkBoxFeatid.TabIndex = 40;
            this.checkBoxFeatid.Text = "checkBox1";
            this.checkBoxFeatid.UseVisualStyleBackColor = true;
            this.checkBoxFeatid.CheckedChanged += new System.EventHandler(this.checkBoxFeatid_CheckedChanged);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(13, 601);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(35, 13);
            this.labelStatus.TabIndex = 41;
            this.labelStatus.Text = "label2";
            // 
            // DataComparisonWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1123, 626);
            this.ControlBox = false;
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.checkBoxFeatid);
            this.Controls.Add(this.checkBoxLegeKolommen);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonExporteerBestanden);
            this.Controls.Add(this.textboxNieuweBestanden);
            this.Controls.Add(this.buttonNieuweBestanden);
            this.Controls.Add(this.buttonControleUitvoeren);
            this.Controls.Add(this.textBoxOrigineleBestanden);
            this.Controls.Add(this.buttonOrigineleBestanden);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataComparisonWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataComparisonWindow";
            this.Load += new System.EventHandler(this.DataComparisonWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textboxNieuweBestanden;
        private System.Windows.Forms.Button buttonNieuweBestanden;
        private System.Windows.Forms.Button buttonControleUitvoeren;
        private System.Windows.Forms.TextBox textBoxOrigineleBestanden;
        private System.Windows.Forms.Button buttonOrigineleBestanden;
        private System.Windows.Forms.Button buttonExporteerBestanden;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox checkBoxLegeKolommen;
        private System.Windows.Forms.CheckBox checkBoxFeatid;
        private System.Windows.Forms.Label labelStatus;
    }
}