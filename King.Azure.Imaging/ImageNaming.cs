﻿namespace King.Azure.Imaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImageNaming : IImageNaming
    {
        #region Members
        /// <summary>
        /// File Name Format
        /// </summary>
        public const string FileNameFormat = "{0}_{1}.{2}";

        /// <summary>
        /// Dynamic Version Format
        /// </summary>
        public const string DynamicVersionFormat = "{0}_{1}_{2}x{3}";

        /// <summary>
        /// Original
        /// </summary>
        public const string Original = "original";

        /// <summary>
        /// Default Extension
        /// </summary>
        public const string DefaultExtension = "jpeg";

        /// <summary>
        /// Path Format
        /// </summary>
        public const string PathFormat = "{0}/{1}";
        #endregion

        #region Methods
        public string DynamicVersion(string extension, int quality, int width, int height)
        {
            return string.Format(DynamicVersionFormat, extension, quality, width, height).ToLowerInvariant();
        }
        public string FileName(Guid identifier, string version, string extension)
        {
            return string.Format(FileNameFormat, identifier, version, extension).ToLowerInvariant();
        }
        public Guid FromFileName(string file)
        {
            return Guid.Parse(file.Substring(0, file.IndexOf('_')));
        }
        public string Extension(string file)
        {
            return file.Contains('.') ? file.Substring(file.LastIndexOf('.') + 1).ToLowerInvariant() : DefaultExtension;
        }
        public string RelativePath(string folder, string file)
        {
            return string.Format(PathFormat, folder, file).ToLowerInvariant();
        }
        #endregion
    }
}