using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Picasso;
using System.Diagnostics;

namespace Picasso_DissectGUI
{
    public partial class MainWindow : Form
    {
        Image ToDisplay, BaseImg;
        string ImgPath;
        Master M;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImgPath = diag.FileName;
                BaseImg = Image.FromFile(ImgPath);
            }
            picDisplay.Image = BaseImg;
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            M = new Master(ImgPath);
            int ChildrenCount;
            Stopwatch Watch = new Stopwatch();
            Watch.Start();
            M.GenerateChildren(out ChildrenCount);
            Watch.Stop();
            MessageBox.Show("Generated " + ChildrenCount + " in " + Watch.ElapsedMilliseconds + " ms");
            DisplayMaster(Master.RenderState.FullOverlay);
        }

        private void btnSeeOrig_Click(object sender, EventArgs e)
        {
            DisplayMaster(Master.RenderState.Original);
        }

        private void btnSeeLeft_Click(object sender, EventArgs e)
        {
            DisplayMaster(Master.RenderState.Negative);
        }

        private void btnSeeEdit_Click(object sender, EventArgs e)
        {
            DisplayMaster(Master.RenderState.EditOrig);
        }

        private void btnSeeOver_Click(object sender, EventArgs e)
        {
            DisplayMaster(Master.RenderState.FullOverlay);
        }

        private void btnSeeShapes_Click(object sender, EventArgs e)
        {
            DisplayMaster(Master.RenderState.Base1Shapes);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ToDisplay.Save(diag.FileName);
            }
        }

        private void btnWriteLog_Click(object sender, EventArgs e)
        {

        }

        private void DisplayMaster(Master.RenderState State)
        {
            ToDisplay = M.Render(Master.RenderState.FullOverlay);
            picDisplay.Image = ToDisplay;
        }
    }
}
