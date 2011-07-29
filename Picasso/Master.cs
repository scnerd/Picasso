//#define GUIOUTPUT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

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
 * This file's purpose: The master is does all the administrative and initial work.
 * It takes a filepath of a picture, copies that picture into a subdirectory with the log,
 * and then kicks the whole process off once requested to.
 * 
 * Comments:
 * Bugs: 
 * 
 */

namespace Picasso
{
    /*
     * This is the original algorithm
     * Note that, during implementation, not every detail got directly translated to code
     * However, perhaps this will help understand generall what I'm doing
     * 
     * -> Image (input)
     *  Reduce to fewer colors
     *  Reduce to grid
     *  Find common shapes in a range of colors
     *   save shapes as individual areas
     *   allow each child to have children
     *   allow each child to get its orig px's for analysis
     *    save relative positions of childred within children
     *    save relative positions of children
     *    compare results with the previous results
     *    look, with margin, for similarities
     *   save image & analysis together
     *    add link to analysis in a registry
     * 
     * Analysis data: txt data for readability
     *  img link
     *  col detail
     *  res detail
     *  min margin
     *  col forgiveness
     *  (doubles)
     *   relative position and size of each child
     *   each child has init & ender, so subs are distinguishable
     *   
     * Shape identifying
     *  split into rectangles that contain a block in which no px > 2 * col forgiveness from any other px
     *  use median color to create an alpha layer
     *  subtract & repeat
     *  remove children smaller than min dimensions
     *  foreach child
     *   use original px's, scaled ((Cw/Iw) + (Ch/Ih))/2 for each detail #
     *   return original algorithm /\
     *   do same for children
     *   
     * Values
     *  Color detail
     *  Resolution detail
     *  Min margin
     *  Color forgiveness
     */

    public class Master
    {
        #region Statics

        internal static Master sMaster;
        internal static Func<Func<System.Windows.Forms.Form>, System.Windows.Forms.Form> sAddable;
        internal static System.Windows.Forms.Form sInvokable;

        private static int NEXTID;
        public static int GetID
        {
            get { return Master.NEXTID++; }
        }

        static Master()
        {
            try
            {
                using (TextReader txt = new StreamReader(File.OpenRead(REGISTERPATH)))
                {
                    NEXTID = int.Parse(txt.ReadLine());
                }
            }
            catch (System.IO.FileNotFoundException)
            { NEXTID = 0; }
            if (!Directory.Exists(DATASUBDIR))
                Directory.CreateDirectory(DATASUBDIR);
        }

        #endregion

        private int mColorDetail = Constants.DEFAULT_COLORDETAIL,
            mResDetail = Constants.DEFAULT_RESOLUTIONDETAIL,
            mMinMargin = Constants.DEFAULT_MINMARGIN,
            mColorForgive = Constants.DEFAULT_COLORFORGIVENESS;

        private const string REGISTERPATH = @"MasterLog.txt",
            DATASUBDIR = @"Data\";
        private string mImagePath, mLogPath;
        private int ID;

        private LogWriter mLogger;
        private Bitmap mBaseImage,
            mLeftover;

