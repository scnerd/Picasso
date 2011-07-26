using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
 * This file's purpose: This is a generic log writer. I wrote it just for this program,
 * so it might be a bit specialized without me noticing it.
 * 
 * Comments: I know this whole project is open source, but with this code file I (DMS) withdraw any
 * copyright of any kind. Please use this code file wherever you want, and don't feel the need to mention me.
 * It's just a generic class.
 * Bugs: 
 * 
 */

namespace Picasso
{
    public class LogWriter : IDisposable
    {
        private const int DEFAULT_BUFFERSIZE = 200;

        private readonly int mBufferSize;
        private string mPath,
            mBuffer = "";
        private TextWriter mWriter;
        private Action mWriteLog;
        private Task mDoWrite;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LogPath"></param>
        /// <param name="AllowOverwrite"></param>
        /// <param name="BufferSize"></param>
        public LogWriter(string LogPath, bool AllowOverwrite = false, int BufferSize = DEFAULT_BUFFERSIZE)
        {
            mPath = LogPath;
            mBufferSize = BufferSize;
            if (!AllowOverwrite && File.Exists(mPath))
                throw new IOException("That log already exists. If you want to overwrite, turn the overwrite flag to TRUE in LogWriter");
            File.CreateText(mPath);
            mWriteLog = new Action(WriteToFile);
            mDoWrite = new Task(mWriteLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        public void Write(string Text)
        {
            mBuffer += Text;
            if (mBuffer.Length > mBufferSize)
                mDoWrite.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteToFile()
        {
            mWriter = new StreamWriter(mPath);
            mWriter.Write(mBuffer);
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        public void WriteLine(string Text)
        {
            Write(Text + mWriter.NewLine);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush()
        {
            mDoWrite.Start();
            mDoWrite.Wait();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            mWriter.Flush();
            mWriter.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Flush();
            Close();
            mWriter.Dispose();
            //Close and dispose of the textwriter
        }
    }
}
