namespace Picasso_DissectGUI
{
    partial class MainWindow
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnMaster = new System.Windows.Forms.Button();
            this.btnSeeOrig = new System.Windows.Forms.Button();
            this.btnSeeLeft = new System.Windows.Forms.Button();
            this.btnSeeEdit = new System.Windows.Forms.Button();
            this.btnSeeOver = new System.Windows.Forms.Button();
            this.btnSeeShapes = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnWriteLog = new System.Windows.Forms.Button();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnWriteLog);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.btnSeeShapes);
            this.splitContainer1.Panel1.Controls.Add(this.btnSeeOver);
            this.splitContainer1.Panel1.Controls.Add(this.btnSeeEdit);
            this.splitContainer1.Panel1.Controls.Add(this.btnSeeLeft);
            this.splitContainer1.Panel1.Controls.Add(this.btnSeeOrig);
            this.splitContainer1.Panel1.Controls.Add(this.btnMaster);
            this.splitContainer1.Panel1.Controls.Add(this.btnLoad);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.picDisplay);
            this.splitContainer1.Size = new System.Drawing.Size(666, 477);
            this.splitContainer1.SplitterDistance = 139;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(116, 34);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnMaster
            // 
            this.btnMaster.Location = new System.Drawing.Point(12, 52);
            this.btnMaster.Name = "btnMaster";
            this.btnMaster.Size = new System.Drawing.Size(116, 34);
            this.btnMaster.TabIndex = 1;
            this.btnMaster.Text = "Run Master";
            this.btnMaster.UseVisualStyleBackColor = true;
            this.btnMaster.Click += new System.EventHandler(this.btnMaster_Click);
            // 
            // btnSeeOrig
            // 
            this.btnSeeOrig.Location = new System.Drawing.Point(12, 92);
            this.btnSeeOrig.Name = "btnSeeOrig";
            this.btnSeeOrig.Size = new System.Drawing.Size(116, 34);
            this.btnSeeOrig.TabIndex = 2;
            this.btnSeeOrig.Text = "See Original";
            this.btnSeeOrig.UseVisualStyleBackColor = true;
            this.btnSeeOrig.Click += new System.EventHandler(this.btnSeeOrig_Click);
            // 
            // btnSeeLeft
            // 
            this.btnSeeLeft.Location = new System.Drawing.Point(12, 132);
            this.btnSeeLeft.Name = "btnSeeLeft";
            this.btnSeeLeft.Size = new System.Drawing.Size(116, 34);
            this.btnSeeLeft.TabIndex = 3;
            this.btnSeeLeft.Text = "See Leftover";
            this.btnSeeLeft.UseVisualStyleBackColor = true;
            this.btnSeeLeft.Click += new System.EventHandler(this.btnSeeLeft_Click);
            // 
            // btnSeeEdit
            // 
            this.btnSeeEdit.Location = new System.Drawing.Point(12, 172);
            this.btnSeeEdit.Name = "btnSeeEdit";
            this.btnSeeEdit.Size = new System.Drawing.Size(116, 34);
            this.btnSeeEdit.TabIndex = 4;
            this.btnSeeEdit.Text = "See Edited";
            this.btnSeeEdit.UseVisualStyleBackColor = true;
            this.btnSeeEdit.Click += new System.EventHandler(this.btnSeeEdit_Click);
            // 
            // btnSeeOver
            // 
            this.btnSeeOver.Location = new System.Drawing.Point(12, 212);
            this.btnSeeOver.Name = "btnSeeOver";
            this.btnSeeOver.Size = new System.Drawing.Size(116, 34);
            this.btnSeeOver.TabIndex = 5;
            this.btnSeeOver.Text = "See Overlay";
            this.btnSeeOver.UseVisualStyleBackColor = true;
            this.btnSeeOver.Click += new System.EventHandler(this.btnSeeOver_Click);
            // 
            // btnSeeShapes
            // 
            this.btnSeeShapes.Location = new System.Drawing.Point(12, 252);
            this.btnSeeShapes.Name = "btnSeeShapes";
            this.btnSeeShapes.Size = new System.Drawing.Size(116, 34);
            this.btnSeeShapes.TabIndex = 6;
            this.btnSeeShapes.Text = "See Shapes";
            this.btnSeeShapes.UseVisualStyleBackColor = true;
            this.btnSeeShapes.Click += new System.EventHandler(this.btnSeeShapes_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 292);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(116, 34);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save Current";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnWriteLog
            // 
            this.btnWriteLog.Location = new System.Drawing.Point(12, 332);
            this.btnWriteLog.Name = "btnWriteLog";
            this.btnWriteLog.Size = new System.Drawing.Size(116, 34);
            this.btnWriteLog.TabIndex = 8;
            this.btnWriteLog.Text = "Write To Log";
            this.btnWriteLog.UseVisualStyleBackColor = true;
            this.btnWriteLog.Click += new System.EventHandler(this.btnWriteLog_Click);
            // 
            // picDisplay
            // 
            this.picDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picDisplay.Location = new System.Drawing.Point(0, 0);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(523, 477);
            this.picDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDisplay.TabIndex = 0;
            this.picDisplay.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 477);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSeeShapes;
        private System.Windows.Forms.Button btnSeeOver;
        private System.Windows.Forms.Button btnSeeEdit;
        private System.Windows.Forms.Button btnSeeLeft;
        private System.Windows.Forms.Button btnSeeOrig;
        private System.Windows.Forms.Button btnMaster;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnWriteLog;
        private System.Windows.Forms.PictureBox picDisplay;
    }
}