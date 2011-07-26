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
 * This file's purpose: To define a simple 4-point polygon. Note that this is technically a quadrilateral, not a rectangle.
 * This file also defines the Polygon class.
 * 
 * Comments:
 * Bugs: 
 * 
 */

namespace Picasso
{
    public abstract class Polygon : Child
    {
        protected readonly int mVerticeCount = -1;

        protected Polygon(Child Parent, ImageSection ImgSec, int VertexCount)
            : base(Parent, ImgSec)
        {
            if (VertexCount <= 2) throw new InvalidOperationException("Vertices count must be greater than 2");
            mVerticeCount = VertexCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImgSec"></param>
        /// <returns></returns>
        internal new static double ApproxFit(ImageSection ImgSec)
        {
            return Area(GenerateCorners(ImgSec).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImgSec"></param>
        /// <returns></returns>
        private static List<Point> GenerateCorners(ImageSection ImgSec)
        {
            //Find all points that are clear on at least two sides of any other selected pixels
            List<Point> Outsides;
            {
                Point[] Pts = ImgSec.PixelsUsed();
                Outsides = new List<Point>();
                foreach (Point p in Pts)
                {
                    int Bordering = 0;
                    foreach (Point q in Pts)
                    {
                        if (Math.Abs(p.X - q.X) == 1 || Math.Abs(p.Y - q.Y) == 1) Bordering++;
                        if (Bordering >= 3) break;
                    }
                    if (Bordering <= 2) Outsides.Add(p);
                }
            }

            List<Point> Corners = new List<Point>();

            //Order points around the shape in a roughly circular shape
            //This solution also ensure that the resulting shape is convex
            //I'll take a scan approach
            //Pick a point that you know is on the outside, so any one with min or max x or y
            //I'm going to find min y, then min x for that
            //Then do a scan using that and (-1,-1) to find the point with the SMALLEST angle
            //Guaranteed, those two points are part of the convex containing polygon
            //Using those two points, scan around
            //Scan every other point and its angle with the current two
            //The point with the greatest angle is the next convex vertex
            //Continue until you reach the original point
            {
                Point a, b = new Point(-1, -1), c = new Point(-1, -1);
                double angle = Math.PI, temp;
                //int MinY = Outsides.Min(p => p.Y);
                //MinY = 0;
                a = new Point(Outsides.Min(p => (p.Y == 0 ? p.X : ImgSec.Width)), 0);
                foreach (Point p in Outsides)
                {
                    if (!p.Equals(a))
                        if ((temp = GetAngle(new Point(-1, -1), a, p)) < angle)
                        {
                            b = p;
                            angle = temp;
                        }
                }
                Corners.Add(a);
                Corners.Add(b);
                do
                {
                    angle = 0d;
                    foreach (Point q in Outsides)
                        if (!Corners.Contains(q))
                            if ((temp = GetAngle(Corners[Corners.Count - 2], Corners[Corners.Count - 1], q)) > angle)
                            {
                                c = q;
                                angle = temp;
                            }
                    if (Corners.Contains(c)) break;
                    else Corners.Add(c);
                } while (Corners.Count < Outsides.Count);
            }

            return Corners;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImgSec"></param>
        /// <param name="Vertices"></param>
        /// <returns></returns>
        internal Point[] HardFit(ImageSection ImgSec)
        {
            List<Point> Corners = GenerateCorners(ImgSec);

            //Find the Centroid
            //Func<Point, Point, Point, double> GetAngle = (a, b, c) => { };
            //http://en.wikipedia.org/wiki/Centroid#Centroid_of_polygon
            //Point Centroid;
            //{
            //    int ASigma = 0, XSigma = 0, YSigma = 0, ASigAdd;
            //    for (int i = 0; i < Outsides.Count - 1; i++)
            //    {
            //        ASigAdd = Outsides[i].X * Outsides[i + 1].Y - Outsides[i + 1].X * Outsides[i].Y;
            //        ASigma += ASigAdd;
            //        XSigma += (Outsides[i].X + Outsides[i + 1].X) * ASigAdd;
            //        YSigma += (Outsides[i].Y + Outsides[i + 1].Y) * ASigAdd;
            //    }
            //    double A = 1d / 2d * ASigma;
            //    double
            //        X = 1d / (6d * A) * XSigma,
            //        Y = 1d / (6d * A) * YSigma;
            //    Centroid = new Point((int)X, (int)Y);
            //}

            //Ensure that the whole thing is convex
            //{
            //    Func<Point, Point, Point, bool> IsConcave = (a, b, c) => GetAngle(a, b, Centroid) + GetAngle(Centroid, b, c) > 180d;

            //    for (int k = 0; k < Corners.Count; k++)
            //        if (IsConcave(Corners[k], Corners[(k + 1) % Corners.Count], Corners[(k + 2) % Corners.Count]))
            //        { Corners.RemoveAt((k + 1) % Corners.Count); k--; }
            //}
            while (Corners.Count < mVerticeCount && mVerticeCount != -1)
                Corners.Insert(0, Corners[0]);

            if (Corners.Count > mVerticeCount && mVerticeCount != -1)
            {
                //Insert points as new Point[]{Base1, Base2, Extender2, Extender1}
                List<Point[]> TrianglePossibilities;
                List<double> TriangleScores;

                //Figure out, based on ASA, the area of a triangle; note that the angles are in radians
                //in<Angle1, Side,   Angle2, output>
                Func<double, double, double, double> AreaOfTriangle = (a1, s, a2) => s * s * Math.Sin(a1) * Math.Sin(a2) / Math.Sin(Math.PI - (a1 + a2));
                //a = s*sin(A)/sin(180-(A+B))
                //area = a*s*sin(B)

                //Func<Point, Point, double> Hyp = (p1,p2) => Math.Sqrt(Math.Pow(p1.X - p2.X,2) + Math.Pow(p1.Y - p2.Y,2));
                //Func<Point, Point, Point, double> GetAngle =
                //    (r1, v, r2) =>
                //        Math.Acos((Math.Pow(Hyp(v, r1), 2) + Math.Pow(Hyp(v, r2), 2) + Math.Pow(Hyp(r1, r2), 2)) / (2 * Hyp(v, r1) * Hyp(v, r2)));

                double TempArea, TempBase;
                //while (Corners.Count > Vertices)
                {
                    TrianglePossibilities = new List<Point[]>();
                    TriangleScores = new List<double>();
                    //int b1, b2, e1, e2;
                    for (int i = 0; i < Corners.Count; i++)
                    {
                        //Compare using 1&2, repetition ensures that all possibilities get considered
                        TrianglePossibilities.Add(new Point[] { Corners[(i + 3) % Corners.Count], Corners[i], Corners[(i + 1) % Corners.Count], Corners[(i + 2) % Corners.Count] });
                        //You have to use this formula for area of the triangle, because the peak of the triangle is an unknown point beyond the polygon
                        //Save the efficiency of doing each of these by taking the area * altitude of the full triangle and subtracting the area of the quadrilateral
                        TriangleScores.Add(
                            (TempArea = AreaOfTriangle(
                                GetAngle(Corners[i], Corners[(i + 3) % Corners.Count], Corners[(i + 2) % Corners.Count]),
                                TempBase = Hyp(Corners[(i + 3) % Corners.Count], Corners[i]),
                                GetAngle(Corners[(i + 3) % Corners.Count], Corners[i], Corners[(i + 1) % Corners.Count])))
                                * GetAltitude(TempBase, TempArea)
                                - Area(Corners[i], Corners[(i + 1) % Corners.Count], Corners[(i + 2) % Corners.Count], Corners[(i + 3) % Corners.Count]));
                        //Apologies for the complexity. Note the repetition of Corners[(i + k) % Corners.Count]
                        //k=1=e1, k=2=e2, k=3=b1, k=0=b2
                        //This has to be this way to prevent impossible triangles
                        //I've already checked that the shape is convex, so \_/ is only guaranteed to work as \/
                    }

                    //Based off of y-y2=m(x-x2)
                    //a=b1=p[3], b=b2=p[0], c=e1=p[2], d=e2=p[1], e=a.y, f=b.y, g=c.y, h=d.y
                    //using wolframalpha to solve
                    //x = -(-(c*(e-g))/(a-c)+(d*(f-h))/(b-d)+g-h)/((e-g)/(a-c)-(f-h)/(b-d))
                    //y = -(-a*f*g+a*g*h+b*e*h-b*g*h+c*e*f-c*e*h-d*e*f+d*f*g)/(a*f-a*h-b*e+b*g-c*f+c*h+d*e-d*g)
                    Func<Point[], Point> CalcThirdPoint = p => new Point(
                        -(-(p[2].X * (p[3].Y - p[2].Y)) / (p[3].X - p[2].X) + (p[1].X * (p[0].Y - p[1].Y)) / (p[0].X - p[1].X) + p[2].Y - p[1].Y) / ((p[3].Y - p[2].Y) / (p[3].X - p[2].X) - (p[0].Y - p[1].Y) / (p[0].X - p[1].X)),
                        -(-p[3].X * p[0].Y * p[2].Y + p[3].X * p[2].Y * p[1].Y + p[0].X * p[3].Y * p[1].Y - p[0].X * p[2].Y * p[1].Y + p[2].X * p[3].Y * p[0].Y - p[2].X * p[3].Y * p[1].Y - p[1].X * p[3].Y * p[0].Y + p[1].X * p[0].Y * p[2].Y) / (p[3].X * p[0].Y - p[3].X * p[1].Y - p[0].X * p[3].Y + p[0].X * p[2].Y - p[2].X * p[0].Y + p[2].X * p[1].Y + p[1].X * p[3].Y - p[1].X * p[2].Y));
                    List<int> indices = new List<int>();
                    int index;
                    while (Corners.Count > mVerticeCount)
                    {
                        index = TriangleScores.IndexOf(TriangleScores.Min());
                        Corners.Insert(Corners.IndexOf(TrianglePossibilities[index][2]), CalcThirdPoint(TrianglePossibilities[index]));
                        Corners.Remove(TrianglePossibilities[index][2]);
                        Corners.Remove(TrianglePossibilities[index][3]);
                        TrianglePossibilities.RemoveAt(index);
                        TriangleScores.RemoveAt(index);
                        //Also remove 2 surrounding possibilities, since their estimates and required points no longer exist
                        if (index - 1 >= 0)
                        {
                            TrianglePossibilities.RemoveAt(index - 1);
                            TriangleScores.RemoveAt(index - 1);
                        }
                        if (index + 1 < TrianglePossibilities.Count)
                        {
                            TrianglePossibilities.RemoveAt(index + 1);
                            TriangleScores.RemoveAt(index + 1);
                        }
                    }
                }
            }
            return Corners.ToArray();

            throw new NotImplementedException();
        }

        #region Add To Geometry when you're done here

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pts"></param>
        /// <returns></returns>
        private static double Area(params Point[] Pts)
        {
            int ASigma = 0;
            for (int i = 0; i < Pts.Length - 1; i++)
            {
                ASigma += Pts[i].X * Pts[i + 1].Y - Pts[i + 1].X * Pts[i].Y;
            }
            return 1d / 2d * ASigma;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pts"></param>
        /// <returns></returns>
        private static Point Centroid(Point[] Pts)
        {
            int XSigma = 0, YSigma = 0, ASigAdd;
            for (int i = 0; i < Pts.Length - 1; i++)
            {
                ASigAdd = Pts[i].X * Pts[i + 1].Y - Pts[i + 1].X * Pts[i].Y;
                XSigma += (Pts[i].X + Pts[i + 1].X) * ASigAdd;
                YSigma += (Pts[i].Y + Pts[i + 1].Y) * ASigAdd;
            }
            double A = Area(Pts);
            double
                X = 1d / (6d * A) * XSigma,
                Y = 1d / (6d * A) * YSigma;
            return new Point((int)X, (int)Y);
        }

        //http://stackoverflow.com/questions/1211212/how-to-calculate-an-angle-from-three-points
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double Hyp(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="v"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double GetAngle(Point a, Point v, Point b)
        {
            return Math.Acos((Math.Pow(Hyp(v, a), 2) + Math.Pow(Hyp(v, b), 2) + Math.Pow(Hyp(a, b), 2)) / (2 * Hyp(v, a) * Hyp(v, b)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="Leg1"></param>
        /// <param name="Leg2"></param>
        /// <returns></returns>
        private static double GetAltitude(double Base, double Leg1, double Leg2)
        {
            //Law of cosines
            //C = acos((a^2+b^2-c^2)/2ab)
            //sin(Base->Leg=C) = h/(b->Leg)
            //h = b*sin(C)
            //Base can't be c, because that would make C the top of the triangle, not a side angle
            //Base is a
            //Leg1 is b, Leg2 is c
            //so h=Leg1*acos((Base^2+Leg1^2-Leg2^2)/(2*Base*Leg1))
            return Leg1 * Math.Cos((Math.Pow(Base, 2) + Math.Pow(Leg1, 2) - Math.Pow(Leg2, 2)) / (2 * Base * Leg1));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="Area"></param>
        /// <returns></returns>
        private static double GetAltitude(double Base, double Area)
        {
            //A = 1/2 h b
            //h = 2A/b
            return 2d * Area / Base;
        }

        #endregion

        public override void Draw(Graphics g)
        {
            g.FillPolygon(new SolidBrush(this.mImgSec.MedianCol), mVertices);
            base.Draw(g);
        }
    }

    internal class Rectangle : Polygon
    {
        public Rectangle(Child Parent, ImageSection ImgSec)
            : base(Parent, ImgSec, 4)
        { }

    }
}
