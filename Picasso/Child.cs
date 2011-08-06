#define GUIOUTPUT
//#define MULTITHREAD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
#if MULTITHREAD
using System.Threading.Tasks;
#endif

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
 * This file's purpose: This is one of the most important pieces, because it
 * is the definition of a basic shape. It takes a image section with arbitrarily
 * selected pixels and tries to find the closest possible corresponding geometric
 * shape. That shape has a single color. When looking for children within a child,
 * note that it retrieves the original pixels from the master. This is because
 * the child's image section has been reduced to a single color, so the original
 * detail is needed to most effectively break the child into sub-children.
 * 
 * Comments:
 * Bugs: 
 * 
 */

namespace Picasso
{
    public abstract class Child
    {
#if GUIOUTPUT
        static ProgressDisplay mDisp = new ProgressDisplay();
        static int mScannedPx = 0;
        static Child()
        {
            mDisp.Show();
            RegisterChild((c, i) => new Rectangle(c, i), new Func<ImageSection, double>(Rectangle.ApproxFit));
            RegisterChild((c, i) => new Triangle(c, i), new Func<ImageSection, double>(Triangle.ApproxFit));
            RegisterChild((c, i) => new Ellipse(c, i), new Func<ImageSection, double>(Ellipse.ApproxFit));
        }

        static void UpdateDisplay(int Scanned, int Tot, int NewChild)
        {
            mDisp.Update(Scanned, Tot, NewChild);
        }
#endif

        protected ImageSection mImgSec;
        protected Point[] mVertices;
        protected int mZIndex;
        private Child mParent;
        //internal static Master sMaster = null;
        private Child[] mSubChildren;

        private static List<Func<ImageSection, double>> sRegisteredChecks = new List<Func<ImageSection, double>>();
        private static List<Func<Child, ImageSection, Child>> sRegisteredConstructs = new List<Func<Child, ImageSection, Child>>();

        /// <summary>
        /// 0 means absolutely no selected pixels can fit into this shape, 1 means all and only selected pixels can fit into this shape. Selection is calculated by Constants.CalcFit(Selected, Unselected)
        /// </summary>
        /// <param name="Img"></param>
        /// <returns></returns>
        protected static double ApproxFit(ImageSection Img)
        { return 0d; } //NEEDS OVERRIDE using new keyword

