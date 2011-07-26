using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

/*
 * Author: David M
 * Email: scnerd@gmail.com
 * Picasso summary: Picasso is meant to be an image invention algorithm.
 * Primarily, it has three tasks:
 * 1) Tear apart any image into simplified geometric shapes, and save the pattern to a standard output
 * 2) Analyze those outputs to look for patterns: common shapes w/in shapes, common shapes together, etc
 * 3) Generate new images based on those similar patterns by piecing together often-used shape combinations
 * Note that each log file is stored with a bitmap copy of the original image in a sub-directory, so
 * when doing the final complication of the image, it might be helpful to analyze where the how the
 * shapes originally look, with image noise and everything, to produce a more realistic result.
 * 
 * This file's purpose: To perform basic, standard operations on the master image.
 * Namely, this reduces the image to a limited set of colors and a standard resolution.
 * 
 * Comments:
 * Bugs: 
 * 
 */

namespace Picasso
{
    internal class ImgManip
    {
        private Bitmap mBase, mImg;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Img"></param>
        internal ImgManip(ref Bitmap Img)
        {
            mBase = Img;
            mImg = (Bitmap)Img.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ColDetail"></param>
        internal void ReduceColors(int ColDetail)
        {
            Color c;
            Func<byte, byte> Round = i => (byte)(i + (i % ColDetail > ColDetail/2 ? ColDetail - i : i - ColDetail));
            for (int y = 0; y < mBase.Height; y++)
            {
                for (int x = 0; x < mBase.Width; x++)
                {
                    c = mBase.GetPixel(x, y);
                    mImg.SetPixel(x, y, Color.FromArgb(Round(c.R), Round(c.G), Round(c.B)));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ResDetail"></param>
        internal void ReduceGrid(int ResDetail)
        {
            float i = (float)ResDetail/(float)(mImg.Height > mImg.Width?mImg.Height:mImg.Width);
            Bitmap SmallRes = new Bitmap((int)(mImg.Width * i), (int)(mImg.Height * i));
            Graphics g = Graphics.FromImage(SmallRes);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(mImg, new System.Drawing.Rectangle(new Point(0,0),SmallRes.Size));
            g.Flush();
            g.Dispose();
            mImg = SmallRes;
        }

        /// <summary>
        /// 
        /// </summary>
        internal Bitmap GetImage
        { get { return mImg; } }
    }
}
