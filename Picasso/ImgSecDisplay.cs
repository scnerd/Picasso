using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Picasso
{
    public partial class frmImgSecDisplay : Form
    {
        private Image mImg;
        private Action UpdateDel;
        private bool IsRunning = false;

        public frmImgSecDisplay()
        {
            InitializeComponent();
            UpdateDel = new Action(this.Update);
        }

        internal void DisplayImage(Bitmap Picture)
        {
            IsRunning = true;
            mImg = Picture;
            picDisplay.Image = mImg;
            //this.Invoke(UpdateDel);
            IsRunning = false;
        }

        public new void CloseSafe()
        {
            while (IsRunning) ;
        }
    }
}