        internal Child(Child Parent, ImageSection Img)
        {
            mImgSec = Img;
            mParent = Parent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ScaledForgive"></param>
        /// <param name="ScaledDetail"></param>
        /// <param name="MinMargin"></param>
        /// <param name="Count"></param>
        internal void GenerateSubChildren(int ScaledForgive, int ScaledDetail, int MinMargin, int ZIndex = 0)
        {
            mZIndex = ZIndex;
            mSubChildren = Child.ScanImageSection(Master.sMaster.OriginalPixels(this.mImgSec), ScaledForgive, ScaledDetail, MinMargin, this);
            //Count += mSubChildren.Length;
            double Scale;
            foreach (Child c in mSubChildren)
            {
                Scale = Constants.GetScale(c.mImgSec.Size, this.mImgSec.Size);
                c.GenerateSubChildren((int)(Scale * ScaledForgive), (int)(Scale * ScaledDetail), MinMargin, ZIndex+1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal virtual string ToString()
        {
            return "Vertices:\t" + mVertices.ToString() + Constants.EOL
                + "Median Color:\t" + mImgSec.MedianCol.ToString() + Constants.EOL
                + "Z Index:\t" + mZIndex.ToString() + Constants.EOL
                + "Location:\t" + this.mImgSec.Location.ToString() + Constants.EOL
                + "Size:\t" + this.mImgSec.Size.ToString() + Constants.EOL;
        }

        /// <summary>
        /// 
        /// </summary>
        //static Child()
        //{
        //    //if (Master.sMaster == null)
        //    //    throw new TypeInitializationException("Child", new Exception("Set the static field mMaster before instantiating any children"));
        //    //sRegisteredChecks.Add(new Func<ImageSection, double>(Rectangle.ApproxFit));
        //    //sRegisteredChecks.Add(new Func<ImageSection, double>(Triangle.ApproxFit));
        //    //sRegisteredChecks.Add(new Func<ImageSection, double>(Ellipse.ApproxFit));
        //    //RegisterChild((c, i) => new Rectangle(c, i), new Func<ImageSection, double>(Rectangle.ApproxFit));
        //    //RegisterChild((c, i) => new Triangle(c, i), new Func<ImageSection, double>(Triangle.ApproxFit));
        //    //RegisterChild((c, i) => new Ellipse(c, i), new Func<ImageSection, double>(Ellipse.ApproxFit));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Constructor"></param>
        /// <param name="ApproxFit"></param>
        public static void RegisterChild(Func<Child, ImageSection, Child> Constructor, Func<ImageSection, double> ApproxFit)
        {
            sRegisteredConstructs.Add(Constructor);
            sRegisteredChecks.Add(ApproxFit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="ImgSec"></param>
        /// <returns></returns>
        internal static Child FromSection(Child Parent, ImageSection ImgSec)
        {
            Func<ImageSection, double> BestFitter = null;
            double BestFit = 0d;
            double temp;
            sRegisteredChecks.ForEach(f => { if (BestFit < (temp = f(ImgSec))) { BestFitter = f; BestFit = temp; } });

            if (BestFit == 0d) throw new InvalidOperationException("No shapes are registered, or they are not matching any pixels.");

            return sRegisteredConstructs[sRegisteredChecks.IndexOf(BestFitter)](Parent, ImgSec);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImgSec"></param>
        /// <param name="ColorForgive"></param>
        /// <param name="ColorDetail"></param>
        /// <param name="MinMargin"></param>
        /// <returns></returns>
        internal static Child[] ScanImageSection(ImageSection ImgSec, int ColorForgive, int ColorDetail, int MinMargin, Child Parent = null)
        {
#if GUIOUTPUT
            mScannedPx = 0;
            int MaxSize = ImgSec.Width * ImgSec.Height;
            frmImgSecDisplay DisplayOut = null;
            //DisplayOut.Show();
            //Master.sInvokable.Invoke(Master.sAddable, null => {return new frmImgSecDisplay();}, DisplayOut);
            MakeForm NewDisp = delegate() { return new frmImgSecDisplay(); };
            AssignForm AssDisp = delegate(System.Windows.Forms.Form ToAssign) { DisplayOut = (frmImgSecDisplay)ToAssign; };
            Master.sInvokable.Invoke(Master.sAddable, new object[] { NewDisp, AssDisp });
#endif
            //Break the image into ImageSections
            List<ImageSection> Sections = new List<ImageSection>();
            ImageSectionFactory Factory;
            for (int y = 0; y < ImgSec.Height; y++)
            {
                for (int x = 0; x < ImgSec.Width; x++)
                {
                    if (ImgSec.GetPixel(x, y).A == byte.MaxValue)
                    {
                        Factory = new ImageSectionFactory(new Point(x, y), ColorForgive, ColorDetail
#if GUIOUTPUT
                            ,DisplayOut
#endif
                            );
                        Sections.Add(Factory.Recognize(ref ImgSec, x, y));
#if GUIOUTPUT
                        UpdateDisplay((mScannedPx += Factory.SelectedPixels().Count()), MaxSize, 0);
#endif
                    }
                }
            }

            Sections.RemoveAll(imsec => imsec == null);
            //Figure out if it just selected the whole thing. If so, cancel the scan
            if (Sections.Count == 1 && Sections[0].Size == ImgSec.Size)
                return new Child[0];

            //Figure out which ones of those are too small and should be discounted
            for (int remove = 0; remove < Sections.Count; remove++)
            {
                if (Sections[remove].PixelsUsed().Length <= MinMargin) Sections.RemoveAt(remove);
            }
            Sections.TrimExcess();

            // Once that's done, turn the remaining Image Sections into Children
            Child[] Children = new Child[Sections.Count];
#if MULTITHREAD
            Task<Child>[] Tasks = new Task<Child>[Sections.Count];
#endif
            for (int i = 0; i < Sections.Count; i++)
            {
#if MULTITHREAD
                Tasks[i] = new Task<Child>(s => Child.FromSection((Child)((object[])s)[0], (ImageSection)((object[])s)[1]), new object[]{Parent, Sections[i]});
                Tasks[i].Start();
#else
                Children[i] = Child.FromSection(Parent, Sections[i]);
#endif

#if GUIOUTPUT
                UpdateDisplay(MaxSize, MaxSize, 1);
#endif
            }
#if MULTITHREAD
            try
            {
                while (Tasks.Any(t => !t.IsCompleted)) Tasks.First(t => !t.IsCompleted).Wait();
            }
            catch (NullReferenceException) { }
            for (int i = 0; i < Tasks.Length; i++)
            {
                Children[i] = Tasks[i].Result;
            }
#endif

            return Children;
        }

        public static explicit operator ImageSection(Child c)
        {
            return c.mImgSec;
        }

        public virtual void Draw(Graphics g)
        {
            foreach (Child c in mSubChildren)
                c.Draw(g);
        }

        public Child[] Children
        { get { return mSubChildren; } }

    }
}
