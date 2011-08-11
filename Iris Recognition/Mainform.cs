using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace CannyEdgeDetection
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }
        Canny CannyData;

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bitmap files (*.bmp)|*.bmp|PNG files (*.png)|*.png|TIFF files (*.tif)|*.tif|JPEG files (*.jpg)|*.jpg |All files (*.*)|*.*";
            ofd.FilterIndex = 5;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    IrisImage.Image = Bitmap.FromFile(ofd.FileName);

                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void selectFullImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            TimeSpan dt3 = new TimeSpan();

            dt1 = DateTime.Now;
            pg1.Value = 0;
            CannyData = new Canny ((Bitmap)IrisImage.Image);
            pg1.Value = 10;

            HystThreshImage.Image = CannyData.DisplayImage(CannyData.NonMax);
            GNL.Image = CannyData.DisplayImage(CannyData.GNL);
            GNH.Image = CannyData.DisplayImage(CannyData.GNH);
            CannyEdges.Image = CannyData.DisplayImage(CannyData.EdgeMap);

            dt2 = DateTime.Now;
            dt3 = dt2 - dt1;
            time.Text = dt3.ToString();
            pg1.Value = 100;
        }

        private void Mainform_Load(object sender, EventArgs e)
        {

        }

        private void BtnCannyEdgeDetect_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            TimeSpan dt3 = new TimeSpan();
            float TH, TL, Sigma;
            int MaskSize;

            dt1 = DateTime.Now;
            pg1.Value = 0;
            TH = (float)Convert.ToDouble(TxtTH.Text);
            TL = (float)Convert.ToDouble(TxtTL.Text);

            MaskSize = Convert.ToInt32(TxtGMask.Text);
            Sigma = (float)Convert.ToDouble(TxtSigma.Text);
            pg1.Value = 10;
            CannyData = new Canny((Bitmap)IrisImage.Image,TH,TL,MaskSize,Sigma );

            HystThreshImage.Image = CannyData.DisplayImage(CannyData.NonMax);

            GaussianFilteredImage.Image = CannyData.DisplayImage(CannyData.FilteredImage);

            GNL.Image = CannyData.DisplayImage(CannyData.GNL);

            GNH.Image = CannyData.DisplayImage(CannyData.GNH);

            CannyEdges.Image = CannyData.DisplayImage(CannyData.EdgeMap);

            dt2 = DateTime.Now;
            dt3 = dt2 - dt1;
            time.Text = dt3.ToString();
            pg1.Value = 100;
        }
    }
}