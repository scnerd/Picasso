namespace CannyEdgeDetection
{
    partial class Mainform
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.CNMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectFullImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pg1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.time = new System.Windows.Forms.ToolStripStatusLabel();
            this.IrisImage = new System.Windows.Forms.PictureBox();
            this.HystThreshImage = new System.Windows.Forms.PictureBox();
            this.GNL = new System.Windows.Forms.PictureBox();
            this.GNH = new System.Windows.Forms.PictureBox();
            this.CannyEdges = new System.Windows.Forms.PictureBox();
            this.TxtTH = new System.Windows.Forms.TextBox();
            this.TxtTL = new System.Windows.Forms.TextBox();
            this.BtnCannyEdgeDetect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.GaussianFilteredImage = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtSigma = new System.Windows.Forms.TextBox();
            this.TxtGMask = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.CNMenuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IrisImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HystThreshImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GNL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GNH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CannyEdges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GaussianFilteredImage)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripLabel4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(852, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(33, 22);
            this.toolStripLabel1.Text = "Open";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(25, 22);
            this.toolStripLabel4.Text = "Exit";
            // 
            // CNMenuStrip
            // 
            this.CNMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFullImageToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.CNMenuStrip.Name = "CNMenuStrip";
            this.CNMenuStrip.Size = new System.Drawing.Size(167, 48);
            // 
            // selectFullImageToolStripMenuItem
            // 
            this.selectFullImageToolStripMenuItem.Name = "selectFullImageToolStripMenuItem";
            this.selectFullImageToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.selectFullImageToolStripMenuItem.Text = "Select Full Image";
            this.selectFullImageToolStripMenuItem.Click += new System.EventHandler(this.selectFullImageToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.pg1,
            this.toolStripStatusLabel3,
            this.time});
            this.statusStrip1.Location = new System.Drawing.Point(0, 543);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(852, 22);
            this.statusStrip1.TabIndex = 40;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel1.Text = "Status :";
            // 
            // pg1
            // 
            this.pg1.Name = "pg1";
            this.pg1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(93, 17);
            this.toolStripStatusLabel3.Text = "Processing Time : ";
            // 
            // time
            // 
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(69, 17);
            this.time.Text = "HH:MM:SSSS";
            // 
            // IrisImage
            // 
            this.IrisImage.ContextMenuStrip = this.CNMenuStrip;
            this.IrisImage.Location = new System.Drawing.Point(22, 53);
            this.IrisImage.Name = "IrisImage";
            this.IrisImage.Size = new System.Drawing.Size(256, 196);
            this.IrisImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.IrisImage.TabIndex = 41;
            this.IrisImage.TabStop = false;
            // 
            // HystThreshImage
            // 
            this.HystThreshImage.Location = new System.Drawing.Point(563, 53);
            this.HystThreshImage.Name = "HystThreshImage";
            this.HystThreshImage.Size = new System.Drawing.Size(256, 196);
            this.HystThreshImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.HystThreshImage.TabIndex = 42;
            this.HystThreshImage.TabStop = false;
            // 
            // GNL
            // 
            this.GNL.Location = new System.Drawing.Point(301, 333);
            this.GNL.Name = "GNL";
            this.GNL.Size = new System.Drawing.Size(256, 196);
            this.GNL.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.GNL.TabIndex = 43;
            this.GNL.TabStop = false;
            // 
            // GNH
            // 
            this.GNH.Location = new System.Drawing.Point(19, 333);
            this.GNH.Name = "GNH";
            this.GNH.Size = new System.Drawing.Size(256, 196);
            this.GNH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.GNH.TabIndex = 44;
            this.GNH.TabStop = false;
            // 
            // CannyEdges
            // 
            this.CannyEdges.Location = new System.Drawing.Point(563, 333);
            this.CannyEdges.Name = "CannyEdges";
            this.CannyEdges.Size = new System.Drawing.Size(256, 196);
            this.CannyEdges.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CannyEdges.TabIndex = 45;
            this.CannyEdges.TabStop = false;
            // 
            // TxtTH
            // 
            this.TxtTH.Location = new System.Drawing.Point(22, 282);
            this.TxtTH.Name = "TxtTH";
            this.TxtTH.Size = new System.Drawing.Size(38, 20);
            this.TxtTH.TabIndex = 46;
            this.TxtTH.Text = "20";
            // 
            // TxtTL
            // 
            this.TxtTL.Location = new System.Drawing.Point(127, 282);
            this.TxtTL.Name = "TxtTL";
            this.TxtTL.Size = new System.Drawing.Size(41, 20);
            this.TxtTL.TabIndex = 47;
            this.TxtTL.Text = "10";
            // 
            // BtnCannyEdgeDetect
            // 
            this.BtnCannyEdgeDetect.Location = new System.Drawing.Point(474, 263);
            this.BtnCannyEdgeDetect.Name = "BtnCannyEdgeDetect";
            this.BtnCannyEdgeDetect.Size = new System.Drawing.Size(75, 36);
            this.BtnCannyEdgeDetect.TabIndex = 48;
            this.BtnCannyEdgeDetect.Text = "Canny";
            this.BtnCannyEdgeDetect.UseVisualStyleBackColor = true;
            this.BtnCannyEdgeDetect.Click += new System.EventHandler(this.BtnCannyEdgeDetect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 263);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "High Threshold TH";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Low Threshold TL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 51;
            this.label3.Text = "Input Image";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(560, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Non Max Suppressed Image";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 317);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 53;
            this.label5.Text = "Strong Edges";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(298, 317);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 54;
            this.label6.Text = "Weak Edges";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(571, 317);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 55;
            this.label7.Text = "Final Canny Edges";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(290, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 13);
            this.label8.TabIndex = 57;
            this.label8.Text = "Gaussian Filtered Image";
            // 
            // GaussianFilteredImage
            // 
            this.GaussianFilteredImage.Location = new System.Drawing.Point(293, 53);
            this.GaussianFilteredImage.Name = "GaussianFilteredImage";
            this.GaussianFilteredImage.Size = new System.Drawing.Size(256, 196);
            this.GaussianFilteredImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.GaussianFilteredImage.TabIndex = 56;
            this.GaussianFilteredImage.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(403, 263);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 61;
            this.label9.Text = "Sigma";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(298, 263);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 13);
            this.label10.TabIndex = 60;
            this.label10.Text = "Gaussian Mask Size";
            // 
            // TxtSigma
            // 
            this.TxtSigma.Location = new System.Drawing.Point(406, 282);
            this.TxtSigma.Name = "TxtSigma";
            this.TxtSigma.Size = new System.Drawing.Size(41, 20);
            this.TxtSigma.TabIndex = 59;
            this.TxtSigma.Text = "1";
            // 
            // TxtGMask
            // 
            this.TxtGMask.Location = new System.Drawing.Point(301, 282);
            this.TxtGMask.Name = "TxtGMask";
            this.TxtGMask.Size = new System.Drawing.Size(38, 20);
            this.TxtGMask.TabIndex = 58;
            this.TxtGMask.Text = "5";
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 565);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.TxtSigma);
            this.Controls.Add(this.TxtGMask);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.GaussianFilteredImage);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CannyEdges);
            this.Controls.Add(this.HystThreshImage);
            this.Controls.Add(this.TxtTL);
            this.Controls.Add(this.BtnCannyEdgeDetect);
            this.Controls.Add(this.TxtTH);
            this.Controls.Add(this.IrisImage);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.GNH);
            this.Controls.Add(this.GNL);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Mainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Canny Edge Detection";
            this.Load += new System.EventHandler(this.Mainform_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.CNMenuStrip.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IrisImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HystThreshImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GNL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GNH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CannyEdges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GaussianFilteredImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ContextMenuStrip CNMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectFullImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar pg1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel time;
        private System.Windows.Forms.PictureBox IrisImage;
        private System.Windows.Forms.PictureBox HystThreshImage;
        private System.Windows.Forms.PictureBox GNL;
        private System.Windows.Forms.PictureBox GNH;
        private System.Windows.Forms.PictureBox CannyEdges;
        private System.Windows.Forms.TextBox TxtTH;
        private System.Windows.Forms.TextBox TxtTL;
        private System.Windows.Forms.Button BtnCannyEdgeDetect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox GaussianFilteredImage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TxtSigma;
        private System.Windows.Forms.TextBox TxtGMask;
    }
}

