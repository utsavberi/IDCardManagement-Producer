namespace IDCardManagement
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            //this.ApplicantDataSet = new IDCardManagement.ApplicantDataSet();
            this.utsavFinal634792753739040581extrapicBindingSource = new System.Windows.Forms.BindingSource(this.components);
            //this.utsavFinal634792753739040581extrapicTableAdapter = new IDCardManagement.ApplicantDataSetTableAdapters.utsavFinal634792753739040581extrapicTableAdapter();
            this.groupBox1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.enrolledLabel = new System.Windows.Forms.Label();
            this.attendedLabel = new System.Windows.Forms.Label();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            //((System.ComponentModel.ISupportInitialize)(this.ApplicantDataSet)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.utsavFinal634792753739040581extrapicBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ApplicantDataSet
            // 
            //this.ApplicantDataSet.DataSetName = "ApplicantDataSet";
            //this.ApplicantDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // utsavFinal634792753739040581extrapicBindingSource
            // 
            //this.utsavFinal634792753739040581extrapicBindingSource.DataMember = "utsavFinal634792753739040581extrapic";
            //this.utsavFinal634792753739040581extrapicBindingSource.DataSource = this.ApplicantDataSet;
            // 
            // utsavFinal634792753739040581extrapicTableAdapter
            // 
           // this.utsavFinal634792753739040581extrapicTableAdapter.ClearBeforeFill = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoScroll = true;
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.attendedLabel);
            this.groupBox1.Controls.Add(this.enrolledLabel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(715, 434);
            this.groupBox1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Silver;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(715, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // enrolledLabel
            // 
            this.enrolledLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enrolledLabel.AutoSize = true;
            this.enrolledLabel.Location = new System.Drawing.Point(391, 54);
            this.enrolledLabel.Name = "enrolledLabel";
            this.enrolledLabel.Size = new System.Drawing.Size(233, 15);
            this.enrolledLabel.TabIndex = 0;
            this.enrolledLabel.Text = "Total No. of Applicants Enrolled for Event :";
            // 
            // attendedLabel
            // 
            this.attendedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.attendedLabel.AutoSize = true;
            this.attendedLabel.Location = new System.Drawing.Point(391, 83);
            this.attendedLabel.Name = "attendedLabel";
            this.attendedLabel.Size = new System.Drawing.Size(244, 15);
            this.attendedLabel.TabIndex = 1;
            this.attendedLabel.Text = "Total No. of Applicants who attended Event :";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 434);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Form1_Load);
            //((System.ComponentModel.ISupportInitialize)(this.ApplicantDataSet)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.utsavFinal634792753739040581extrapicBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource utsavFinal634792753739040581extrapicBindingSource;
       // private ApplicantDataSet ApplicantDataSet;
        //private ApplicantDataSetTableAdapters.utsavFinal634792753739040581extrapicTableAdapter utsavFinal634792753739040581extrapicTableAdapter;
        private System.Windows.Forms.Panel groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Label attendedLabel;
        private System.Windows.Forms.Label enrolledLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}