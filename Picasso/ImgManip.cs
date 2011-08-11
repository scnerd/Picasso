using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AForge.Imaging.ColorReduction;

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
        //internal void ReduceColors(int ColDetail)
        //{
        //    Color c;
        //    //Func<byte, byte> Round = i => (byte)(i + (i % ColDetail > ColDetail/2 ? ColDetail - i : i - ColDetail));
        //    for (int y = 0; y < mBase.Height; y++)
        //    {
        //        for (int x = 0; x < mBase.Width; x++)
        //        {
        //            //c = mBase.GetPixel(x, y);
        //            //Color c = Color.FromArgb(Round(c.R), Round(c.G), Round(c.B));
        //            c = RoundHue(mBase.GetPixel(x, y), ColDetail);
        //            mImg.SetPixel(x, y, c);
        //        }
        //    }
        //}
        internal void ReduceColors(int ColDetail)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ResDetail"></param>
        internal double ReduceGrid(int ResDetail)
        {
            float i = (float)ResDetail/(float)(mImg.Height > mImg.Width?mImg.Height:mImg.Width);
            Bitmap SmallRes = new Bitmap((int)Math.Floor(mImg.Width * i), (int)Math.Floor(mImg.Height * i));
            Graphics g = Graphics.FromImage(SmallRes);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(mImg, new System.Drawing.Rectangle(new Point(0,0),SmallRes.Size));
            g.Flush();
            g.Dispose();
            mImg = SmallRes;
            return i;
        }

        /// <summary>
        /// 
        /// </summary>
        internal Bitmap GetImage
        { get { return mImg; } }


        private Color RoundHue(Color c, int ColorDetail)
        {
            //See HSB Definitions.txt for details
            int Red = c.R, Green = c.G, Blue = c.B, N;
            int Max = Math.Max(Red, Math.Max(Green, Blue)),
                Min = Math.Min(Red, Math.Min(Green, Blue));
            if (Max == Min) return c;
            Func<int, int> Round = i => ((int)Math.Round((double)i / (double)ColorDetail)) * ColorDetail;
            //Find N, round it off, then reverse to find the one middle color
            if (Red == Max && Blue == Min)
            { // n%60 = (g - b) * 60 / (r - b), Base = 0
                N = Round((Green - Blue) * 60 / (Red - Blue));
                if (N <= 60)
                    Green = Blue + N / 60 * (Red - Blue);
                else
                {
                    Green = Red;
                    //Red = Blue + (60 - N % 60) / 60 * (Green - Blue);
                    //This operation is handled by the fall-through below
                    //Now that Green does == Max
                }
            }
            if (Green == Max && Blue == Min)
            { // n%60 = 60 - ( (r - b) * 60 / (g - b) ), Base = 60
                N = Round(60 - (Red - Blue) * 60 / (Green - Blue));
                if(N <= 60)
                    Red = Blue + (60 - N) / 60 * (Green - Blue);
                else
                    Red = Blue;
            }
            if (Green == Max && Red == Min) 
            { // n%60 = (b - r) * 60 / (g - r), Base = 120
                N = Round(Blue - Red) * 60 / (Green - Red);
                if (N <= 60)
                    Blue = Red + N / 60 * (Green - Red);
                else
                    Blue = Green;
            }
            if (Blue == Max && Red == Min)
            { // n%60 = 60 - ( ( g - r) * 60 / ( b - r))
                N = Round(60 - (Green - Red) * 60 / (Blue - Red));
                if (N <= 60)
                    Green = Red + (60 - N % 60) / 60 * (Blue - Red);
                else
                    Green = Red;
            }
            if (Blue == Max && Green == Min)
            {
                N = Round(Red - Green) * 60 / (Blue - Green);
                if (N <= 60)
                    Red = Green + N / 60 * (Blue - Green);
                else
                    Red = Blue;
            }
            if (Red == Max && Green == Min)
            {
                N = Round(60 - (Blue - Green) * 60 / (Red - Green));
                if (N <= 60)
                    Blue = Green + (60 - N % 60) / 60 * (Red - Green);
                else
                {
                    Blue = Green;
                    //Repeat, because we need the first if again
                    return RoundHue(Color.FromArgb(Red, Green, Blue), ColorDetail);
                }
            }

            return Color.FromArgb(Red, Green, Blue);
        }
    }
}
