//#define DISPHIGHCONT

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
#if DISPHIGHCONT
            //TEST CODE
            picDisplay.Image = Master.sMaster.Render(Master.RenderState.EditOrig);
#else
            //mImg = Picture;
            picDisplay.Image = mImg = Picture;
            //this.Invoke(UpdateDel);
#endif
            IsRunning = false;
        }

        public new void CloseSafe()
        {
            while (IsRunning) ;
        }
    }
}
