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
    public partial class ProgressDisplay : Form
    {
        private double mProg = -1d;
        private int mScannedPx = 0, mTotPx = 0, mChildren = 0;
        Timer mTick = new Timer();
        System.Diagnostics.Stopwatch mTime;

        public ProgressDisplay()
        {
            //mProg = Progress;
            //mChildren = Children;
            InitializeComponent();
            mTick.Interval = 30;
            mTick.Tick += new EventHandler(mTick_Tick);
            mTick.Start();
            mTime = new System.Diagnostics.Stopwatch();
            mTime.Start();
        }

        private void mTick_Tick(object sender, EventArgs e)
        {
            if (mProg != -1d)
            {
                double d;
                barProgress.Value = (int)((d = mProg) * 100d);
                lblPx.Text = "Pixels: " + mScannedPx.ToString() + "/" + mTotPx.ToString();
                lblChildren.Text = "Children: " + mChildren.ToString();
                lblRemaining.Text = "Estimated Time Remaining: " + Estimate(d);
            }
            this.Update();
        }

        private string Estimate(double d)
        {
            long mil = (long)((double)mTime.ElapsedMilliseconds / d * (1d - d));
            int Min = (int)Math.Floor(mil / (decimal)60000);
            int Sec = (int)Math.Floor((mil % 60000) / (decimal)1000);
            return Min.ToString() + ":" + Sec.ToString() + ":" + (mil % 1000).ToString();
        }

        internal void Update(int ScannedPx, int TotPx, int AddChildren)
        {
            if (TotPx != 0)
                mProg = (double)ScannedPx / (double)TotPx;
            mScannedPx = ScannedPx;
            mTotPx = TotPx;
            mChildren += AddChildren;
            mTick_Tick(this, null);
        }

    }
}
