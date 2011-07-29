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
 * This file's purpose: Generate ImageSections in a standard way based on an image.
 * 
 * Comments:
 * Bugs: 
 * 
 */

namespace Picasso
{
    public class ImageSectionFactory
    {

        //private Point mBase;
        private List<Point> mAddedPixels = new List<Point>();
        private List<List<byte[]>> mColorMap = new List<List<byte[]>>(); // Each is byte[4], ARGB. A = 255 means selected, < 255 means not
        //Also, note, that it's mColorMap[x][y][color#], so outer array is columns, inner is rows
        private int mForgive, mDetail;
        private System.Drawing.Rectangle mSize;
        private System.Timers.Timer mVisualReport;
        private frmImgSecDisplay mVisDisplay;
        private Action<Point[]> Update;
        private Action Show, Close;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Initial"></param>
        /// <param name="ColorForgiveness"></param>
        /// <param name="ColorDetail"></param>
        internal ImageSectionFactory(Point Initial, int ColorForgiveness, int ColorDetail)
        {
            mForgive = ColorForgiveness;
            mDetail = ColorDetail;
            mSize = new System.Drawing.Rectangle(Initial, new Size());
        }

        internal ImageSectionFactory(Point Initial, int ColorForgiveness, int ColorDetail, frmImgSecDisplay Display)
            : this(Initial, ColorForgiveness, ColorDetail)
        {
            mVisualReport = new System.Timers.Timer(1000d);
            mVisualReport.Elapsed += new System.Timers.ElapsedEventHandler(mVisualReport_Elapsed);
            mVisDisplay = Display;
            Show = new Action(mVisDisplay.Show);
            Update = Add => mVisDisplay.DisplayImage(Master.sMaster.OriginalPixels(Add));
            Close = new Action(mVisDisplay.CloseSafe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mVisualReport_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (mAddedPixels)
            {
                //Update(mAddedPixels.ToArray());
                Update.Invoke(mAddedPixels.ToArray());
            }
        }

        /// <summary>
        /// Checks if a pixel should be added to this collection
        /// </summary>
        /// <param name="Pixel"></param>
        /// <returns></returns>
        internal bool ValidPixel(Color Pixel)
        {
            int max = 0;
            Action<int> Check = i => max = Math.Max(i,max);
            mColorMap.ForEach(l => {l.ForEach(i => {if(i != null)
                Check((Math.Abs(i[1] - Pixel.R) + Math.Abs(i[2] - Pixel.G) + Math.Abs(i[3] - Pixel.B))/mDetail);});});
            //for each pixel, take math.abs(pixel.r - color.r) + math.abs(pixel.g - ...)... all / ColorDetail and see if it's greater than current max
            return max <= mForgive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="Pixel"></param>
        internal void AddPixel(Point Location, Color Pixel)
        {
            //Note: Remember to deal with out-of-bounds in all FOUR directions
            if (!mSize.Contains(Location))
            {
                int
                    //XShift = Math.Max(mSize.Left - Location.X, Location.X - mSize.Right + 1),
                    //YShift = Math.Max(mSize.Top - Location.Y, Location.Y - mSize.Bottom + 1);
                    XShift = (Location.X >= mSize.Left && Location.X < mSize.Right ? 0 : (Location.X < mSize.Left ? Location.X - mSize.Left : Location.X - mSize.Right + 1)),
                    YShift = (Location.Y >= mSize.Top && Location.Y < mSize.Bottom ? 0 : (Location.Y < mSize.Top ? Location.Y - mSize.Top : Location.Y - mSize.Bottom + 1));

                if (XShift < 0)
                {
                    mSize.X += XShift;
                    for (int i = 0; i < Math.Abs(XShift); i++)
                        mColorMap.Insert(0, new List<byte[]>());
                }
                if (YShift < 0)
                {
                    mSize.Y += YShift;
                    for (int i = 0; i < Math.Abs(YShift); i++)
                        mColorMap[Location.X - mSize.X].Insert(0, null);
                }

                mSize.Width += Math.Abs(XShift);
                mSize.Height += Math.Abs(YShift);
            }
            //if(Location.X - mSize.X + 1 >= mColorMap.Count)
            int cA = mColorMap.Count;
            for (int i = 0; i < Location.X - mSize.X - cA + 1; i++)
                mColorMap.Add(new List<byte[]>());
            //if (Location.Y - mSize.Y + 1 >= mColorMap[Location.X - mSize.X].Count)
            int cB = mColorMap[Location.X - mSize.X].Count;
            for (int i = 0; i < Location.Y - mSize.Y - cB + 1; i++)
                mColorMap[Location.X - mSize.X].Add(null);

            mAddedPixels.Add(Location);
            mColorMap[Location.X - mSize.X][Location.Y - mSize.Y] = new byte[] { 0xFF, Pixel.R, Pixel.G, Pixel.B };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        internal void SubtractFrom(ref ImageSection Source)
        {
            System.Drawing.Rectangle SourceFill = new System.Drawing.Rectangle(Source.Location, Source.Size);
            foreach (Point p in mAddedPixels)
                if (SourceFill.Contains(p))
                    Source.Alpha.SetPixel(p.X - Source.Location.X, p.Y - Source.Location.Y, Constants.ALPHA_EMPTY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal ImageSection Generate()
        {
            Color Median;
            Bitmap Graphic = CreateGraphic(out Median);
            Bitmap Alpha = CreateAlpha();
            return new ImageSection(mSize.Location, Graphic, Alpha, Median);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Bitmap CreateGraphic(out Color Average)
        {
            Bitmap ToReturn = new Bitmap(mSize.Width, mSize.Height);
            //byte[] MedianData = null;
            ////Try up/down
            //for (int i = 0; i < Math.Min(mSize.Width, mSize.Height) / 2 - 1; i++)
            //{
            //    if (MedianData == null) MedianData = GetPixel(mSize.Width / 2 + i, mSize.Height, mSize.Location);
            //    if (MedianData == null) MedianData = GetPixel(mSize.Width / 2 - i, mSize.Height, mSize.Location);
            //    if (MedianData == null) MedianData = GetPixel(mSize.Width / 2, mSize.Height + i, mSize.Location);
            //    if (MedianData == null) MedianData = GetPixel(mSize.Width / 2, mSize.Height - i, mSize.Location);
            //    else break;
            //}
            ////Else, try diagonals
            //if (MedianData == null)
            //{
            //    for (int i = 0; i < Math.Min(mSize.Width, mSize.Height) / 2 - 1; i++)
            //    {
            //        if (MedianData == null) MedianData = GetPixel(mSize.Width / 2 + i, mSize.Height + i, mSize.Location);
            //        if (MedianData == null) MedianData = GetPixel(mSize.Width / 2 - i, mSize.Height + i, mSize.Location);
            //        if (MedianData == null) MedianData = GetPixel(mSize.Width / 2 + i, mSize.Height - i, mSize.Location);
            //        if (MedianData == null) MedianData = GetPixel(mSize.Width / 2 - i, mSize.Height - i, mSize.Location);
            //        else break;
            //    }
            //}
            ////Else, in desperate attempt, just get the average color
            //if (MedianData == null)
            //{
            //    ulong R = 0, G = 0, B = 0;
            //    Action<byte[]> AddIn = c => { R += c[1]; G += c[2]; B += c[3]; };
            //    mColorMap.ForEach(AL => AL.ForEach(BL => {if(BL != null) AddIn(BL); }));
            //    MedianData = new byte[] { 0xFF, (byte)(R / (ulong)mAddedPixels.Count), (byte)(G / (ulong)mAddedPixels.Count), (byte)(B / (ulong)mAddedPixels.Count) };
            //}
            //Median = Color.FromArgb(MedianData[0], MedianData[1], MedianData[2], MedianData[3]);

            long R = 0, G = 0, B = 0;
            int Count = 0;
            mColorMap.ForEach(a => a.ForEach(b => { if (b != null && b[0] == 255) { Count++; R += b[1]; G += b[2]; B += b[3]; } }));
            Average = Color.FromArgb((int)(R / Count), (int)(G / Count), (int)(B / Count));

            Graphics.FromImage(ToReturn).Clear(Constants.ALPHA_EMPTY);
            foreach (Point p in mAddedPixels)
            {
                ToReturn.SetPixel(p.X - mSize.X, p.Y - mSize.Y, Average);
            }
            return ToReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Bitmap CreateAlpha()
        {
            Bitmap ToReturn = new Bitmap(mSize.Width, mSize.Height);
            Graphics.FromImage(ToReturn).Clear(Constants.ALPHA_EMPTY);
            foreach (Point p in mAddedPixels)
            {
                ToReturn.SetPixel(p.X - mSize.X, p.Y - mSize.Y, Constants.ALPHA_FULL);
            }
            return ToReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GenerateSize()
        {
            mSize = Constants.PtsToRect(mAddedPixels.ToArray());
            //int MinX, MinY, MaxX, MaxY;
            ////Initialize with just something
            //if(mAddedPixels.Count > 0)
            //{
            //    MinX = MaxX = mAddedPixels[0].X;
            //    MinY = MaxY = mAddedPixels[0].Y;
            //}
            //else
            //{
            //    mSize = new System.Drawing.Rectangle();
            //    return;
            //}
            //Action<Point> Checks = p => 
            //{ MinX = Math.Min(MinX, p.X); MinY = Math.Min(MinY, p.Y); MaxX = Math.Max(MaxX, p.X); MaxY = Math.Max(MaxY, p.Y); };

            //for (int i = 1; i < mAddedPixels.Count; i++) //Start with 1, because we've already pulled the first point
            //{
            //    Checks(mAddedPixels[i]);
            //}

            //mSize = new System.Drawing.Rectangle(MinX, MinY, MaxX - MinX + 1, MaxY - MinY + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal Point[] SelectedPixels()
        {
            return mAddedPixels.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Base"></param>
        /// <returns></returns>
        private byte[] GetPixel(int X, int Y, Point Base)
        {
            return (mColorMap[X - Base.X].Count > Y - Base.Y ? mColorMap[X - Base.X][Y - Base.Y] : null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Graphic"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal ImageSection Recognize(ref ImageSection Graphic, int x, int y)
        {
            if (mVisualReport != null && mVisDisplay != null)
            {
                Show.Invoke();
                mVisualReport.Start();
            }
            //We've got something, so now scan to find adjacent pixels.
            //Step 1: diagonal, down-right to get a general square shape
            //Step 2: right from each of diag's to fill right side
            //Step 3: down from each of diag's to fill bottom
            //Step 4: around in circles around that, to pick up stragglers
            Func<Color, Point, bool> CheckAdd = (c, p) => { if (this.ValidPixel(c)) { this.AddPixel(p, c); return true; } return false; };
            int DiagMove = 0;
            while (CheckAdd(Graphic.GetPixel(x + DiagMove, y + DiagMove), new Point(x + DiagMove, y + DiagMove)))
                DiagMove++; //This should work on the first pixel, but change to do{}while if it doesn't

            int SubY, SubX;
            for (SubY = 0; SubY <= DiagMove; SubY++) //See the comment on the next line
                for (SubX = SubY + 1; SubX <= DiagMove - SubY && CheckAdd(Graphic.GetPixel(x + SubX, y + SubY), new Point(x + SubX, y + SubY)); SubX++) ; //Note the semicolon: the logical check does all the needed activity

            //This is just a perpendicular copy of above
            for (SubX = 0; SubX <= DiagMove; SubX++)
                for (SubY = SubX + 1; SubY <= DiagMove - SubX && CheckAdd(Graphic.GetPixel(x + SubX, y + SubY), new Point(x + SubX, y + SubY)); SubY++) ; //Note the semicolon: the logical check does all the needed activity

            //Now the tricky part, the round about
            //I'm going to ignore diagonals
            //For each pixel that's been added so far, check all adjacent pixels not checked yet and checkadd them
            //Add the successes to another list and foreach through that one as well. Repeat.
            { //These parenthesis are just to keep these large array variables from lingering too long
                List<Point> LatestAdds = new List<Point>(), AllTested = new List<Point>();
                LatestAdds.AddRange(this.SelectedPixels());
                Point[] CurrentTesting;
                do
                {
                    AllTested.AddRange(LatestAdds);
                    CurrentTesting = LatestAdds.ToArray();
                    LatestAdds = new List<Point>();
                    foreach (Point p in CurrentTesting)
                    {
                        if (p.X - 1 >= 0 && !LatestAdds.Contains(new Point(p.X - 1, p.Y)) && !AllTested.Contains(new Point(p.X - 1, p.Y)))
                            if (CheckAdd(Graphic.GetPixel(p.X - 1, p.Y), new Point(p.X - 1, p.Y)))
                                LatestAdds.Add(new Point(p.X - 1, p.Y));
                        if (p.Y - 1 >= 0 && !LatestAdds.Contains(new Point(p.X, p.Y - 1)) && !AllTested.Contains(new Point(p.X, p.Y - 1)))
                            if (CheckAdd(Graphic.GetPixel(p.X, p.Y - 1), new Point(p.X, p.Y - 1)))
                                LatestAdds.Add(new Point(p.X, p.Y - 1));
                        if (p.X + 1 >= 0 && !LatestAdds.Contains(new Point(p.X + 1, p.Y)) && !AllTested.Contains(new Point(p.X + 1, p.Y)))
                            if (CheckAdd(Graphic.GetPixel(p.X + 1, p.Y), new Point(p.X + 1, p.Y)))
                                LatestAdds.Add(new Point(p.X + 1, p.Y));
                        if (p.Y + 1 >= 0 && !LatestAdds.Contains(new Point(p.X, p.Y + 1)) && !AllTested.Contains(new Point(p.X, p.Y + 1)))
                            if (CheckAdd(Graphic.GetPixel(p.X, p.Y + 1), new Point(p.X, p.Y + 1)))
                                LatestAdds.Add(new Point(p.X, p.Y + 1));
                    }
                } while (LatestAdds.Count > 0);
            }

            //
            //Almost done
            //
            this.SubtractFrom(ref Graphic);
            if (mVisualReport != null && mVisDisplay != null)
            {
                mVisualReport.Stop();
                Close.Invoke();
            }
            return this.Generate();

            //
            // ALL DONE! One ImageSection down
            //
        }
    }
}
