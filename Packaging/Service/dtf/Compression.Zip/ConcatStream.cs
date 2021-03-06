﻿//---------------------------------------------------------------------
// <copyright file="ConcatStream.cs" company="Microsoft">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
//
//    The use and distribution terms for this software are covered by the
//    Common Public License 1.0 (http://opensource.org/licenses/cpl1.0.php)
//    which can be found in the file CPL.TXT at the root of this distribution.
//    By using this software in any fashion, you are agreeing to be bound by
//    the terms of this license.
//
//    You must not remove this notice, or any other, from this software.
// </copyright>
// <summary>
// Part of the Deployment Tools Foundation project.
// </summary>
//---------------------------------------------------------------------

namespace CoApp.Packaging.Service.dtf.Compression.Zip
{
    using System;
    using System.IO;

    /// <summary>
    /// Used to trick a DeflateStream into reading from or writing to
    /// a series of (chunked) streams instead of a single steream.
    /// </summary>
    internal class ConcatStream : Stream
    {
        private Stream source;
        private long position;
        private long length;
        private Action<ConcatStream> nextStreamHandler;

        internal ConcatStream(Action<ConcatStream> nextStreamHandler)
        {
            if (nextStreamHandler == null)
            {
                throw new ArgumentNullException("nextStreamHandler");
            }

            this.nextStreamHandler = nextStreamHandler;
            this.length = Int64.MaxValue;
        }

        internal Stream Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override long Length
        {
            get
            {
                return this.length;
            }
        }

        public override long Position
        {
            get { return this.position; }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.source == null)
            {
                this.nextStreamHandler(this);
            }

            count = (int) Math.Min(count, this.length - this.position);

            int bytesRemaining = count;
            while (bytesRemaining > 0)
            {
                if (this.source == null)
                {
                    throw new InvalidOperationException();
                }

                int partialCount = (int) Math.Min(bytesRemaining,
                    this.source.Length - this.source.Position);

                if (partialCount == 0)
                {
                    this.nextStreamHandler(this);
                    continue;
                }

                partialCount = this.source.Read(
                    buffer, offset + count - bytesRemaining, partialCount);
                bytesRemaining -= partialCount;
                this.position += partialCount;
            }

            return count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.source == null)
            {
                this.nextStreamHandler(this);
            }

            int bytesRemaining = count;
            while (bytesRemaining > 0)
            {
                if (this.source == null)
                {
                    throw new InvalidOperationException();
                }

                int partialCount = (int) Math.Min(bytesRemaining,
                    Math.Max(0, this.length - this.source.Position));

                if (partialCount == 0)
                {
                    this.nextStreamHandler(this);
                    continue;
                }

                this.source.Write(
                    buffer, offset + count - bytesRemaining, partialCount);
                bytesRemaining -= partialCount;
                this.position += partialCount;
            }
        }

        public override void Flush()
        {
            if (this.source != null)
            {
                this.source.Flush();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            this.length = value;
        }

        public override void Close()
        {
            if (this.source != null)
            {
                this.source.Close();
            }
        }
    }
}
