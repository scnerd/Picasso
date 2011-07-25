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
 * This file's purpose: 
 * 
 * Comments:
 * Bugs: 
 * 
 */

namespace Picasso
{
    public class ImageSection
    {
        protected Point mMasterOrigin;
        protected Bitmap mBaseImage,
            mAlpha;
        protected Color mMedian;


        /// <summary>
        /// 
        /// </summary>
        private ImageSection(){}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Origin"></param>
        /// <param name="Image"></param>
        /// <param name="Alpha"></param>
        public ImageSection(Point Origin, Bitmap Image, Bitmap Alpha, Color Median)
        {
            mMasterOrigin = Origin;
            mBaseImage = Image;
            mAlpha = Alpha;
            mMedian = Median;
        }

        /// <summary>
        /// 
        /// </summary>
        internal Size Size
        { get { return mBaseImage.Size; } }

        /// <summary>
        /// 
        /// </summary>
        internal Point Location
        { get { return mMasterOrigin; } }

        /// <summary>
        /// 
        /// </summary>
        internal int Height
        { get { return this.Size.Height; } }

        /// <summary>
        /// 
        /// </summary>
        internal int Width
        { get { return this.Size.Width; } }

        /// <summary>
        /// 
        /// </summary>
        internal Color MedianCol
        { get { return mMedian; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal Point[] PixelsUsed()
        {
            List<Point> Used = new List<Point>();
            for (int y = 0; y < mAlpha.Height; y++)
                for (int x = 0; x < mAlpha.Width; x++)
                    if (mAlpha.GetPixel(x, y) == Constants.ALPHA_FULL) // A = R = G = B = 255
                        Used.Add(new Point(mMasterOrigin.X + x, mMasterOrigin.Y + y));
            return Used.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static explicit operator ImageSection(Bitmap b)
        {
            ImageSection ImgS = new ImageSection();
            ImgS.mMasterOrigin = new Point(0,0);
            ImgS.mBaseImage = b;
            ImgS.mAlpha = (Bitmap)b.Clone();
            Graphics.FromImage(ImgS.mAlpha).Clear(Color.FromArgb(0xFF,0xFF,0xFF,0xFF));
            return ImgS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        internal Color GetPixel(int X, int Y)
        { return (new System.Drawing.Rectangle(this.Location, this.Size).Contains(X, Y) && mAlpha.GetPixel(X, Y).A == 255 ? mBaseImage.GetPixel(X, Y) : Constants.ALPHA_EMPTY); }

        /// <summary>
        /// 
        /// </summary>
        internal Bitmap Alpha
        { get { return mAlpha; } }
    }
}
