using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation
{
    public class WatershedPixel
    {
        public int X;
        public int Y;
        public int Height;
        // labels the pixel as belonging to a unique basin or as a part of the watershed line
        public int Label;
        // Distance is a work image of distances
        public int Distance;

        public WatershedPixel()
        {
            this.X = -1;
            this.Y = -1;
            this.Height = -1000;
            this.Label = -1000;
            this.Distance = -1000;
        }

        public WatershedPixel(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Height = -1000;
            this.Label = WatershedCommon.INIT;
            this.Distance = 0;
        }

        public WatershedPixel(int x, int y, int height)
        {
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Label = WatershedCommon.INIT;
            this.Distance = 0;
        }

        public override bool Equals(Object obj)
        {
            WatershedPixel p = (WatershedPixel)obj;
            return (X == p.X && X == p.Y);
        }

        public override int GetHashCode()
        {
            return X;
        }
        public override string ToString()
        {
            return "Height = " + Height + "; X = " + X.ToString() + ", Y = " + Y.ToString() +
                   "; Label = " + Label.ToString() + "; Distance = " + Distance.ToString();
        }
    }

    public class WatershedGrayscale : FilterGrayToGray
    {
        #region Variables
        private WatershedPixel FictitiousPixel = new WatershedPixel();
        private int _currentLabel = 0;
        private int _currentDistance = 0;
        private FifoQueue _fifoQueue = new FifoQueue();
        // each pixel can be accesesd from 2 places: a dictionary for faster direct lookup of neighbouring pixels 
        // or from a height ordered list
        // sorted array of pixels according to height
        private List<List<WatershedPixel>> _heightSortedList;
        // string in the form "X,Y" is used as a key for the dictionary lookup of a pixel
        private Dictionary<string, WatershedPixel> _pixelMap;
        private int _watershedPixelCount = 0;
        private int _numberOfNeighbours = 8;
        private bool _borderInWhite;
        private int _pictureWidth = 0;
        private int _pictureHeight = 0;
        #endregion

        #region Constructors
        public WatershedGrayscale()
            : this(8)
        { }

        public WatershedGrayscale(int numberOfNeighbours)
        {
            if (numberOfNeighbours != 8 && numberOfNeighbours != 4)
                throw new Exception("Invalid number of neighbour pixels to check. Valid values are 4 and 8.");
            _borderInWhite = true;
            _numberOfNeighbours = numberOfNeighbours;
            _heightSortedList = new List<List<WatershedPixel>>(256);
            for (int i = 0; i < 256; i++)
                _heightSortedList.Add(new List<WatershedPixel>());
        }
        #endregion

        #region Properties
        /// <summary>
        /// number of neighbours to check for each pixel. valid values are 8 and 4
        /// </summary>
        public int NumberOfNeighbours
        {
            get { return _numberOfNeighbours; }
            set
            {
                if (value != 8 && value != 4)
                    throw new Exception("Invalid number of neighbour pixels to check. Valid values are 4 and 8.");
                _numberOfNeighbours = value;
            }
        }

        /// <summary>
        /// Number of labels/basins found
        /// </summary>
        public int LabelCount
        {
            get { return _currentLabel; }
            set { _currentLabel = value; }
        }

        /// <summary>
        /// True: border is drawn in white. False: border is drawn in black
        /// </summary>
        /// <value></value>
        public bool BorderInWhite
        {
            get { return _borderInWhite; }
            set { _borderInWhite = value; }
        }
        #endregion

        private void CreatePixelMapAndHeightSortedArray(BitmapData data)
        {
            _pictureWidth = data.Width;
            _pictureHeight = data.Height;
            // pixel map holds every pixel thus size of (_pictureWidth * _pictureHeight)
            _pixelMap = new Dictionary<string, WatershedPixel>(_pictureWidth * _pictureHeight);
            unsafe
            {
                int offset = data.Stride - data.Width;
                byte* ptr = (byte*)(data.Scan0);

                // get histogram of all values in grey = height
                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++, ptr++)
                    {
                        WatershedPixel p = new WatershedPixel(x, y, *ptr);
                        // add every pixel to the pixel map
                        _pixelMap.Add(p.X.ToString() + "," + p.Y.ToString(), p);
                        _heightSortedList[*ptr].Add(p);
                    }
                    ptr += offset;
                }
            }
            this._currentLabel = 0;
        }

        private void Segment()
        {
            // Geodesic SKIZ (skeleton by influence zones) of each level height
            for (int h = 0; h < _heightSortedList.Count; h++)
            {
                // get all pixels for current height
                foreach (WatershedPixel heightSortedPixel in _heightSortedList[h])
                {
                    heightSortedPixel.Label = WatershedCommon.MASK;
                    // for each pixel on current height get neighbouring pixels
                    List<WatershedPixel> neighbouringPixels = GetNeighbouringPixels(heightSortedPixel);
                    // initialize queue with neighbours at level h of current basins or watersheds
                    foreach (WatershedPixel neighbouringPixel in neighbouringPixels)
                    {
                        if (neighbouringPixel.Label > 0 || neighbouringPixel.Label == WatershedCommon.WSHED)
                        {
                            heightSortedPixel.Distance = 1;
                            _fifoQueue.AddToEnd(heightSortedPixel);
                            break;
                        }
                    }
                }
                _currentDistance = 1;
                _fifoQueue.AddToEnd(FictitiousPixel);
                // extend basins
                while (true)
                {
                    WatershedPixel p = _fifoQueue.RemoveAtFront();
                    if (p.Equals(FictitiousPixel))
                    {
                        if (_fifoQueue.IsEmpty)
                            break;
                        else
                        {
                            _fifoQueue.AddToEnd(FictitiousPixel);
                            _currentDistance++;
                            p = _fifoQueue.RemoveAtFront();
                        }
                    }
                    List<WatershedPixel> neighbouringPixels = GetNeighbouringPixels(p);
                    // labelling p by inspecting neighbours
                    foreach (WatershedPixel neighbouringPixel in neighbouringPixels)
                    {
                        // neighbouringPixel belongs to an existing basin or to watersheds
                        // in the original algorithm the condition is:
                        //   if (neighbouringPixel.Distance < currentDistance && 
                        //      (neighbouringPixel.Label > 0 || neighbouringPixel.Label == WatershedCommon.WSHED))
                        //   but this returns incomplete borders so the this one is used                        
                        if (neighbouringPixel.Distance <= _currentDistance &&
                           (neighbouringPixel.Label > 0 || neighbouringPixel.Label == WatershedCommon.WSHED))
                        {
                            if (neighbouringPixel.Label > 0)
                            {
                                // the commented condition is also in the original algorithm 
                                // but it also gives incomplete borders
                                if (p.Label == WatershedCommon.MASK /*|| p.Label == WatershedCommon.WSHED*/)
                                    p.Label = neighbouringPixel.Label;
                                else if (p.Label != neighbouringPixel.Label)
                                {
                                    p.Label = WatershedCommon.WSHED;
                                    _watershedPixelCount++;
                                }
                            }
                            else if (p.Label == WatershedCommon.MASK)
                            {
                                p.Label = WatershedCommon.WSHED;
                                _watershedPixelCount++;
                            }
                        }
                        // neighbouringPixel is plateau pixel
                        else if (neighbouringPixel.Label == WatershedCommon.MASK && neighbouringPixel.Distance == 0)
                        {
                            neighbouringPixel.Distance = _currentDistance + 1;
                            _fifoQueue.AddToEnd(neighbouringPixel);
                        }
                    }
                }
                // detect and process new minima at height level h
                foreach (WatershedPixel p in _heightSortedList[h])
                {
                    // reset distance to zero
                    p.Distance = 0;
                    // if true then p is inside a new minimum 
                    if (p.Label == WatershedCommon.MASK)
                    {
                        // create new label
                        _currentLabel++;
                        p.Label = _currentLabel;
                        _fifoQueue.AddToEnd(p);
                        while (!_fifoQueue.IsEmpty)
                        {
                            WatershedPixel q = _fifoQueue.RemoveAtFront();
                            // check neighbours of q
                            List<WatershedPixel> neighbouringPixels = GetNeighbouringPixels(q);
                            foreach (WatershedPixel neighbouringPixel in neighbouringPixels)
                            {
                                if (neighbouringPixel.Label == WatershedCommon.MASK)
                                {
                                    neighbouringPixel.Label = _currentLabel;
                                    _fifoQueue.AddToEnd(neighbouringPixel);
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<WatershedPixel> GetNeighbouringPixels(WatershedPixel centerPixel)
        {
            List<WatershedPixel> temp = new List<WatershedPixel>();
            if (_numberOfNeighbours == 8)
            {
                /*
                CP = Center pixel
                (X,Y) -- get all 8 connected 
                |-1,-1|0,-1|1,-1|
                |-1, 0| CP |1, 0|
                |-1,+1|0,+1|1,+1|
                */
                // -1, -1                
                if ((centerPixel.X - 1) >= 0 && (centerPixel.Y - 1) >= 0)
                    temp.Add(_pixelMap[(centerPixel.X - 1).ToString() + "," + (centerPixel.Y - 1).ToString()]);
                //  0, -1
                if ((centerPixel.Y - 1) >= 0)
                    temp.Add(_pixelMap[centerPixel.X.ToString() + "," + (centerPixel.Y - 1).ToString()]);
                //  1, -1
                if ((centerPixel.X + 1) < _pictureWidth && (centerPixel.Y - 1) >= 0)
                    temp.Add(_pixelMap[(centerPixel.X + 1).ToString() + "," + (centerPixel.Y - 1).ToString()]);
                // -1, 0
                if ((centerPixel.X - 1) >= 0)
                    temp.Add(_pixelMap[(centerPixel.X - 1).ToString() + "," + centerPixel.Y.ToString()]);
                //  1, 0
                if ((centerPixel.X + 1) < _pictureWidth)
                    temp.Add(_pixelMap[(centerPixel.X + 1).ToString() + "," + centerPixel.Y.ToString()]);
                // -1, 1
                if ((centerPixel.X - 1) >= 0 && (centerPixel.Y + 1) < _pictureHeight)
                    temp.Add(_pixelMap[(centerPixel.X - 1).ToString() + "," + (centerPixel.Y + 1).ToString()]);
                //  0, 1
                if ((centerPixel.Y + 1) < _pictureHeight)
                    temp.Add(_pixelMap[centerPixel.X.ToString() + "," + (centerPixel.Y + 1).ToString()]);
                //  1, 1
                if ((centerPixel.X + 1) < _pictureWidth && (centerPixel.Y + 1) < _pictureHeight)
                    temp.Add(_pixelMap[(centerPixel.X + 1).ToString() + "," + (centerPixel.Y + 1).ToString()]);
            }
            else
            {
                /*
                CP = Center pixel, N/A = not used
                (X,Y) -- get only 4 connected 
                | N/A |0,-1| N/A |
                |-1, 0| CP |+1, 0|
                | N/A |0,+1| N/A |
                */
                //  -1, 0
                if ((centerPixel.X - 1) >= 0)
                    temp.Add(_pixelMap[(centerPixel.X - 1).ToString() + "," + centerPixel.Y.ToString()]);
                //  0, -1
                if ((centerPixel.Y - 1) >= 0)
                    temp.Add(_pixelMap[centerPixel.X.ToString() + "," + (centerPixel.Y - 1).ToString()]);
                //  1, 0
                if ((centerPixel.X + 1) < _pictureWidth)
                    temp.Add(_pixelMap[(centerPixel.X + 1).ToString() + "," + centerPixel.Y.ToString()]);
                //  0, 1
                if ((centerPixel.Y + 1) < _pictureHeight)
                    temp.Add(_pixelMap[centerPixel.X.ToString() + "," + (centerPixel.Y + 1).ToString()]);
            }
            return temp;
        }

        private void DrawWatershedLines(BitmapData data)
        {
            if (_watershedPixelCount == 0)
                return;

            byte watershedColor = 255;
            if (!_borderInWhite)
                watershedColor = 0;
            unsafe
            {
                int offset = data.Stride - data.Width;
                byte* ptr = (byte*)(data.Scan0);

                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++, ptr++)
                    {
                        // if the pixel in our map is watershed pixel then draw it
                        if (_pixelMap[x.ToString() + "," + y.ToString()].Label == WatershedCommon.WSHED)
                            *ptr = watershedColor;
                    }
                    ptr += offset;
                }
            }
        }

        protected override void ProcessFilter(BitmapData imageData)
        {
            CreatePixelMapAndHeightSortedArray(imageData);
            Segment();
            DrawWatershedLines(imageData);
        }
    }

    public class FifoQueue
    {
        List<WatershedPixel> queue = new List<WatershedPixel>();

        public int Count
        {
            get { return queue.Count; }
        }

        public void AddToEnd(WatershedPixel p)
        {
            queue.Add(p);
        }

        public WatershedPixel RemoveAtFront()
        {
            WatershedPixel temp = queue[0];
            queue.RemoveAt(0);
            return temp;
        }

        public bool IsEmpty
        {
            get { return (queue.Count == 0); }
        }

        public override string ToString()
        {
            return base.ToString() + " Count = " + queue.Count.ToString();
        }
    }

    public class WatershedCommon
    {
        #region Constants
        public const int INIT = -1;
        public const int MASK = -2;
        public const int WSHED = 0;
        #endregion
    }
}