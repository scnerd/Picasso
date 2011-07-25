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
    internal class Ellipse : Child
    {
        private const int ANGLETRYCOUNT = 20; // How many angles to try for best fit
        private double mRotation, mEllWidth, mEllHeight;

        public Ellipse(Child Parent, ImageSection ImgSec)
            : base(Parent, ImgSec)
        {
            this.HardFit();
        }

        internal new static double ApproxFit(ImageSection ImgSec)
        {
            //double Fit = 0d;
            //return Fit;

            //Just return circle from top to bottom, left to right

            return ImgSec.Width / 2 * ImgSec.Height / 2 * Math.PI - ImgSec.PixelsUsed().Length;
        }

        private void HardFit()
        {
            /*
             * Rotate the rectangle so that top-left is centered at the top with bot-right at the bottom center. This angle is BaseTheta
             * Find the min/max x/y
             * Use those and find the efficiency of that ellipse
             * Rotate by PI/2/ANGLETRYCOUNT for PI/2 rad's (90 degs), running this same algorithm for each rotation
             * Use the most efficient, using -(BaseTheta+Theta) as the rotation of the ellipse
             */

            PointF[] Pts = (PointF[])mImgSec.PixelsUsed().SelectMany(p => new PointF[]{new PointF((float)p.X, (float)p.Y)});
            System.Drawing.Drawing2D.Matrix BaseRotator = new System.Drawing.Drawing2D.Matrix(), StepRotate = new System.Drawing.Drawing2D.Matrix();
            // |\ = ATan(Width/Height)  so |\   |
            // | \                         | \  |
            // |  \                        |  \ |
            // |___\                       |___\| = -Atan(W/H)

            float BaseTheta = (float)(-Math.Atan((double)mImgSec.Width / (double)mImgSec.Height));
            BaseRotator.Rotate(BaseTheta);
            StepRotate.Rotate((float)Math.PI / 2f / (float)ANGLETRYCOUNT);
            BaseRotator.TransformPoints(Pts);
            double BestFit = 0d,temp;
            int BestChoice = -1;
            SizeF BestDim = new SizeF(),tempDim;
            for(int i = 0; i < ANGLETRYCOUNT - 1; i++, StepRotate.TransformPoints(Pts))
            {
                if((temp = GetFit(Pts, out tempDim)) > BestFit)
                {BestChoice = i; BestFit = temp;BestDim = tempDim;}
            }

            mRotation = -(BaseTheta+BestChoice*Math.PI / 2d / (double)ANGLETRYCOUNT);
            mEllWidth = BestDim.Width;
            mEllHeight = BestDim.Height;
            return;
        }

        private double GetFit(PointF[] Pts, out SizeF EllipseDims)
        {
            float MinX, MinY, MaxX, MaxY;
            if (Pts.Length > 0)
            {
                MinX = MaxX = Pts[0].X;
                MinY = MaxY = Pts[0].Y;
            }
            else { EllipseDims = new SizeF(0f, 0f); return 0d; }
            Action<PointF> CheckMins = f =>
            { MinX = Math.Min(MinX, f.X); MinY = Math.Min(MinY, f.Y); MaxX = Math.Max(MaxX, f.X); MaxY = Math.Max(MaxY, f.Y); };
            for (int i = 1; i < Pts.Length; i++) CheckMins(Pts[i]);
            RectangleF BoundBox = new RectangleF(MinX, MinY, MaxX - MinX + 1, MaxY - MinY + 1);
            EllipseDims = BoundBox.Size;
            //Area of an ellipse = a*b*PI
            //We can't assume that the ellipse contains all of the specified points (e.g. hollow rectangle)
            //So we need to count which ones it does contain
            int[] temp;
            int count;
            return Constants.CalcFit(
                count = Pts.Count(
                p => p.X >= 
                    (temp = GetXs(mEllHeight, mEllWidth, mImgSec.Height, mImgSec.Width, (int)p.Y, mRotation))[0] && p.X <= temp[1]),
                    (int)(BoundBox.Width / 2f * BoundBox.Height / 2f * Math.PI - count));


            //The following is brilliant but stupid
            //I'd love to follow up on all of this
            //Basically, it creates an ellipse from four points
            //But it's getting too long, so, for now, I'll cheap out and just fit the bounding box
            /*
             * http://mathforum.org/library/drmath/view/54485.html
             * http://www.algebra.com/algebra/homework/equations/THEO-20100329.lesson
             * Correlation between nonstandard
             * ax^2+cy^2+dx+ey+f=0
             * and standard
             * (x-H)^2/A^2+(y-K)^2/B^2=1
             * 
             * H = -d/(2a)
             * K = -e/(2c)
             * A^2 = (-f+d^2/(4a)+e^2/(4c))/a
             * B^2 = (-f+d^2/(4a)+e^2/(4c))/c
             * 
             * Now, based on that relationship, and that A > width/2, you can deduce
             * a=(-4f+d^2/a+e^2/2)/w^2
             * Where w is the maximum width needed for the ellipse
             * Now to solve for all the others: since we'll be working with four points, we can deduce four variables, C, D, E, and F
             * f = -a*k^2-c*h^2-d*k-e*h
             * c = -(a*m^2+d*m+f+e*n)/n^2
             * d = -(a*o^2+p*(c*p+e)+f)/o
             * e = -(a*q^2+c*r^2+d*q+f)/r
             * 
             * For each one of those, plug in a different point
             * 
             * f=-a*x1^2-c*y1-d*x1-e*y1
             * c=-(a*x2^2+d*x2+f+e*y2)/y2^2
             *  substitute and solve for f
             *  f=-a*x1^2-(-(a*x2^2+d*x2+f+e*y2)/y2^2)*y1-d*x1-e*y1
             *  
             *  if x1=k, y1=h, x2=m, y2=n, x3=o, y3=p, x4=q, y4=r
             *  
             *  f=-a*k^2-(-(a*m^2+d*m+f+e*n)/n^2)*h-d*k-e*h
             *  f=(a*(k^2 n^2-h m^2)+d (k n^2-h m)+e h (n-1) n)/(h-n^2)
             *  f=(((k*n^2-h*m)*(-(p*((p*(-(a*m^2)-d*m-f-e*n))/n^2+e))-a*o^2-f))/o+a*(k^2*n^2-h*m^2)+e*h*(n-1)*n)/(h-n^2)
             *  f=((a*(k^2*n^2-h*m^2))/(h-n^2)+(a*m^2*p^2*(k*n^2-h*m))/(n^2*o*(h-n^2))-(a*o*(k*n^2-h*m))/(h-n^2)+(d*m*p^2*(k*n^2-h*m))/(n^2*o*(h-n^2))+(g*p^2*(k*n^2-h*m))/(n*o*(h-n^2))-(g*p*(k*n^2-h*m))/(o*(h-n^2))+(g*h*(n-1)*n)/(h-n^2))/(-(p^2*(k*n^2-h*m))/(n^2*o*(h-n^2))+(k*n^2-h*m)/(o*(h-n^2))+1)
             *  f = (f1+f2)/f3
             *   f1 = (a*(k^2*n^2-h*m^2))/(h-n^2)+(a*m^2*p^2*(k*n^2-h*m))/(n^2*o*(h-n^2))-(a*o*(k*n^2-h*m))/(h-n^2)+(d*m*p^2*(k*n^2-h*m))/(n^2*o*(h-n^2))
             *   f2 = (g*p^2*(k*n^2-h*m))/(n*o*(h-n^2))-(g*p*(k*n^2-h*m))/(o*(h-n^2))+(g*h*(n-1)*n)/(h-n^2)
             *   f3 = (-(p^2*(k*n^2-h*m))/(n^2*o*(h-n^2))+(k*n^2-h*m)/(o*(h-n^2))+1)
             *   
             * finding c and e in terms of d,
             * c = (-(m (-a o^2-p (c p+e)-f))/o-a m^2-f-e n)/n^2
             * e = (-a o^2 q+a o q^2+c o r^2-c p^2 q+f o-f q)/(p q-o r)
             * combine
             * c = (-(m (-p ((-a o^2 q+a o q^2+c o r^2-c p^2 q+f o-f q)/(p q-o r)+c p)-a o^2-f))/o-(n (-a o^2 q+a o q^2+c o r^2-c p^2 q+f o-f q))/(p q-o r)-a m^2-f)/n^2
             * or, in terms of c
             * 
             * 
             * 
             * so 
             */

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EllHeight">Relative ellipse height</param>
        /// <param name="EllWidth">Relative ellipse width</param>
        /// <param name="RectHeight">Absolute, from top to bottom of image</param>
        /// <param name="RectWidth">Absolute, from left to right of image</param>
        /// <param name="Y">The row to find the x's for</param>
        /// <param name="theta">The rotation of the ellipse in radians</param>
        /// <returns></returns>
        private static int[] GetXs(double EllHeight, double EllWidth, int RectHeight, int RectWidth, int Y, double theta)
        {
            //This is the more intuitive equation, but I've inline temp'ed the actual code to make it potentially more efficient
            //double 
            //    h = RectWidth / 2d,
            //    k = RectHeight / 2d,
            //    y = (double)Y,
            //    p = EllWidth,
            //    q = EllHeight;
            ////This formula obtained by solving (x-h)^2/p^2+(y-k)^2/q^2=1, where h = Center.X, k = Center.Y, p = semi-width, and q = semi-height
            //double det = Math.Sqrt(5 * Math.Pow(h, 2) + (Math.Pow(y * Math.Sin(theta) - k, 2) / Math.Pow(q, 2) - 1d) * Math.Pow(p, 2)) / 2d;
            //return new int[] { (int)((h + det) / Math.Cos(theta)), (int)((h - det) / Math.Cos(theta)) };
            try
            {
            double
                h = RectWidth / 2d,
                k = RectHeight / 2d;
            //This formula obtained by solving (x-h)^2/p^2+(y-k)^2/q^2=1, where h = Center.X, k = Center.Y, p = semi-width, and q = semi-height
            double det = Math.Sqrt(5 * Math.Pow(h, 2) + (Math.Pow((double)Y * Math.Sin(theta) - k, 2) / Math.Pow(EllHeight, 2) - 1d) * Math.Pow(EllWidth, 2)) / 2d;
            return new int[] { (int)((h + det) / Math.Cos(theta)), (int)((h - det) / Math.Cos(theta)) };
            }
            catch
            {
                return new int[] { -1, -2 };
            }
        }

        public override void Draw(Graphics g)
        {
            g.RotateTransform((float)-mRotation);
            Point[] p = new Point[]{mImgSec.Location};
            g.Transform.TransformPoints(p);
            if(g.Transform != new System.Drawing.Drawing2D.Matrix() && p[0] == mImgSec.Location)
                throw new OperationCanceledException("Ellipse.Draw: the rotation of the array affects the referenced items in that array: fix");
            g.FillEllipse(new SolidBrush(this.mImgSec.MedianCol), new System.Drawing.Rectangle(p[0], new Size((int)mEllWidth, (int)mEllHeight)));
            g.RotateTransform((float)mRotation);
            base.Draw(g);
        }
    }
}