        private Child[] mChildren;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageFile"></param>
        public Master(string ImageFile)
        {
            //Child.sMaster = this;
            Master.sMaster = this;
            // Copy image to a set, standard location
            // Generate a log file there
            ID = GetID;
            string Direc = DATASUBDIR + "PicProj " + ID + @"\";
            Directory.CreateDirectory(Direc);
            Image.FromFile(ImageFile).Save(mImagePath = Direc + "image.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            mLogPath = Direc + "log.txt";
            mLogger = new LogWriter(mLogPath,true);
            mBaseImage = (Bitmap)Bitmap.FromFile(mImagePath);
            //mLeftover = (Bitmap)mBaseImage.Clone();
        }

        public Master(string ImageFile, Func<Func<System.Windows.Forms.Form>, System.Windows.Forms.Form> Adder, System.Windows.Forms.Form Invokable)
            : this(ImageFile)
        {
            sAddable = Adder;
            sInvokable = Invokable;
        }

        #region Steps

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Count"></param>
        public void GenerateChildren()
        {
            //int Count = 0;
#if GUIOUTPUT
            double Prog = 0d;
            ProgressDisplay Disp = new ProgressDisplay();
            System.Threading.Tasks.Task t = new System.Threading.Tasks.Task(new Action(Disp.Show));
            t.Start();
#endif
            //Reduce the image into simplicity
            mLeftover = (Bitmap)mBaseImage.Clone();
            ImgManip Man = new ImgManip(ref mLeftover);
            Man.ReduceColors(mColorDetail);
            Man.ReduceGrid(mResDetail);
            mLeftover = Man.GetImage;
            Bitmap Clear = new Bitmap(mLeftover.Width, mLeftover.Height);
            Graphics.FromImage(Clear).Clear(Constants.ALPHA_FULL);
            ImageSection Img = new ImageSection(new Point(0, 0), mLeftover, Clear, Color.Empty);

            mChildren = Child.ScanImageSection(Img, mColorForgive, mColorDetail, mMinMargin);
#if GUIOUTPUT
            Prog = 0.5d;
#endif
            //Count += mChildren.Length;
            double Scale;
            foreach (Child c in mChildren)
            {
                Scale = Constants.GetScale(((ImageSection)c).Size, this.Size);
                c.GenerateSubChildren((int)(Scale * mColorForgive), (int)(Scale * mColorDetail), mMinMargin);
#if GUIOUTPUT
                Prog += 1d / (double)mChildren.Count() * 0.5d;
#endif
            }
#if GUIOUTPUT
            Disp.Close();
            Disp.Dispose();
            t.Dispose();
#endif
        }

        #endregion

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        public int ColorDetail
        {
            get { return mColorDetail; }
            set { mColorDetail = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ResDetail
        {
            get { return mResDetail; }
            set { mResDetail = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MinMargin
        {
            get { return mMinMargin; }
            set { mMinMargin = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ColorForgive
        {
            get { return mColorForgive; }
            set { mColorForgive = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Size Size
        { get { return mBaseImage.Size; } }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        { get { return this.Size.Height; } }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        { get { return this.Size.Width; } }

        #endregion

        public enum RenderState
        {
            Original,
            Negative,
            Base1Shapes,
            FullOverlay,
            EditOrig
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Image Render(RenderState State = RenderState.FullOverlay)
        {
            Image Img = null;
            Graphics g;
            switch (State)
            {
                case RenderState.Original:
                    Img = (Image)mBaseImage.Clone();
                    break;

                case RenderState.FullOverlay:
                    Img = (Image)mBaseImage.Clone();
                    g = Graphics.FromImage(Img);
                    foreach (Child c in mChildren)
                        c.Draw(g);
                    g.Dispose();
                    break;

                case RenderState.Base1Shapes:
                    Img = new Bitmap(mBaseImage.Width, mBaseImage.Height);
                    g = Graphics.FromImage(Img);
                    foreach (Child c in mChildren)
                        c.Draw(g);
                    g.Dispose();
                    break;

                case RenderState.Negative:
                    Img = (Image)mLeftover.Clone();
                    break;

                case RenderState.EditOrig:
                    ImgManip Man = new ImgManip(ref mBaseImage);
                    Man.ReduceColors(mColorDetail);
                    Man.ReduceGrid(mResDetail);
                    Img = Man.GetImage;
                    break;
            }
            return Img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        internal ImageSection OriginalPixels(ImageSection Base)
        {
            Bitmap Clip = new Bitmap(Base.Size.Width, Base.Size.Height);
            Graphics.FromImage(Clip).DrawImageUnscaledAndClipped(mBaseImage, new System.Drawing.Rectangle(Base.Location, Base.Size));
            return new ImageSection(Base.Location, Clip, Base.Alpha, Color.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pts"></param>
        /// <returns></returns>
        internal Bitmap OriginalPixels(Point[] Pts)
        {
            System.Drawing.Rectangle Size = Constants.PtsToRect(Pts);
            Bitmap B = new Bitmap(mBaseImage.Width, mBaseImage.Height);
            foreach (Point p in Pts)
                B.SetPixel(p.X, p.Y, mBaseImage.GetPixel(p.X, p.Y));
            return B;
        }

        public Child[] Children
        {
            get { return mChildren; }
        }
    }
}
