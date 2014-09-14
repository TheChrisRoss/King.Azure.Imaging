﻿namespace King.Azure.Imaging.Unit.Test
{
    using ImageProcessor.Imaging.Formats;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class ImagingTests
    {
        [Test]
        public void Constructor()
        {
            new Imaging();
        }

        [Test]
        public void IsIImaging()
        {
            Assert.IsNotNull(new Imaging() as IImaging);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SizeDataNull()
        {
            var i = new Imaging();
            i.Size(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SizeDataEmpty()
        {
            var i = new Imaging();
            i.Size(new byte[0]);
        }

        [Test]
        public void Size()
        {
            var file = Environment.CurrentDirectory + @"\icon.png";
            var bytes = File.ReadAllBytes(file);

            var i = new Imaging();
            var size = i.Size(bytes);

            Assert.IsNotNull(size);
            var bitMap = new Bitmap(file);
            Assert.AreEqual(bitMap.Width, size.Width);
            Assert.AreEqual(bitMap.Height, size.Height);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ResizeDataNull()
        {
            var i = new Imaging();
            i.Resize(null, new ImageVersion());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ResizeDataEmpty()
        {
            var i = new Imaging();
            i.Resize(new byte[0], new ImageVersion());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResizeVersionNull()
        {
            var i = new Imaging();
            i.Resize(new byte[123], null);
        }

        [Test]
        public void Resize()
        {
            var file = Environment.CurrentDirectory + @"\icon.png";
            var bytes = File.ReadAllBytes(file);
            var version = new ImageVersion()
            {
                Format = new GifFormat(),
                Width = 10,
                Height = 10,
            };

            var i = new Imaging();
            var data = i.Resize(bytes, version);

            Assert.IsNotNull(data);
            var size = i.Size(data);
            Assert.AreEqual(version.Width, size.Width);
            Assert.AreEqual(version.Height, size.Height);
        }
    }
}