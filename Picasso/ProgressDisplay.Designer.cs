namespace Picasso
{
    partial class ProgressDisplay
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
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.lblChildren = new System.Windows.Forms.Label();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.lblPx = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // barProgress
            // 
            this.barProgress.Location = new System.Drawing.Point(12, 12);
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(260, 32);
            this.barProgress.TabIndex = 0;
            // 
            // lblChildren
            // 
            this.lblChildren.AutoSize = true;
            this.lblChildren.Location = new System.Drawing.Point(12, 83);
            this.lblChildren.Name = "lblChildren";
            this.lblChildren.Size = new System.Drawing.Size(57, 13);
            this.lblChildren.TabIndex = 1;
            this.lblChildren.Text = "Children: 0";
            // 
            // lblRemaining
            // 
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Location = new System.Drawing.Point(12, 108);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(138, 13);
            this.lblRemaining.TabIndex = 2;
            this.lblRemaining.Text = "Estimated Time Remaining: ";
            // 
            // lblPx
            // 
            this.lblPx.AutoSize = true;
            this.lblPx.Location = new System.Drawing.Point(12, 56);
            this.lblPx.Name = "lblPx";
            this.lblPx.Size = new System.Drawing.Size(57, 13);
            this.lblPx.TabIndex = 3;
            this.lblPx.Text = "Pixels: 0/0";
            // 
            // ProgressDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 132);
            this.Controls.Add(this.lblPx);
            this.Controls.Add(this.lblRemaining);
            this.Controls.Add(this.lblChildren);
            this.Controls.Add(this.barProgress);
            this.Name = "ProgressDisplay";
            this.Text = "Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Label lblChildren;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.Label lblPx;
    }
}