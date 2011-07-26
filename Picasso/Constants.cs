﻿using System;
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
 * This file's purpose: To define certain constant numbers or objects within the application.
 * 
 * Comments:
 * Bugs: 
 * 
 */

namespace Picasso
{
    internal static class Constants
    {
        internal const int DEFAULT_COLORDETAIL = 25; // How many levels to divide each color channel of the image into
        internal const int DEFAULT_RESOLUTIONDETAIL = 512; // How many divisions to cut the image into, at the longer dimension
        internal const int DEFAULT_MINMARGIN = 2; // How many adjecent blocks must be within the forgiveness range so that a block doesn't get generalized as noise
        internal const int DEFAULT_COLORFORGIVENESS = 4; // The maximum sum number of color intervals difference between two adjacent blocks to be considered the same color
        internal const string EOL = "\r\n";
        internal static readonly Color ALPHA_EMPTY = Color.FromArgb(0, 0, 0, 0);
        internal static readonly Color ALPHA_FULL = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

        internal static readonly Func<Size, Size, double> GetScale =
            (Child, Parent) => Math.Sqrt(((double)Child.Width / (double)Parent.Width + (double)Child.Height / (double)Parent.Height) / 2d);
        internal static readonly Func<int, int, double> CalcFit =
            (Selected, Unselected) => (double)(Selected - Unselected) / (double)Selected;
    }
}